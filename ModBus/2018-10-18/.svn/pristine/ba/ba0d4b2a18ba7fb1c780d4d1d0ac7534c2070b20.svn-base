using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Sock
{
    public class SingleSocketClientTask : ThreadTask
    {
        private readonly LinkedList<Communicate.Base.IStatusChangedListener> mStatusListenerList;
        private readonly LinkedList<Communicate.Base.IDataReceivedListener> mDataListenerList;

        private ConcurrentQueue<ItemEventArgs> mSendMsgList;

        private AutoResetEvent mSemaphoreCounter = null;

        protected string mRemoteAddress;// { get; set; }
        protected int mRemotePort;// { get; set; }

        public int AutoReconnectTimeout { get; set; }

        private Socket mSock;

        public SingleSocketClientTask(string ip = "localhost", int port = SocketServerTask.DEFAULT_LISTEN_PORT)
        {
            mSendMsgList = new ConcurrentQueue<ItemEventArgs>();
            mStatusListenerList = new LinkedList<Communicate.Base.IStatusChangedListener>();
            mDataListenerList = new LinkedList<Communicate.Base.IDataReceivedListener>();
            mSemaphoreCounter = new AutoResetEvent(false);

            mRemoteAddress = ip;
            mRemotePort = port;

            AutoReconnectTimeout = 2000;
        }

        public string GetRemoteAddress()
        {
            return mRemoteAddress;
        }

        protected override void doProc(object param)
        {
            //throw new NotImplementedException();
            if (mSock == null)
            {
                mSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //onSocketCreated(mSock);
            }

            byte[] recv_buffer = new byte[1024];
            //int recv_count = 0;
            Socket sock = mSock;
            try
            {
                while (!IsCanceled())
                {
                    try
                    {
                        lock (mSemaphoreCounter)
                        {
                            if (mSock == null)
                            {
                                mSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                                //onSocketCreated(mSock);

                                sock = mSock;
                            }
                        }

                        if (!checkConnected(mSock, 200))
                        {
                            mSemaphoreCounter.WaitOne(Math.Max(1000, AutoReconnectTimeout));
                            continue;
                        }


                        CheckWriteOrReadBuffer(mSock, recv_buffer);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        lock (mSemaphoreCounter)
                        {
                            mSock = null;
                        }

                        notifyDisconnectedListener(ex);
                        //if (EnableDebug)
                        System.Diagnostics.Trace.WriteLine("LocalClientTask::doProc remote socket closed:" + ex);
                    }
                    catch (SocketException ex)
                    {
                        //if (EnableDebug)
                            System.Diagnostics.Trace.WriteLine("LocalClientTask::sendOrRecvData SocketException exception:" + ex);

                        if ((ex.SocketErrorCode > SocketError.WouldBlock
                            && ex.SocketErrorCode < SocketError.IsConnected)
                            || ex.SocketErrorCode > SocketError.IsConnected)
                        {
                            lock (mSemaphoreCounter)
                            {
                                mSock = null;
                            }

                            notifyDisconnectedListener(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        //if (EnableDebug)
                            System.Diagnostics.Trace.WriteLine("LocalClientTask::sendOrRecvData Receive exception:" + ex);
                    }
                }
            }
            catch (Exception e)
            {
                //if (EnableDebug)
                    System.Diagnostics.Trace.WriteLine("LocalClientTask::doproc cause exception:" + e);
                //throw;
            }

            lock (mSemaphoreCounter)
            {
                mSock = null;
            }

            sock.Close();

            notifyDisconnectedListener(null);

            System.Diagnostics.Trace.WriteLine("<<<<<<<<<<<<<<<<<LocalClientTask()<<<<<<<<<<<<<<");
        }

        protected virtual bool CheckWriteOrReadBuffer(Socket sock, byte[] recv_buffer)
        {
            //try
            //{
            if (!IsSendBufferEmpty() && sock.Poll(1500, SelectMode.SelectWrite))
            {
                ItemEventArgs write_data;
                if (TryDequeue(out write_data))
                {
                    int count = sock.Send(write_data.Buffer);

                    onSendDataResult(count, write_data.Buffer);
                }
            }
            else if (sock.Poll(2000, SelectMode.SelectRead))
            {
                int recv_count = sock.Receive(recv_buffer);
                if (recv_count > 0)
                {
                    onDataReceived(recv_buffer, recv_count);
                }
                else
                {
                    sock.Disconnect(true);
                    sock.Close();

                    notifyDisconnectedListener(new ObjectDisposedException("remote socket close"));

                    lock (mSemaphoreCounter)
                    {
                        mSock = null;
                    }
                }
            }
            else if (sock.Poll(500, SelectMode.SelectError))
            {
                if (!sock.Connected)
                    notifyDisconnectedListener(null);
            }
            else
            {
                if (!sock.Connected)
                    notifyDisconnectedListener(null);
            }
            return true;
        }

        protected virtual bool checkConnected(Socket sock, int timeout)
        {
            if (!sock.Connected)
            {
                IAsyncResult result = null;
                try
                {
                    if (mRemotePort == 0)
                    {
                        return false;
                    }

                    IPEndPoint ipe = null;
                    if (String.IsNullOrEmpty(mRemoteAddress))
                        ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), mRemotePort);
                    else
                        ipe = new IPEndPoint(IPAddress.Parse(mRemoteAddress), mRemotePort);

#if DEBUG
                    long startTime = DateTime.Now.Ticks;
                    //if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("LocalClientTask::checkConnected(1) connect timeout:{0} time:{1}",
                                timeout, startTime / TimeSpan.TicksPerSecond));
                    }

#endif

#if true
                    result = sock.BeginConnect(ipe, null, null);

                    bool success = result.AsyncWaitHandle.WaitOne(timeout, true);
#if DEBUG
                    //if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("LocalClientTask::checkConnected(3) connect to port:{0}, result:{1}, Connected:{2}, waitResult:{3}, timeout:{4}, time:{5}",
                                mRemotePort, result, sock.Connected, success, timeout, new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(startTime)).Duration().TotalMilliseconds));
                    }

#endif
                    try
                    {
                        sock.EndConnect(result);
                    }
                    catch (Exception)
                    {
                    }

                    result = null;
#else
                    sock.Connect(ipe);
#endif
#if DEBUG
                    //if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("LocalClientTask::checkConnected(3) connect to port : {0} result:{1}",
                                mRemotePort, sock.Connected));
                    }

#endif
                    if (!IsCanceled())
                    {
                        if (sock.Connected)
                        {
                            notifyConnectedListener();
                        }
                    }
                }
                //catch (ObjectDisposedException ex)
                //{
                //    Cancel(true);

                //    if (EnableDebug)
                //    {
                //        System.Diagnostics.Trace.WriteLine(String.Format("ClientSocketTask::checkConnected(7) connect to port : {0}, iscancel:{1} failed:{2}",
                //        mServerPort, isCanceled(), ex));
                //    }          
                //}
                catch (Exception ex)
                {
#if DEBUG
                    //if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("ClientSocketTask::checkConnected(8) connect to port : {0}, iscancel:{1} failed:{2}",
                        mRemotePort, IsCanceled(), ex));
                    }
#endif
                }
                finally
                {
#if DEBUG				
                    //if (EnableDebug)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("ClientSocketTask::checkConnected(9) connect to port : {0}, iscancel:{1} ",
                        mRemotePort, IsCanceled()));
                    }
#endif
                    try
                    {
                        if (result != null)
                            sock.EndConnect(result);
                    }
                    catch (Exception)
                    {

                    }
                }

                return sock.Connected;
            }

            return sock.Connected;
        }

        public void Connect(string ip, int port)
        {
            if (ip == mRemoteAddress && port == mRemotePort)
            {
                System.Diagnostics.Trace.WriteLine("connect() no changed...");
                return;
            }

            mRemoteAddress = ip;
            mRemotePort = port;

            lock (mSemaphoreCounter)
            {
                if (mSock != null && mSock.Connected)
                {
                    //mSock.Disconnect(true);
                    mSock.Close();
                    mSock = null;
                }
            }

            mSemaphoreCounter.Set();
        }

        protected void doPushMessage(ItemEventArgs data)
        {
            if (data == null)
                throw new ArgumentNullException("send can't be empty...");

            mSendMsgList.Enqueue(data);
        }

        protected bool TryDequeue(out ItemEventArgs data)
        {
            return mSendMsgList.TryDequeue(out data);
        }


        protected bool IsSendBufferEmpty()
        {
            return mSendMsgList.IsEmpty;
        }

        protected override void onCanceled()
        {
            mSemaphoreCounter.Set();
            base.onCanceled();

            lock (mSemaphoreCounter)
            {
                if (mSock != null)
                {
                    mSock.Close();
                }
            }
            mSemaphoreCounter.Set();
        }

        public virtual int Send(string data)
        {
            byte[] buff = Encoding.UTF8.GetBytes(data);
            return Send(buff, 0, buff.Length);
        }

        public int Send(byte[] data)
        {
            return Send(data, 0, data.Length);
        }

        public int Send(byte[] data, int offset, int count)
        {
            byte[] buff = new byte[count];
            if (offset > 0)
            {
                for (int i = 0; i < count; i++)
                    buff[i] = data[i + offset];
            }
            else
            {
                Array.Copy(data, buff, count);
            }

            doPushMessage(new ItemEventArgs(buff));
            return count;
        }

        public bool IsConnected()
        {
            lock (mSemaphoreCounter)
            {
                if (mSock != null)
                    return mSock.Connected;
            }

            return false;
        }

        //public abstract string GetRemoteAddress();

        public Socket GetSocket()
        {
            return mSock;
        }

        protected bool onReadDataError(Socket sock)
        {
            return false;
        }

        protected virtual void onSendDataResult(int result, byte[] data)
        {
            //if (EnableDebug)
                System.Diagnostics.Trace.WriteLine(String.Format("{0}::onSendDataResult() send data length:{1}, result:{2}", GetTag(), data.Length, result));

        }
        
        protected virtual void onDataReceived(byte[] data, int data_length)
        {
            //string recMessage = Encoding.UTF8.GetString(data, 0, data_length);

            //if (EnableDebug && data != null)
            //    System.Diagnostics.Trace.WriteLine(String.Format("{0}::onDataReceived(0) data_length:{1}", getTag(), data_length));

            notifyDataReceivedListener(data, data_length);
        }

        #region Implement for IConnection
        public void RegisterDataListener(Communicate.Base.IDataReceivedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("RegisterDataListener(), listener can't be null...");
            }

            lock (mDataListenerList)
            {
                if (mDataListenerList.Contains(listener))
                {
                    return;
                }

                mDataListenerList.AddLast(listener);
            }
        }

        public void UnregisterDataListener(Communicate.Base.IDataReceivedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("unregisterDataListener(), listener can't be null...");
            }
            //throw new NotImplementedException();
            lock (mDataListenerList)
            {
                mDataListenerList.Remove(listener);
            }
        }

        public virtual void RegisterStatusListener(Communicate.Base.IStatusChangedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("registerStatusListener(), listener can't be null...");
            }

            lock (mStatusListenerList)
            {
                if (mStatusListenerList.Contains(listener))
                {
                    return;
                }

                mStatusListenerList.AddLast(listener);
            }
        }

        public void UnregisterStatusListener(Communicate.Base.IStatusChangedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("unregisterStatusListener(), listener can't be null...");
            }
            //throw new NotImplementedException();
            lock (mStatusListenerList)
            {
                mStatusListenerList.Remove(listener);
            }
        }
        #endregion

        private bool doNotifyDataReceivedListener(Communicate.Base.IDataReceivedListener listener, object sender, byte[] buffer, int count)
        {
#if true
            try
            {
                if (typeof(ISynchronizeInvoke).IsAssignableFrom(listener.GetType()) && ((ISynchronizeInvoke)listener).InvokeRequired)
                {
                    ((ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                    {
                        listener.OnDataReceivedCallback(sender, buffer, count);
                    }), null);
                }
                else
                {
                    listener.OnDataReceivedCallback(sender, buffer, count);
                }
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception)
            {
            }
#else
            lock (mListenerList)
            {
                foreach (IDataReceivedListener l in mListenerList)
                    l.onDataReceivedCallback(sender, buffer, count);
                //mListenerList.ForEach( (c) => c.onRecvDataCallback(sock, data)  );
            }
#endif

            return true;
        }

        protected virtual void notifyDataReceivedListener(byte[] buffer, int count)
        {
            notifyDataReceivedListener(this, buffer, count);
        }

        protected virtual void notifyDataReceivedListener(object sender, byte[] buffer, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("notifyDataReceivedListener() data can't be null...");
            }

            lock (mDataListenerList)
            {
                if (mDataListenerList.Count == 0) return;
            }

            byte[] buff = new byte[count];
            Array.Copy(buffer, buff, count);

            lock (mDataListenerList)
            {
                LinkedListNode<Communicate.Base.IDataReceivedListener> nodeNow = mDataListenerList.First;
                while (nodeNow != null)
                {
                    if (!doNotifyDataReceivedListener(nodeNow.Value, sender, buff, count))
                    {
                        LinkedListNode<Communicate.Base.IDataReceivedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mDataListenerList.Remove(nodeDel);
                        continue;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        protected virtual void notifyDisconnectedListener(Exception ex)
        {
            notifyDisconnectedListener(this, ex);
        }

        protected virtual void notifyDisconnectedListener(object sock, Exception ex)
        {
            lock (mStatusListenerList)
            {
                if (mStatusListenerList.Count == 0) return;
            }

            lock (mStatusListenerList)
            {
                LinkedListNode<Communicate.Base.IStatusChangedListener> nodeNow = mStatusListenerList.First;
                while (nodeNow != null)
                {
                    if (!doNotifyStatusListener(nodeNow.Value, sock, false, ex))
                    {
                        LinkedListNode<Communicate.Base.IStatusChangedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mStatusListenerList.Remove(nodeDel);
                        continue;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        //protected void doNotifyDisconnectedListener(IConnection sock)

        protected virtual void notifyConnectedListener()
        {
            notifyConnectedListener(this);
        }

        protected virtual void notifyConnectedListener(object sock)
        {
            lock (mStatusListenerList)
            {
                if (mStatusListenerList.Count == 0) return;
            }

            lock (mStatusListenerList)
            {
                LinkedListNode<Communicate.Base.IStatusChangedListener> nodeNow = mStatusListenerList.First;
                while (nodeNow != null)
                {
                    if (!doNotifyStatusListener(nodeNow.Value, sock, true,  null))
                    {
                        LinkedListNode<Communicate.Base.IStatusChangedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mStatusListenerList.Remove(nodeDel);
                        continue;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        protected bool doNotifyStatusListener(Communicate.Base.IStatusChangedListener listener, object sock, bool isConnected, Exception ex)
        {
            try
            {
                if ((listener is ISynchronizeInvoke)
                    && ((ISynchronizeInvoke)listener).InvokeRequired)
                {
                    ((ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                    {
                        if (isConnected)
                            listener.OnConnected(sock);
                        else
                            listener.OnDisconnected(sock, ex);
                    }), null);
                }
                else
                {
                    if (isConnected)
                        listener.OnConnected(sock);
                    else
                        listener.OnDisconnected(sock, ex);
                }
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception)
            {
            }

            return true;
        }


        protected class ItemEventArgs
        {
            public readonly byte[] Buffer;

            public ItemEventArgs(byte[] buff)
            {
                //this.Sp = sp;
                this.Buffer = buff;
                //this.IsRead = read;
            }
        }
    }
}
