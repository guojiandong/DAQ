using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.V1
{
    public abstract class AbstractClientSocketTask : SocketTask, ISocketClient
    {
        private const bool SUPPORT_MULTI_PACKET = true;
        private ConcurrentQueue<SendSockPacketEventArgs> mSendMsgList;
        private SemaphoreSlim mSemaphoreCounter;

        public int Write_Timeout { get; set; }
        public int Read_Timeout { get; set; }
        public int Delay_Timeout { get; set; }

        public AbstractClientSocketTask(ISynchronizeInvoke control, Socket sock)
            : base(control, sock)
        {
            mSendMsgList = new ConcurrentQueue<SendSockPacketEventArgs>();
            //mRecvBuff = new List<byte>();
            this.Write_Timeout = 100;
            this.Read_Timeout = 2000;
            this.Delay_Timeout = 0;

            mSemaphoreCounter = new SemaphoreSlim(0, int.MaxValue);
        }

        protected override void onCanceled()
        {
            mSemaphoreCounter.Release();
            base.onCanceled();

            mSemaphoreCounter.Release();
        }

        public bool IsConnected()
        {
            return isConnected();
        }

        public abstract bool isConnected();



        protected override void onSocketCreated(Socket sock)
        {
            //sock.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            setSocketKeepAlive(sock, true);
        }

        private void doPushMessage(SendSockPacketEventArgs data)
        {
            if (data == null)
                throw new ArgumentNullException("send can't be empty...");

            mSendMsgList.Enqueue(data);

            mSemaphoreCounter.Release();
        }

        private bool TryDequeue(out SendSockPacketEventArgs data)
        {
            return mSendMsgList.TryDequeue(out data);
        }


        protected bool IsSendBufferEmpty()
        {
            return mSendMsgList.IsEmpty;
        }

        public override int send(byte[] data, int offset, int count, SocketFlags socketFlags)
        {
            if (data == null || count <= 0 || count >= UInt16.MaxValue)
            {
                throw new InvalidOperationException("data can't be null, count must be in renge: 0 ~ " + UInt16.MaxValue);
            }

            SendSockPacketEventArgsWithFlag info = new SendSockPacketEventArgsWithFlag(data, offset, count, getMaxPacketSize() - CONST_PACKET_HEADER_LENGTH);
            info.Flags = socketFlags;
            doPushMessage(info);

            return count;
        }

        public override int send(byte[] data, int offset, int count)
        {
            if (data == null || count <= 0 || count >= UInt16.MaxValue)
            {
                throw new InvalidOperationException("data can't be null, count must be in renge: 0 ~ " + UInt16.MaxValue);
            }

            doPushMessage(new SendSockPacketEventArgs(data, offset, count, getMaxPacketSize() - CONST_PACKET_HEADER_LENGTH));

            return count;
        }


        protected virtual bool CheckWriteOrReadBuffer(Socket sock, byte[] recv_buffer)
        {
            if (!IsSendBufferEmpty() && sock.Poll(this.Write_Timeout, SelectMode.SelectWrite))
            {
                SendSockPacketEventArgs write_data;
                if (TryDequeue(out write_data))
                {
                    int count = write_data.send(sock);
                    onSendDataResult(count, null);
                }
            }
            else if (sock.Poll(this.Read_Timeout, SelectMode.SelectRead))
            {
                int recv_count = sock.Receive(recv_buffer);
                if (recv_count > 0)
                {
                    recv_socket_buffer_helper(sock, recv_buffer, recv_count);
                }
                else
                {
                    if (EnableDebug)
                        System.Diagnostics.Trace.WriteLine(String.Format("{0}::Receive {1}, {2}", GetTag(), recv_count, sock.Connected));

                    return onReadDataError(sock);
                }
            }
            else
            {
                //if (sock.Poll(500, SelectMode.SelectError))
                //{

                //}

                if (!sock.Connected)
                {
                    notifyDisconnectedListener();
                }
                else
                {
                    onReadWriteTimeout();

                    if (this.Delay_Timeout >= 0)
                        mSemaphoreCounter.Wait(this.Delay_Timeout);
                }
            }

            return true;
        }

        protected abstract bool onReadDataError(Socket sock);

        protected virtual void onSendDataResult(int result, byte[] data)
        {
            if (EnableDebug && data != null)
                System.Diagnostics.Trace.WriteLine(String.Format("{0}::onSendDataResult() send data length:{1}, result:{2}", GetTag(), data.Length, result));

        }

        protected virtual void onReadWriteTimeout()
        {

        }

        protected virtual void onDataReceived(byte[] data, int data_length)
        {
            //string recMessage = Encoding.UTF8.GetString(data, 0, data_length);

            if (EnableDebug && data != null)
                System.Diagnostics.Trace.WriteLine(String.Format("{0}::onDataReceived(0) data_length:{1}", GetTag(), data_length));

            notifyDataReceivedListener(data, data_length);
        }

        public override void registerStatusListener(ISocketStatusChangedListener listener)
        {
            base.registerStatusListener(listener);

            //doNotifyStatusListener(listener, this, isConnected());
        }

        private const int MAX_CACHE_COUNT = 5;
        private List<RecvPacketEventArgs> mCacheRecvBuffer = new List<RecvPacketEventArgs>(MAX_CACHE_COUNT);
        private void recv_socket_buffer_helper(Socket sock, byte[] recv_buffer, int recv_count)
        {
            if (SUPPORT_MULTI_PACKET && getMaxPacketSize() > 0 && recv_count > CONST_PACKET_HEADER_LENGTH && SockPacketEventArgs.IsSupportMultiPacket(recv_buffer, 0))
            {
                int src_offset = 0;

                while (!IsCanceled() && recv_count > 0)
                {
                    UInt32 ticker = SockPacketEventArgs.GetTicker(recv_buffer, src_offset);
                    int total_pkg_length = SockPacketEventArgs.GetTotalPacketLength(recv_buffer, src_offset);

                    if (total_pkg_length <= 0)
                    {
                        if (EnableDebug)
                        {
                            System.Diagnostics.Trace.WriteLine(String.Format("{0}::recv_socket_buffer_helper() invalid data length:{1}, recv_count:{2}, ticker:{3}",
                                GetTag(), total_pkg_length, recv_count, ticker));
                        }

                        break;
                    }

                    RecvPacketEventArgs pkg = null;
                    foreach (RecvPacketEventArgs item in mCacheRecvBuffer)
                    {
                        if (ticker == item.Ticker)
                        {
                            pkg = item;
                            break;
                        }
                    }

                    if (pkg == null)
                    {
                        pkg = new RecvPacketEventArgs(ticker, total_pkg_length);

                        mCacheRecvBuffer.Insert(0, pkg);
                        if (mCacheRecvBuffer.Count > MAX_CACHE_COUNT)
                        {
                            mCacheRecvBuffer.RemoveAt(MAX_CACHE_COUNT);
                        }
                    }

                    int count = pkg.SetBuffer(recv_buffer, src_offset);
                    if (count <= 0)
                    {
                        break;
                    }

                    recv_count -= count;
                    src_offset += count;

                    if (pkg.IsFull())
                    {
                        mCacheRecvBuffer.Remove(pkg);

                        onDataReceived(pkg.Buffer, pkg.CurrentLength);
                    }
                }
            }
            else
            {
                onDataReceived(recv_buffer, recv_count);
            }
        }

        private class RecvPacketEventArgs
        {
            public readonly byte[] Buffer;
            public readonly UInt32 Ticker;
            public int CurrentLength;
            public RecvPacketEventArgs(UInt32 ticker, int total_length)
            {
                this.Ticker = ticker;
                this.CurrentLength = 0;
                this.Buffer = new byte[total_length];
            }

            public int SetBuffer(byte[] src_buff, int src_offset)
            {
                int cur_length = SockPacketEventArgs.GetCurrentPacketLength(src_buff, src_offset);
                int cur_index = SockPacketEventArgs.GetPacketIndex(src_buff, src_offset);

                if (cur_length <= 0)
                {
                    return -1;
                }

                Array.Copy(src_buff, src_offset, this.Buffer, cur_index, cur_length);

                CurrentLength += cur_length;

                return (cur_length + CONST_PACKET_HEADER_LENGTH);
            }

            public bool IsFull()
            {
                return (this.CurrentLength >= this.Buffer.Length) ? true : false;
            }
        }

        private const UInt16 CONST_PACKET_HEADER_FLAG = 0xF8FE;
        private const UInt16 CONST_PACKET_HEADER_LENGTH = 16;
        private class SockPacketEventArgs
        {
            public readonly byte[] Buffer;
            public SockPacketEventArgs(UInt32 ticker, UInt16 total_len, UInt16 index, byte[] buff, int offset, int count)
            {
                if (SUPPORT_MULTI_PACKET && total_len > 0)
                {
                    this.Buffer = new byte[CONST_PACKET_HEADER_LENGTH + count];
                    // header flag
                    Array.Copy(BitConverter.GetBytes(CONST_PACKET_HEADER_FLAG), this.Buffer, 2);

                    // ticker
                    Array.Copy(BitConverter.GetBytes(ticker), 0, this.Buffer, 2, 4);

                    //total content length
                    Array.Copy(BitConverter.GetBytes(total_len), 0, this.Buffer, 6, 2);

                    // current content length
                    Array.Copy(BitConverter.GetBytes((UInt16)count), 0, this.Buffer, 8, 2);

                    // index
                    Array.Copy(BitConverter.GetBytes(index), 0, this.Buffer, 10, 2);


                    Array.Copy(buff, offset, this.Buffer, CONST_PACKET_HEADER_LENGTH, count);
                }
                else
                {
                    this.Buffer = new byte[count];
                    Array.Copy(buff, offset, this.Buffer, 0, count);
                }
            }

            public static bool IsSupportMultiPacket(byte[] buff, int offset)
            {
                return (BitConverter.ToUInt16(buff, offset) == CONST_PACKET_HEADER_FLAG) ? true : false;
            }

            public static UInt32 GetTicker(byte[] buff, int offset)
            {
                return BitConverter.ToUInt32(buff, offset + 2);
            }

            public static int GetTotalPacketLength(byte[] buff, int offset)
            {
                return BitConverter.ToUInt16(buff, offset + 6);
            }

            public static int GetCurrentPacketLength(byte[] buff, int offset)
            {
                return BitConverter.ToUInt16(buff, offset + 8);
            }

            public static int GetPacketIndex(byte[] buff, int offset)
            {
                return BitConverter.ToUInt16(buff, offset + 10);
            }

            //public static int CopyPacketContent(byte[] src, int src_offset, byte[] dest, int dest_offset)
            //{
            //    Array.Copy(src, src_offset + CONST_PACKET_HEADER_LENGTH,
            //        dest, dest_offset + GetPacketIndex(src, src_offset),
            //        GetCurrentPacketLength(src, src_offset));
            //}
        }

        private class SendSockPacketEventArgs
        {
            public SockPacketEventArgs[] Pkg;

            public SendSockPacketEventArgs(byte[] buff, int offset, int count, int max_packet_len)
            {
                if (SUPPORT_MULTI_PACKET && max_packet_len > 0)
                {
                    int total_length = count;
                    int pkg_count = total_length / max_packet_len;
                    if ((total_length % max_packet_len) > 0) pkg_count++;

                    Pkg = new SockPacketEventArgs[pkg_count];

                    DateTime dt = DateTime.Now;

                    Random ra = new Random();
                    UInt32 ticker = Convert.ToUInt32(String.Format("{0}{1}{2}{3}{4}",
                                                dt.DayOfYear, dt.Hour, dt.Minute, dt.Second, ra.Next(0, 9)));


                    ticker += (UInt32)ra.Next(1000, 1000000);
                    UInt16 index = 0;
                    for (UInt16 i = 0; i < pkg_count; i++)
                    {
                        count = Math.Min(total_length, max_packet_len);
                        if (count <= 0)
                            break;

                        Pkg[i] = new SockPacketEventArgs(ticker, (UInt16)total_length, index, buff, offset + index, count);

                        index += (UInt16)count;
                        //offset += count;

                        total_length -= count;
                    }
                }
                else
                {
                    Pkg = new SockPacketEventArgs[1];
                    Pkg[0] = new SockPacketEventArgs(0, 0, 0, buff, offset, count);
                }
            }

            public virtual int send(Socket sock)
            {
                if (this.Pkg == null || this.Pkg.Length == 0)
                    return 0;

                int count = 0;
                foreach (SockPacketEventArgs pkg in this.Pkg)
                {
                    count += sock.Send(pkg.Buffer);
                }

                return count;
            }
        }

        private class SendSockPacketEventArgsWithFlag : SendSockPacketEventArgs
        {
            public SocketFlags Flags { get; set; }

            public SendSockPacketEventArgsWithFlag(byte[] buff, int offset, int count, int max_packet_len)
                : base(buff, offset, count, max_packet_len)
            {
                Flags = SocketFlags.None;
            }

            public override int send(Socket sock)
            {
                if (this.Pkg == null || this.Pkg.Length == 0)
                    return 0;

                int count = 0;
                foreach (SockPacketEventArgs pkg in this.Pkg)
                {
                    count += sock.Send(pkg.Buffer, Flags);
                }

                return count;
            }
        }


    }
}
