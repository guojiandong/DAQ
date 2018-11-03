using Ksat.AppPlugIn.Logging;
using Ksat.AppPlugIn.Model.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Net
{
    public class TcpSocketSession : AbstractSocketSession
    {
        private const string TAG = "TcpSocketSession";
        
        private IAsyncResult mAsyncResultListener = null;

        public TcpSocketSession(Socket socket) : this(socket, "", 0)
        {
        }

        public TcpSocketSession(Socket socket, string ip, int port) : base(socket, ip, port)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="remoteEndPoint"></param>
        /// <param name="proxy"></param>
        public TcpSocketSession(Socket socket, IPEndPoint remoteEndPoint)
            : base(socket, remoteEndPoint)
        {
        }

        public override void Initialize(AbstractCommunicationProfile profile)
        {
            if(profile != null && (profile is CommunicationSocketTcpClientProfile))
            {
                CommunicationSocketTcpClientProfile p = (CommunicationSocketTcpClientProfile)profile;

                this.SetSocketKeepAlive(p.KeepAliveInterval);

                this.AutoReconnectInterval = p.AutoReconnectInterval;

                if (String.IsNullOrEmpty(this.RemoteIP))
                    this.RemoteIP = p.IpAddress;

                if (RemotePort == 0)
                    this.RemotePort = p.Port;
            }

            Client.NoDelay = true;
            Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

            //throw new NotImplementedException();
        }

        protected virtual void OnSocketClosed()
        {
            NotifySessionStatusChanged(Base.SessionStatus.Disconnected);
        }

        public override int Read(byte[] data, int offset, int length)
        {
            if (!this.IsDisposed) return -1;

            if (this.Client != null &&
                    this.Client.Connected)
            {
                if (this.Client.Poll(10, SelectMode.SelectRead))
                {
                    try
                    {
                        #region
                        int num = this.Client.Receive(data, offset, length, SocketFlags.None);

                        if (num <= 0)
                        {
                            //throw new SocketException((int)SocketError.HostDown);
                            OnSocketClosed();
                        }
                        #endregion

                        return num;
                    }
                    catch (SocketException ex)
                    {
                        NotifySessionStatusChanged(Base.SessionStatus.CauseError, ex);

                        OnSocketClosed();
                        //throw new SocketException((int)SocketError.HostDown);
                    }
                }

                return 0;
            }
            else
            {
                //OnCloseSocket();
                //throw new SocketException((int)SocketError.HostDown);
            }

            return -1;
        }
        
        public override int Write(byte[] data, int offset, int length)
        {
            if (!this.IsDisposed) return -1;

            if (this.Client.Connected)
            {
                try
                {
                    int successNum = 0;
                    int num = 0;
                    while (num < data.Length)
                    {
                        int remainLength = data.Length - num;
                        int sendLength = remainLength >= this.Client.SendBufferSize ? this.Client.SendBufferSize : remainLength;

                        SocketError error;
                        successNum += this.Client.Send(data, num, sendLength, SocketFlags.None, out error);

                        num += sendLength;

                        if (successNum <= 0 || error != SocketError.Success)
                        {
                            throw new SocketException((int)SocketError.HostDown);
                        }
                    }

                    return successNum;
                }
                catch (SocketException ex)
                {
                    NotifySessionStatusChanged(Base.SessionStatus.CauseError, ex);
                    //OnSocketClosed();
                    Logger.Warn(TAG, "Write exeption:"+ex.ToString());
                    //throw;
                }
            }
            else
            {
                OnSocketClosed();
                //throw new SocketException((int)SocketError.HostDown);
            }

            return -1;
        }
        

        public override int TryConnect(int timeout = 0)
        {
            if (IsDisposed || this.Client == null) return -1;

            if (Client.Connected)
                return 1;

            try
            {
                if (RemotePort == 0)
                {
                    return -1;
                }

                IPEndPoint ipe = null;
                if (String.IsNullOrEmpty(RemoteIP))
                    ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), RemotePort);
                else
                    ipe = new IPEndPoint(IPAddress.Parse(RemoteIP), RemotePort);

#if DEBUG
                long startTime = DateTime.Now.Ticks;
                if (EnableLog)
                {
                    Logger.Info(TAG, String.Format("TryConnected(1) connect timeout:{0} time:{1}",
                            timeout, startTime / TimeSpan.TicksPerSecond));
                }
#endif

                if (mAsyncResultListener != null)
                {
                    try
                    {
                        Client.EndConnect(mAsyncResultListener);
                    }
                    catch (Exception)
                    {
                    }
                    mAsyncResultListener = null;
                }

                mAsyncResultListener = Client.BeginConnect(ipe, AsyncCallbackListener, this);

#if DEBUG
                if (EnableLog)
                {
                    Logger.Info(TAG, String.Format("ClientSocketSession::TryConnected(2) port:{0}, result:{1}, obj:{2}, waitResult:{3}, timeout:{4}, time:{5}",
                            RemotePort, mAsyncResultListener, Client.Connected, this, timeout, new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(startTime)).Duration().TotalMilliseconds));
                }

#endif

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
                if (EnableLog)
                {
                    Logger.Info(TAG, String.Format("ClientSocketTask::TryConnected(8) connect to port : {0}, IsDisposed:{1} failed:{2}",
                    RemotePort, IsDisposed, ex));
                }
#endif
            }
            finally
            {
#if DEBUG
                if (EnableLog)
                {
                    Logger.Info(TAG, String.Format("ClientSocketTask::TryConnected(9) connect to port : {0}, Connected:{1} ",
                                RemotePort, Client.Connected));
                }
#endif
            }

            return 0;
        }


        public static void AsyncCallbackListener(IAsyncResult ar)
        {
#if DEBUG
            Logger.Info(TAG, String.Format("ClientSocketSession::AsyncCallbackListener(3), AsyncState : {0}, IsCompleted:{1}",
                        ar.AsyncState, ar.IsCompleted));
#endif

            if (ar.IsCompleted)
            {
                TcpSocketSession session = ((TcpSocketSession)ar.AsyncState);

                session.mAsyncResultListener = null;

                if (!session.IsDisposed)
                {
                    if (session.IsConnected)
                    {
                        session.NotifySessionStatusChanged(Base.SessionStatus.Connected, session);
                    }
                }
            }
        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <param name="firstTimeTick"></param>
        /// <param name="checkTimeTick"></param>
        public virtual void SetSocketKeepAlive(uint firstTimeTick = 0, uint checkTimeTick = 0)
        {
            if (IsDisposed) return;

            uint dummy = 0;
            byte[] inOptionValues = new byte[System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 3];
            if (firstTimeTick > 0 || checkTimeTick > 0)
            {
                if (firstTimeTick == 0)
                {
                    firstTimeTick = checkTimeTick;
                }
                else if (checkTimeTick == 0)
                {
                    checkTimeTick = firstTimeTick;
                }


                BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);//是否启用Keep-Alive
                BitConverter.GetBytes((uint)firstTimeTick).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy));//多长时间开始第一次探测
                BitConverter.GetBytes((uint)checkTimeTick).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 2);//探测时间间隔
            }
            else
            {
                BitConverter.GetBytes((uint)0).CopyTo(inOptionValues, 0);//是否启用Keep-Alive
                BitConverter.GetBytes((uint)10000).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy));//多长时间开始第一次探测
                BitConverter.GetBytes((uint)3000).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 2);//探测时间间隔
            }

            this.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }


#if false
        public override void Initialize(AbstractCommunicationProfile profile)
        {
            //if (Client != null)
            //{
            //    //-------------------初始化心跳检测---------------------//
            //    uint dummy = 0;
            //    _KeepAliveOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            //    _KeepAliveOptionOutValues = new byte[_KeepAliveOptionValues.Length];
            //    BitConverter.GetBytes((uint)1).CopyTo(_KeepAliveOptionValues, 0);
            //    BitConverter.GetBytes((uint)(2000)).CopyTo(_KeepAliveOptionValues, Marshal.SizeOf(dummy));

            //    uint keepAlive = this.Server.ServerConfig.KeepAlive;

            //    BitConverter.GetBytes((uint)(keepAlive)).CopyTo(_KeepAliveOptionValues, Marshal.SizeOf(dummy) * 2);

            //    Client.IOControl(IOControlCode.KeepAliveValues, _KeepAliveOptionValues, _KeepAliveOptionOutValues);

            //    Client.NoDelay = true;
            //    Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            //    //----------------------------------------------------//

            //    Client.ReceiveTimeout = Server.ServerConfig.NetReceiveTimeout;
            //    Client.SendTimeout = Server.ServerConfig.NetSendTimeout;
            //    Client.ReceiveBufferSize = Server.ServerConfig.NetReceiveBufferSize;
            //    Client.SendBufferSize = Server.ServerConfig.NetSendBufferSize;
            //}

            //if (SocketAsyncProxy != null)
            //{
            //    SocketAsyncProxy.Initialize(this);
            //    SocketAsyncProxy.SocketReceiveEventArgsEx.Completed += SocketEventArgs_Completed;
            //    SocketAsyncProxy.SocketSendEventArgs.Completed += SocketEventArgs_Completed;
            //}
        }

        /// <summary>
        /// 异步读取固定长度数据
        /// </summary>
        /// <param name="dataLength"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        protected override Task<byte[]> ReadAsync(int dataLength, CancellationTokenSource cts)
        {
            Task<byte[]> t = Task.Factory.StartNew(() =>
            {
                int readLength = dataLength;
                List<byte> readBytes = new List<byte>(dataLength);
                while (readLength > 0)
                {
                    if (cts.IsCancellationRequested)
                    {
                        break;
                    }

                    SocketAsyncEventArgsEx saeaEx = SocketAsyncProxy.SocketReceiveEventArgsEx;

                    int oneLength = dataLength <= this.Client.ReceiveBufferSize ? dataLength : this.Client.ReceiveBufferSize;

                    try
                    {
                        if (!this.IsDisposed && this.Client != null)
                        {
                            oneLength = this.Client.Receive(saeaEx.ReceiveBuffer, saeaEx.InitOffset, oneLength, SocketFlags.None);
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (SocketException ex)
                    {
                        OnCloseSocket();
                        this.Server.Logger.Error(true, "", ex);
                        break;
                    }

                    saeaEx.DataLength += oneLength;

                    byte[] data = saeaEx.Get();

                    readBytes.AddRange(data);

                    readLength -= oneLength;
                }
                return readBytes.ToArray();
            }, cts.Token);
            return t;
        }

        public override IList<byte[]> Read(IReceiveFilter receiveFilter)
        {
            if (!this.IsDisposed)
            {
                System.Threading.Thread.Sleep(Server.ServerConfig.NetLoopInterval);
                if (this.Client != null &&
                    this.Client.Connected)
                {
                    if (this.Client.Poll(10, SelectMode.SelectRead))
                    {
                        try
                        {
                            SocketAsyncEventArgsEx saeaEx = SocketAsyncProxy.SocketReceiveEventArgsEx;
                            if (saeaEx.NextOffset >= saeaEx.InitOffset + saeaEx.Capacity)
                            {
                                saeaEx.Reset();
                            }

        #region
                            int num = this.Client.Receive(saeaEx.ReceiveBuffer, saeaEx.NextOffset, saeaEx.InitOffset + saeaEx.Capacity - saeaEx.NextOffset, SocketFlags.None);

                            if (num <= 0)
                            {
                                throw new SocketException((int)SocketError.HostDown);
                            }
                            else
                            {
                                LastActiveTime = DateTime.Now;

                                saeaEx.DataLength += num;
                                if (receiveFilter == null)
                                {
                                    IList<byte[]> listBytes = new List<byte[]>();
                                    listBytes.Add(saeaEx.Get());
                                    return listBytes;
                                }
                                else
                                {
                                    return saeaEx.Get(receiveFilter);
                                }
                            }
        #endregion
                        }
                        catch (SocketException)
                        {
                            OnCloseSocket();
                            throw new SocketException((int)SocketError.HostDown);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    OnCloseSocket();
                    throw new SocketException((int)SocketError.HostDown);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 写操作
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override int Write(byte[] data)
        {
            if (!this.IsDisposed)
            {
                if (this.Client.Connected)
                {
                    try
                    {
                        int successNum = 0;
                        int num = 0;
                        while (num < data.Length)
                        {
                            int remainLength = data.Length - num;
                            int sendLength = remainLength >= this.Client.SendBufferSize ? this.Client.SendBufferSize : remainLength;

                            SocketError error;
                            successNum += this.Client.Send(data, num, sendLength, SocketFlags.None, out error);

                            num += sendLength;

                            if (successNum <= 0 || error != SocketError.Success)
                            {
                                throw new SocketException((int)SocketError.HostDown);
                            }
                        }

                        return successNum;
                    }
                    catch (SocketException)
                    {
                        OnCloseSocket();
                        throw;
                    }
                }
                else
                {
                    OnCloseSocket();
                    throw new SocketException((int)SocketError.HostDown);
                }
            }
            else
            {
                return 0;
            }
        }

        public override void TryReceive()
        {
            if (Client != null)
            {
                try
                {
                    bool willRaiseEvent = this.Client.ReceiveAsync(this.SocketAsyncProxy.SocketReceiveEventArgsEx);
                    if (!willRaiseEvent)
                    {
                        ProcessReceive(this.SocketAsyncProxy.SocketReceiveEventArgsEx);
                    }
                }
                catch (Exception ex)
                {
                    this.Server.Logger.Error(true, ex.Message);
                }
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            ISocketSession socketSession = (ISocketSession)e.UserToken;
            if (socketSession != null && socketSession.Client != null)
            {
                try
                {
                    if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                    {
                        SocketAsyncEventArgsEx saeaEx = (SocketAsyncEventArgsEx)e;
                        if (saeaEx.NextOffset >= saeaEx.InitOffset + saeaEx.Capacity)
                        {
                            saeaEx.Reset();
                        }

                        saeaEx.DataLength += saeaEx.BytesTransferred;

                        IList<byte[]> listBytes = InternalReceivePackage(saeaEx);

                        IList<IRequestInfo> listRequestInfos = RequestInfo.ConvertBytesToRequestInfos(Key, Channel,
                            listBytes);

                        if (this.Server.ServerConfig.StartCheckPackageLength)
                        {
                            InternalReceivePackageData(listRequestInfos);
                        }

                        OnSocketReceiveData(new ReceivePackage(RemoteIP, RemotePort, listRequestInfos)); //没有经过检测数据包长度

                        saeaEx.SetBuffer(saeaEx.ReceiveBuffer, saeaEx.NextOffset, saeaEx.InitOffset + saeaEx.Capacity - saeaEx.NextOffset);

                        bool willRaiseEvent = socketSession.Client.ReceiveAsync(this.SocketAsyncProxy.SocketReceiveEventArgsEx);
                        if (!willRaiseEvent)
                        {
                            ProcessReceive(saeaEx);
                        }
                    }
                    else
                    {
                        OnCloseSocket();
                    }
                }
                catch (SocketException ex)
                {
                    OnCloseSocket();
                    this.Server.Logger.Error(true, ex.Message);
                }
                catch (Exception ex)
                {
                    this.Server.Logger.Error(true, ex.Message);
                }
            }
        }

        private IRunDevice InternalCheckCodeDevice(byte[] data)
        {
            IRunDevice dev = null;
            IRunDevice[] devList = this.Server.DeviceManager.GetDevices(CommunicateType.NET);
            if (devList != null && devList.Length > 0)
            {
                if (this.Server.ServerConfig.ControlMode == ControlMode.Loop
                    || this.Server.ServerConfig.ControlMode == ControlMode.Self
                    || this.Server.ServerConfig.ControlMode == ControlMode.Parallel)
                {
                    try
                    {
                        dev = devList.FirstOrDefault(d => d.DeviceParameter.DeviceCode == d.Protocol.GetCode(data));
                    }
                    catch (Exception ex)
                    {
                        this.Server.Logger.Error(true, ex.Message);
                    }
                }
                else if (this.Server.ServerConfig.ControlMode == ControlMode.Singleton)
                {
                    dev = devList[0];
                }
            }
            return dev;
        }

        private IList<byte[]> InternalReceivePackage(SocketAsyncEventArgsEx saeaEx)
        {
            IList<byte[]> listBytes;
            if (this.Server.ServerConfig.StartReceiveDataFliter)
            {
        #region
                byte[] data = new byte[saeaEx.DataLength];
                Buffer.BlockCopy(saeaEx.ReceiveBuffer, saeaEx.InitOffset, data, 0, data.Length);

                IRunDevice dev = InternalCheckCodeDevice(data);

                if (dev != null)
                {
                    listBytes = InternalFliterData(saeaEx, dev);
                }
                else
                {
                    listBytes = InternalFliterData(saeaEx, null);
                }
        #endregion
            }
            else
            {
                listBytes = InternalFliterData(saeaEx, null);
            }
            return listBytes;
        }

        private IList<byte[]> InternalFliterData(SocketAsyncEventArgsEx saeaEx, IRunDevice dev)
        {
            if (dev != null
                && dev.Protocol != null
                && dev.Protocol.ReceiveFilter != null)
            {
                IList<byte[]> listBytes = saeaEx.Get(dev.Protocol.ReceiveFilter);
                if (listBytes != null && listBytes.Count > 0)
                {
                    LastActiveTime = DateTime.Now;
                }
                return listBytes;
            }
            else
            {
                IList<byte[]> listBytes = new List<byte[]>();
                byte[] data = saeaEx.Get();
                if (data.Length > 0)
                {
                    LastActiveTime = DateTime.Now;
                    listBytes.Add(data);
                }
                return listBytes;
            }
        }

        private void InternalReceivePackageData(IList<IRequestInfo> listRequestInfos)
        {
            if (listRequestInfos.Count > 0)
            {
                IRunDevice dev = InternalCheckCodeDevice(listRequestInfos[0].Data);
                if (dev != null)
                {
                    ((RunDevice)dev).InternalReceiveChannelPackageData(this.Channel, listRequestInfos);
                }
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success)
                {
                    if (e.UserToken == null) return;

                    byte[] data = (byte[])e.UserToken;

                    if (e.BytesTransferred < data.Length)
                    {
                        e.SetBuffer(data, e.BytesTransferred, data.Length - e.BytesTransferred);
                        bool willRaiseEvent = this.Client.SendAsync(e);
                        if (!willRaiseEvent)
                        {
                            ProcessSend(e);
                        }
                    }
                    else
                    {
                        e.UserToken = null;
                    }
                }
                else
                {
                    OnCloseSocket();
                }
            }
            catch (SocketException ex)
            {
                OnCloseSocket();
                this.Server.Logger.Error(true, ex.Message);
            }
            catch (Exception ex)
            {
                this.Server.Logger.Error(true, ex.Message);
            }
        }

        protected override void SendAsync(byte[] data)
        {
            if (Client != null)
            {
                try
                {
                    this.SocketAsyncProxy.SocketSendEventArgs.UserToken = data;
                    this.SocketAsyncProxy.SocketSendEventArgs.SetBuffer(data, 0, data.Length);
                    bool willRaiseEvent = this.Client.SendAsync(this.SocketAsyncProxy.SocketSendEventArgs);

                    if (!willRaiseEvent)
                    {
                        ProcessSend(this.SocketAsyncProxy.SocketSendEventArgs);
                    }
                }
                catch (Exception ex)
                {
                    this.Server.Logger.Error(true, ex.Message);
                }
            }
        }

        protected override void SendSync(byte[] data)
        {
            if (Client != null)
            {
                try
                {
                    this.Client.SendData(data);
                }
                catch (SocketException ex)
                {
                    OnCloseSocket();
                    this.Server.Logger.Error(true, ex.Message);
                }
                catch (Exception ex)
                {
                    this.Server.Logger.Error(true, ex.Message);
                }
            }
        }

        protected override void SocketEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    this.Server.Logger.Info(false, "不支持接收和发送的操作");
                    break;
            }
        }
#endif
    }
}
