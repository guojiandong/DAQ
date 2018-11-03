using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base
{
    /// <summary>
    /// base host
    /// </summary>
    public abstract class AbstractSessionBase : ISession
    {
        #region Members
        private long _connectionID = 1000L;
        private readonly ConnectionCollection _listConnections = new ConnectionCollection();
        private readonly SocketAsyncEventArgsPool _saePool = null;
        private readonly LinkedList<ISocketStatusChangedListener> mListenerList;

        protected static Logging.Log sLog;
        #endregion

        #region Constructors
        /// <summary>
        /// new
        /// </summary>
        /// <param name="socketBufferSize"></param>
        /// <param name="messageBufferSize"></param>
        /// <exception cref="ArgumentOutOfRangeException">socketBufferSize</exception>
        /// <exception cref="ArgumentOutOfRangeException">messageBufferSize</exception>
        protected AbstractSessionBase(int socketBufferSize, int messageBufferSize)
        {
            if (socketBufferSize < 1) throw new ArgumentOutOfRangeException("socketBufferSize");
            if (messageBufferSize < 1) throw new ArgumentOutOfRangeException("messageBufferSize");

            sLog = new Logging.Log(this.GetType());

            this.SocketBufferSize = socketBufferSize;
            this.MessageBufferSize = messageBufferSize;
            this._saePool = new SocketAsyncEventArgsPool(messageBufferSize);

            mListenerList = new LinkedList<ISocketStatusChangedListener>();
        }
        #endregion

        protected virtual bool IsServerMode()
        {
            return true;
        }

        #region IHost Members
        /// <summary>
        /// get socket buffer size
        /// </summary>
        public int SocketBufferSize
        {
            get;
            private set;
        }
        /// <summary>
        /// get message buffer size
        /// </summary>
        public int MessageBufferSize
        {
            get;
            private set;
        }


        /// <summary>
        /// create new <see cref="IConnection"/>
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">socket is null</exception>
        public virtual IConnection NewConnection(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            socket.NoDelay = true;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            socket.ReceiveBufferSize = this.SocketBufferSize;
            socket.SendBufferSize = this.SocketBufferSize;
            return new DefaultConnection(this.NextConnectionID(), socket, this);
        }
        /// <summary>
        /// get <see cref="IConnection"/> by connectionID
        /// </summary>
        /// <param name="connectionID"></param>
        /// <returns></returns>
        public IConnection GetConnectionByID(long connectionID)
        {
            return this._listConnections.Get(connectionID);
        }

        public IConnection GetConnectionByTag(string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("invalid tag");
            }

            foreach(IConnection conn in _listConnections.ToArray())
            {
                if(String.Compare(tag, conn.Tag) == 0)
                {
                    return conn;
                }
            }
            return null;
        }
        /// <summary>
        /// list all <see cref="IConnection"/>
        /// </summary>
        /// <returns></returns>
        public IConnection[] ListAllConnection()
        {
            return this._listConnections.ToArray();
        }
        /// <summary>
        /// get connection count.
        /// </summary>
        /// <returns></returns>
        public int CountConnection()
        {
            return this._listConnections.Count();
        }

        /// <summary>
        /// 启动
        /// </summary>
        public virtual void Start()
        {
        }
        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {
            this._listConnections.DisconnectAll();
        }
        #endregion

        #region Protected Methods

        public virtual int NextRequestSeqID()
        {
            return 0;
        }

        /// <summary>
        /// 生成下一个连接ID
        /// </summary>
        /// <returns></returns>
        protected long NextConnectionID()
        {
            return Interlocked.Increment(ref this._connectionID);
        }
        /// <summary>
        /// register connection
        /// </summary>
        /// <param name="connection"></param>
        /// <exception cref="ArgumentNullException">connection is null</exception>
        protected void RegisterConnection(IConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (connection.Active)
            {
                this._listConnections.Add(connection);
                this.OnConnected(connection);
            }
        }
        /// <summary>
        /// OnConnected
        /// </summary>
        /// <param name="connection"></param>
        protected virtual void OnConnected(IConnection connection)
        {
            NotifyStatusListener("OnConnected", connection);
#if DEBUG
            sLog.Info(string.Concat("socket connected, id:", connection.ConnectionID.ToString(),
                ", remot endPoint:", connection.RemoteEndPoint == null ? "unknow" : connection.RemoteEndPoint.ToString(),
                ", local endPoint:", connection.LocalEndPoint == null ? "unknow" : connection.LocalEndPoint.ToString()));
#endif
        }
        /// <summary>
        /// OnStartSending
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="packet"></param>
        protected virtual void OnStartSending(IConnection connection, Packet packet)
        {
        }
        /// <summary>
        /// OnSendCallback
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="packet"></param>
        /// <param name="isSuccess"></param>
        protected virtual void OnSendCallback(IConnection connection, Packet packet, bool isSuccess)
        {
            NotifyStatusListener("OnSendCallback", connection, packet, isSuccess);
        }
        /// <summary>
        /// OnMessageReceived
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="e"></param>
        protected virtual void OnMessageReceived(IConnection connection, MessageReceivedEventArgs e)
        {
        }
        /// <summary>
        /// OnDisconnected
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ex"></param>
        /// <exception cref="ArgumentNullException">connection is null</exception>
        protected virtual void OnDisconnected(IConnection connection, Exception ex)
        {
            this._listConnections.Remove(connection.ConnectionID);

            NotifyStatusListener("OnDisconnected", connection, ex);
#if DEBUG
            sLog.Info(string.Concat("socket disconnected, id:", connection.ConnectionID.ToString(),
                ", remot endPoint:", connection.RemoteEndPoint == null ? "unknow" : connection.RemoteEndPoint.ToString(),
                ", local endPoint:", connection.LocalEndPoint == null ? "unknow" : connection.LocalEndPoint.ToString(),
                ex == null ? string.Empty : string.Concat(", reason is: ", ex.ToString())));
#endif
        }
        /// <summary>
        /// OnError
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ex"></param>
        protected virtual void OnConnectionError(IConnection connection, Exception ex)
        {
            NotifyStatusListener("OnException", connection, ex);

            sLog.Error(ex.Message, ex);
        }
        #endregion
        
        #region notify status
        protected virtual void onRegisteredListener(ISocketStatusChangedListener listener)
        {
            //foreach(IConnection conn in this._listConnections.ToArray())
            //{
            //    this.doNotifyStatusListener(listener, "OnConnected", conn);
            //}
        }

        protected virtual void onUnregisteredListener(ISocketStatusChangedListener listener)
        {
        }

        public void RegisterListener(ISocketStatusChangedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("RegisterListener(), listener can't be null...");
            }

            //if (IsDisposed)
            //{
            //    Logger.Warn(TAG, "RegisterListener() failed, because this object has Disposed");
            //    return;
            //}

            lock (mListenerList)
            {
                LinkedListNode<ISocketStatusChangedListener> nodeNow = mListenerList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Equals(listener))
                    {
                        return;
                    }
                    nodeNow = nodeNow.Next;
                }

                mListenerList.AddLast(listener);
            }

            onRegisteredListener(listener);
        }

        public void UnregisterListener(ISocketStatusChangedListener listener)
        {
            if (listener == null) return;

            bool flag = false;
            lock (mListenerList)
            {
                flag = mListenerList.Remove(listener);
            }

            if(flag)
                onUnregisteredListener(listener);
        }

        #region notify
        public virtual void doNotifyStatusListener(ISocketStatusChangedListener listener, string api, IConnection connection, object p1 = null, object p2 = null)
        {
            if (typeof(System.ComponentModel.ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
                            && ((System.ComponentModel.ISynchronizeInvoke)listener).InvokeRequired)
            {
                ((System.ComponentModel.ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                {
                    if (api == "OnConnected")
                        listener.OnConnected(connection);
                    else if (api == "OnDisconnected")
                        listener.OnDisconnected(connection, (Exception)p1);
                    else if (api == "OnException")
                        listener.OnException(connection, (Exception)p1);
                    else if (api == "OnSendCallback")
                        listener.OnSendCallback(connection, (Base.Packet)p1, (bool)p2);

                }), null);
            }
            else
            {
                if (api == "OnConnected")
                    listener.OnConnected(connection);
                else if (api == "OnDisconnected")
                    listener.OnDisconnected(connection, (Exception)p1);
                else if (api == "OnException")
                    listener.OnException(connection, (Exception)p1);
                else if (api == "OnSendCallback")
                    listener.OnSendCallback(connection, (Base.Packet)p1, (bool)p2);
                //else if (api == "OnReceived")
                //    listener.OnReceived(connection, (Exception)p1);
            }
        }

        //public void NotifyStatusListener(Action<IConnection> connected, Action<IConnection, Exception> excpetion, Action<IConnection, Base.Packet, bool> sendcallback)
        //{
        //    if (typeof(System.ComponentModel.ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
        //                    && ((System.ComponentModel.ISynchronizeInvoke)listener).InvokeRequired)
        //    {
        //        ((System.ComponentModel.ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
        //        {
        //            if (connected != null)
        //                connected();
        //            else if (api == "OnDisconnected")
        //                listener.OnDisconnected(connection, (Exception)p1);
        //            else if (api == "OnException")
        //                listener.OnException(connection, (Exception)p1);
        //            else if (api == "OnSendCallback")
        //                listener.OnSendCallback(connection, (Base.Packet)p1, (bool)p2);

        //        }), null);
        //    }
        //    else
        //    {
        //        if (api == "OnConnected")
        //            listener.OnConnected(connection);
        //        else if (api == "OnDisconnected")
        //            listener.OnDisconnected(connection, (Exception)p1);
        //        else if (api == "OnException")
        //            listener.OnException(connection, (Exception)p1);
        //        else if (api == "OnSendCallback")
        //            listener.OnSendCallback(connection, (Base.Packet)p1, (bool)p2);
        //        //else if (api == "OnReceived")
        //        //    listener.OnReceived(connection, (Exception)p1);
        //    }
        //}


        public void NotifyStatusListener(string api, IConnection connection, object p1 = null, object p2 = null)
        {
            LinkedList<ISocketStatusChangedListener> list;

            lock (mListenerList)
            {
                list = new LinkedList<ISocketStatusChangedListener>(mListenerList);
            }

            LinkedListNode<ISocketStatusChangedListener> nodeNow = list.First;
            while (nodeNow != null)
            {
                try
                {
#if false
                    Console.WriteLine(this.GetType().Name + "::NotifyListener(), filter:" + filter + ", callback filter:" + item.Value);
#endif
                    doNotifyStatusListener(nodeNow.Value, api, connection, p1, p2);

                }
                catch (ObjectDisposedException ex)
                {
                    LinkedListNode<ISocketStatusChangedListener> nodeDel = nodeNow;
                    nodeNow = nodeNow.Next;
                    lock (mListenerList)
                        mListenerList.Remove(nodeDel);

#if DEBUG
                    sLog.Warn("NotifyListener(6) ObjectDisposedException:" + ex.ToString());
#endif
                    continue;
                }
                catch (Exception ex)
                {
#if DEBUG
                    sLog.Warn("NotifyListener(7) exception:" + ex.ToString());
#endif
                }

                nodeNow = nodeNow.Next;
            }
        }


        public void NotifyDataListener<TMessage>(IConnection connection, TMessage message) where TMessage : class, Messaging.IMessage
        {
            LinkedList<ISocketStatusChangedListener> list;

            lock (mListenerList)
            {
                list = new LinkedList<ISocketStatusChangedListener>(mListenerList);
            }

            //lock (mListenerList)
            {
                LinkedListNode<ISocketStatusChangedListener> nodeNow = list.First;
                while (nodeNow != null)
                {
                    try
                    {
                        if(!(nodeNow.Value is ISocketDataChangedListener<TMessage>))
                        {
                            sLog.Warn("NotifyDataListener() listener type:" + nodeNow.Value.GetType());
                            continue;
                        }

                        ISocketDataChangedListener<TMessage> listener = (ISocketDataChangedListener<TMessage>)nodeNow.Value;

                        if (typeof(System.ComponentModel.ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
                                                    && ((System.ComponentModel.ISynchronizeInvoke)listener).InvokeRequired)
                        {
                            ((System.ComponentModel.ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                            {
                                listener.OnReceived(connection, message);
                            }), null);
                        }
                        else
                        {
                            listener.OnReceived(connection, message);
                        }
                    }
                    catch (ObjectDisposedException ex)
                    {
                        LinkedListNode<ISocketStatusChangedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        lock (mListenerList)
                            mListenerList.Remove(nodeDel);

#if DEBUG
                        sLog.Warn("NotifyDataListener(6) ObjectDisposedException:" + ex.ToString());
#endif
                        continue;
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        sLog.Warn("NotifyDataListener(7) exception:" + ex.ToString());
#endif
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        #endregion
        #endregion


        #region DefaultConnection
        /// <summary>
        /// default socket connection
        /// </summary>
        private class DefaultConnection : IConnection
        {
            #region Private Members
            private int _active = 1;
            private DateTime _latestActiveTime = Utils.DateHelper.UtcNow;
            private readonly int _messageBufferSize;
            private readonly AbstractSessionBase _host = null;

            private readonly Socket _socket = null;

            private SocketAsyncEventArgs _saeSend = null;
            private Packet _currSendingPacket = null;
            private readonly PacketQueue _packetQueue = null;

            private SocketAsyncEventArgs _saeReceive = null;
            private MemoryStream _tsStream = null;
            private int _isReceiving = 0;
            //private object lockObject = new object();
            #endregion

            #region Constructors
            /// <summary>
            /// new
            /// </summary>
            /// <param name="connectionID"></param>
            /// <param name="socket"></param>
            /// <param name="host"></param>
            /// <exception cref="ArgumentNullException">socket is null</exception>
            /// <exception cref="ArgumentNullException">host is null</exception>
            public DefaultConnection(long connectionID, Socket socket, AbstractSessionBase host)
            {
                if (socket == null) throw new ArgumentNullException("socket");
                if (host == null) throw new ArgumentNullException("host");

                this.ConnectionID = connectionID;
                this._socket = socket;
                this._messageBufferSize = host.MessageBufferSize;
                this._host = host;

                try
                {
                    this.LocalEndPoint = (IPEndPoint)socket.LocalEndPoint;
                    this.RemoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;

                    //this.Tag = this.LocalEndPoint.ToString();
                }
                catch (Exception ex) { sLog.Error("get socket endPoint error.", ex); }

                //init send
                this._saeSend = host._saePool.Acquire();
                this._saeSend.Completed += this.SendAsyncCompleted;
                this._packetQueue = new PacketQueue(this.SendPacketInternal);

                //init receive
                this._saeReceive = host._saePool.Acquire();
                this._saeReceive.Completed += this.ReceiveAsyncCompleted;
            }
            #endregion

            #region IConnection Members
            /// <summary>
            /// 连接断开事件
            /// </summary>
            public event DisconnectedHandler Disconnected;

            /// <summary>
            /// return the connection is active.
            /// </summary>
            public bool Active
            {
                get { return Thread.VolatileRead(ref this._active) == 1; }
            }
            /// <summary>
            /// get the connection latest active time.
            /// </summary>
            public DateTime LatestActiveTime
            {
                get { return this._latestActiveTime; }
            }


            public string Tag
            {
                get;
                set;
            }

            /// <summary>
            /// get the connection id.
            /// </summary>
            public long ConnectionID { get; private set; }
            /// <summary>
            /// 获取本地IP地址
            /// </summary>
            public IPEndPoint LocalEndPoint { get; private set; }
            /// <summary>
            /// 获取远程IP地址
            /// </summary>
            public IPEndPoint RemoteEndPoint { get; private set; }
            /// <summary>
            /// 获取或设置与用户数据
            /// </summary>
            public object UserData { get; set; }

            /// <summary>
            /// 异步发送数据
            /// </summary>
            /// <param name="packet"></param>
            public void BeginSend(Packet packet)
            {
                if (!this._packetQueue.TrySend(packet))
                    this.OnSendCallback(packet, false);
            }
            /// <summary>
            /// 异步接收数据
            /// </summary>
            public void BeginReceive()
            {
                if (Interlocked.CompareExchange(ref this._isReceiving, 1, 0) == 0)
                    this.ReceiveInternal();
            }
            /// <summary>
            /// 异步断开连接
            /// </summary>
            /// <param name="ex"></param>
            public void BeginDisconnect(Exception ex = null)
            {
                if (Interlocked.CompareExchange(ref this._active, 0, 1) == 1)
                    this.DisconnectInternal(ex);
            }
            #endregion

            #region Private Methods

            #region Free
            /// <summary>
            /// free send queue
            /// </summary>
            private void FreeSendQueue()
            {
                var result = this._packetQueue.Close();
                if (result.BeforeState == PacketQueue.CLOSED) return;

                if (result.Packets != null)
                    foreach (var p in result.Packets) this.OnSendCallback(p, false);

                if (result.BeforeState == PacketQueue.IDLE) this.FreeSend();
            }
            /// <summary>
            /// free for send.
            /// </summary>
            private void FreeSend()
            {
                this._currSendingPacket = null;
                this._saeSend.Completed -= this.SendAsyncCompleted;
                this._host._saePool.Release(this._saeSend);
                this._saeSend = null;
            }
            /// <summary>
            /// free fo receive.
            /// </summary>
            private void FreeReceive()
            {
                this._saeReceive.Completed -= this.ReceiveAsyncCompleted;
                this._host._saePool.Release(this._saeReceive);
                this._saeReceive = null;
                if (this._tsStream != null)
                {
                    this._tsStream.Close();
                    this._tsStream = null;
                }
            }
            #endregion

            #region Fire Events
            /// <summary>
            /// fire StartSending
            /// </summary>
            /// <param name="packet"></param>
            private void OnStartSending(Packet packet)
            {
                this._host.OnStartSending(this, packet);
            }
            /// <summary>
            /// fire SendCallback
            /// </summary>
            /// <param name="packet"></param>
            /// <param name="isSuccess"></param>
            private void OnSendCallback(Packet packet, bool isSuccess)
            {
                if (isSuccess) this._latestActiveTime = Utils.DateHelper.UtcNow;
                else packet.SentSize = 0;

                this._host.OnSendCallback(this, packet, isSuccess);
            }
            /// <summary>
            /// fire MessageReceived
            /// </summary>
            /// <param name="e"></param>
            private void OnMessageReceived(MessageReceivedEventArgs e)
            {
                this._latestActiveTime = Utils.DateHelper.UtcNow;
                this._host.OnMessageReceived(this, e);
            }
            /// <summary>
            /// fire Disconnected
            /// </summary>
            private void OnDisconnected(Exception ex)
            {
                if (this.Disconnected != null) this.Disconnected(this, ex);
                this._host.OnDisconnected(this, ex);
            }
            /// <summary>
            /// fire Error
            /// </summary>
            /// <param name="ex"></param>
            private void OnError(Exception ex)
            {
                this._host.OnConnectionError(this, ex);
            }
            #endregion

            #region Send
            public int NextRequestSeqID()
            {
                return this._host.NextRequestSeqID();
            }

            /// <summary>
            /// internal send packet.
            /// </summary>
            /// <param name="packet"></param>
            /// <exception cref="ArgumentNullException">packet is null</exception>
            private void SendPacketInternal(Packet packet)
            {
                packet.SendCount++;
                this._currSendingPacket = packet;
                this.OnStartSending(packet);
                this.SendPacketInternal(this._saeSend);
            }
            /// <summary>
            /// internal send packet.
            /// </summary>
            /// <param name="e"></param>
            private void SendPacketInternal(SocketAsyncEventArgs e)
            {
                var packet = this._currSendingPacket;

                //按messageBufferSize大小分块传输
                var length = Math.Min(packet.Payload.Length - packet.SentSize, this._messageBufferSize);
                var completedAsync = true;
                try
                {
                    //copy data to send buffer
                    Buffer.BlockCopy(packet.Payload, packet.SentSize, e.Buffer, 0, length);
                    e.SetBuffer(0, length);
                    completedAsync = this._socket.SendAsync(e);
#if false //DEBUG
                    sLog.Warn("SendPacketInternal(2) start send packet:" + packet.Tag
                        + ", seqid:" + packet.SeqID
                        + ", Total Length:" + packet.Payload.Length
                        + ", length:" + length
                        + ", messageBufferSize:" + _messageBufferSize
                        + ", SentSize:" + packet.SentSize
                        + " completedAsync:" + completedAsync);
#endif
                }
                catch (Exception ex)
                {
                    sLog.Warn("SendPacketInternal(3) send packet seqid:"+packet.SeqID+" exception:" + e.SocketError);

                    this.BeginDisconnect(ex);
                    this.FreeSend();
                    this.OnSendCallback(packet, false);
                    this.OnError(ex);
                }

                if (!completedAsync)
                    ThreadPool.QueueUserWorkItem(_ => this.SendAsyncCompleted(this, e));
            }
            /// <summary>
            /// async send callback
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void SendAsyncCompleted(object sender, SocketAsyncEventArgs e)
            {
                var packet = this._currSendingPacket;

                //send error!
                if (e.SocketError != SocketError.Success)
                {
                    sLog.Warn("SendAsyncCompleted(1) send packet error:" + e.SocketError);

                    this.BeginDisconnect(new SocketException((int)e.SocketError));
                    this.FreeSend();
                    this.OnSendCallback(packet, false);
                    return;
                }

                packet.SentSize += e.BytesTransferred;

#if false //DEBUG
                sLog.Info("SendAsyncCompleted(2) send packet success:" + packet.Tag
                    + ", seqid:" + packet.SeqID
                    + ", Total Length:" + packet.Payload.Length
                    + ", BytesTransferred:" + e.BytesTransferred
                    + ", messageBufferSize:" + _messageBufferSize
                    + ", SentSize:" + packet.SentSize);
#endif

                if (e.Offset + e.BytesTransferred < e.Count)
                {
                    //continue to send until all bytes are sent!
                    var completedAsync = true;
                    try
                    {
                        e.SetBuffer(e.Offset + e.BytesTransferred, e.Count - e.BytesTransferred - e.Offset);
                        completedAsync = this._socket.SendAsync(e);
                    }
                    catch (Exception ex)
                    {
                        sLog.Warn("SendAsyncCompleted(3) send packet exception:" + ex.ToString());

                        this.BeginDisconnect(ex);
                        this.FreeSend();
                        this.OnSendCallback(packet, false);
                        this.OnError(ex);
                    }

                    if (!completedAsync)
                        ThreadPool.QueueUserWorkItem(_ => this.SendAsyncCompleted(sender, e));
                }
                else
                {
                    if (packet.IsSent())
                    {
                        this._currSendingPacket = null;
                        this.OnSendCallback(packet, true);

                        //try send next packet
                        if (!this._packetQueue.TrySendNext()) this.FreeSend();
                    }
                    else this.SendPacketInternal(e);//continue send this packet
                }
            }
            #endregion

            #region Receive
            /// <summary>
            /// receive
            /// </summary>
            private void ReceiveInternal()
            {
                bool completed = true;
                try { completed = this._socket.ReceiveAsync(this._saeReceive); }
                catch (Exception ex)
                {
                    sLog.Warn("ReceiveInternal() exception:"+ex.ToString());

                    this.BeginDisconnect(ex);
                    this.FreeReceive();
                    this.OnError(ex);
                }

                if (!completed)
                    ThreadPool.QueueUserWorkItem(_ => this.ReceiveAsyncCompleted(this, this._saeReceive));
            }
            /// <summary>
            /// async receive callback
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void ReceiveAsyncCompleted(object sender, SocketAsyncEventArgs e)
            {
                if (e.SocketError != SocketError.Success)
                {
                    this.BeginDisconnect(new SocketException((int)e.SocketError));
                    this.FreeReceive();

                    sLog.Warn("ReceiveAsyncCompleted() SocketError:" + e.SocketError);

                    return;
                }

                if (e.BytesTransferred < 1)
                {
                    this.BeginDisconnect(new ObjectDisposedException("socket close"));
                    this.FreeReceive();
                    return;
                }

                ArraySegment<byte> buffer;
                var ts = this._tsStream;
                if (ts == null || ts.Length == 0)
                    buffer = new ArraySegment<byte>(e.Buffer, 0, e.BytesTransferred);
                else
                {
                    ts.Write(e.Buffer, 0, e.BytesTransferred);
                    buffer = new ArraySegment<byte>(ts.GetBuffer(), 0, (int)ts.Length);
                }

                this.OnMessageReceived(new MessageReceivedEventArgs(buffer, this.MessageProcessCallback));
            }
            /// <summary>
            /// message process callback
            /// </summary>
            /// <param name="payload"></param>
            /// <param name="readlength"></param>
            /// <exception cref="ArgumentOutOfRangeException">readlength less than 0 or greater than payload.Count.</exception>
            private void MessageProcessCallback(ArraySegment<byte> payload, int readlength)
            {
                if (readlength < 0 || readlength > payload.Count)
                {
                    sLog.Warn("MessageProcessCallback(), readlength :" + readlength + " less than 0 or greater than payload.Count:"+ payload.Count);
                    throw new ArgumentOutOfRangeException("readlength", "readlength less than 0 or greater than payload.Count.");
                }

                var ts = this._tsStream;
                if (readlength == 0)
                {
                    if (ts == null) this._tsStream = ts = new MemoryStream(this._messageBufferSize);
                    else ts.SetLength(0);

                    ts.Write(payload.Array, payload.Offset, payload.Count);
                    this.ReceiveInternal();
                    return;
                }

                if (readlength == payload.Count)
                {
                    if (ts != null) ts.SetLength(0);
                    this.ReceiveInternal();
                    return;
                }

                //粘包处理
                this.OnMessageReceived(new MessageReceivedEventArgs(
                    new ArraySegment<byte>(payload.Array, payload.Offset + readlength, payload.Count - readlength),
                    this.MessageProcessCallback));
            }
            #endregion

            #region Disconnect
            /// <summary>
            /// disconnect
            /// </summary>
            /// <param name="reason"></param>
            private void DisconnectInternal(Exception reason)
            {
                var e = new SocketAsyncEventArgs();
                e.Completed += this.DisconnectAsyncCompleted;
                e.UserToken = reason;

                var completedAsync = true;
                try
                {
                    this._socket.Shutdown(SocketShutdown.Both);
                    completedAsync = this._socket.DisconnectAsync(e);

                    //if (!((AbstractSessionBase)this._host).IsServerMode())
                    //{

                    //}
                }
                catch (Exception ex)
                {
                    sLog.Error("DisconnectInternal(), exception:", ex);
                    ThreadPool.QueueUserWorkItem(_ => this.DisconnectAsyncCompleted(this, e));
                    return;
                }

                if (!completedAsync)
                    ThreadPool.QueueUserWorkItem(_ => this.DisconnectAsyncCompleted(this, e));
            }
            /// <summary>
            /// async disconnect callback
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void DisconnectAsyncCompleted(object sender, SocketAsyncEventArgs e)
            {
                //dispose socket
                try { this._socket.Close(); }
                catch (Exception ex) { Logger.Error(ex.Message, ex); }

                //dispose socketAsyncEventArgs
                var reason = e.UserToken as Exception;
                e.Completed -= this.DisconnectAsyncCompleted;
                e.Dispose();

                //fire disconnected
                this.OnDisconnected(reason);
                //close send queue
                this.FreeSendQueue();
            }
            #endregion

            #endregion

            #region PacketQueue
            /// <summary>
            /// packet queue
            /// </summary>
            private class PacketQueue
            {
                #region Private Members
                public const int IDLE = 1;     //空闲状态
                public const int SENDING = 2;  //发送中
                public const int ENQUEUE = 3;  //入列状态
                public const int DEQUEUE = 4;  //出列状态
                public const int CLOSED = 5;   //已关闭

                private int _state = IDLE;      //当前状态
                private Queue<Packet> _queue = new Queue<Packet>();
                private Action<Packet> _sendAction = null;
                #endregion

                #region Constructors
                /// <summary>
                /// new
                /// </summary>
                /// <param name="sendAction"></param>
                /// <exception cref="ArgumentNullException">sendAction is null.</exception>
                public PacketQueue(Action<Packet> sendAction)
                {
                    if (sendAction == null) throw new ArgumentNullException("sendAction");
                    this._sendAction = sendAction;
                }
                #endregion

                #region Public Methods
                /// <summary>
                /// try send packet
                /// </summary>
                /// <param name="packet"></param>
                /// <returns>if CLOSED return false.</returns>
                public bool TrySend(Packet packet)
                {
                    var spin = true;
                    while (spin)
                    {
                        switch (this._state)
                        {
                            case IDLE:
                                if (Interlocked.CompareExchange(ref this._state, SENDING, IDLE) == IDLE)
                                    spin = false;
#if false //DEBUG
                                sLog.Info("Connection.TrySend(IDLE), send direct.");
#endif
                                break;
                            case SENDING:
                                if (Interlocked.CompareExchange(ref this._state, ENQUEUE, SENDING) == SENDING)
                                {
                                    this._queue.Enqueue(packet);
                                    this._state = SENDING;
#if false //DEBUG
                                    sLog.Info("Connection.TrySend(SENDING), push to send queue success.");
#endif
                                    return true;
                                }
                                break;
                            case ENQUEUE:
                            case DEQUEUE:
                                Thread.Yield();
                                break;
                            case CLOSED:
#if false //DEBUG
                                sLog.Info("Connection.TrySend(CLOSED), current is closed.");
#endif
                                return false;
                        }
                    }
                    this._sendAction(packet);
                    return true;
                }
                /// <summary>
                /// close
                /// </summary>
                /// <returns></returns>
                public CloseResult Close()
                {
                    var spin = true;
                    int beforeState = -1;
                    while (spin)
                    {
                        switch (this._state)
                        {
                            case IDLE:
                                if (Interlocked.CompareExchange(ref this._state, CLOSED, IDLE) == IDLE)
                                {
                                    spin = false;
                                    beforeState = IDLE;
                                }
                                break;
                            case SENDING:
                                if (Interlocked.CompareExchange(ref this._state, CLOSED, SENDING) == SENDING)
                                {
                                    spin = false;
                                    beforeState = SENDING;
                                }
                                break;
                            case ENQUEUE:
                            case DEQUEUE:
                                Thread.Yield();
                                break;
                            case CLOSED:
                                return new CloseResult(CLOSED, null);
                        }
                    }

                    var arrPackets = this._queue.ToArray();
                    this._queue.Clear();
                    this._queue = null;
                    this._sendAction = null;
                    return new CloseResult(beforeState, arrPackets);
                }
                /// <summary>
                /// try send next packet
                /// </summary>
                /// <returns>if CLOSED return false.</returns>
                public bool TrySendNext()
                {
                    var spin = true;
                    Packet packet = null;
                    while (spin)
                    {
                        switch (this._state)
                        {
                            case SENDING:
                                if (Interlocked.CompareExchange(ref this._state, DEQUEUE, SENDING) == SENDING)
                                {
                                    if (this._queue.Count == 0)
                                    {
                                        this._state = IDLE;
                                        return true;
                                    }

                                    packet = this._queue.Dequeue();
                                    this._state = SENDING;
                                    spin = false;
                                }
                                break;
                            case ENQUEUE:
                                Thread.Yield();
                                break;
                            case CLOSED:
                                return false;
                        }
                    }
                    this._sendAction(packet);
                    return true;
                }
                #endregion

                #region CloseResult
                /// <summary>
                /// close queue result
                /// </summary>
                public sealed class CloseResult
                {
                    /// <summary>
                    /// before close state
                    /// </summary>
                    public readonly int BeforeState;
                    /// <summary>
                    /// wait sending packet array
                    /// </summary>
                    public readonly Packet[] Packets;

                    /// <summary>
                    /// new
                    /// </summary>
                    /// <param name="beforeState"></param>
                    /// <param name="packets"></param>
                    public CloseResult(int beforeState, Packet[] packets)
                    {
                        this.BeforeState = beforeState;
                        this.Packets = packets;
                    }
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}