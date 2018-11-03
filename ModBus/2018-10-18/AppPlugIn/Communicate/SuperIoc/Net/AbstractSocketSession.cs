using Ksat.AppPlugIn.Communicate.SuperIoc.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Net
{
    public abstract class AbstractSocketSession : AbstraceSessionBase, ISocketSession
    {
        
        protected AbstractSocketSession(Socket socket, string ip, int port) : this(socket, null)
        {
            this.RemoteIP = ip;
            this.RemotePort = port;
        }

        protected AbstractSocketSession(Socket socket, IPEndPoint remoteEndPoint)
        {
            SessionID = Guid.NewGuid().ToString();

            this.RemotePort = 0;

            if (remoteEndPoint != null)
            {
                string[] temp = remoteEndPoint.ToString().Split(':');
                if (temp.Length >= 2)
                {
                    this.RemoteIP = temp[0];
                    this.RemotePort = Convert.ToInt32(temp[1]);
                }
            }

            //this.RemoteEndPoint = remoteEndPoint;
            this.Client = socket;
            //SocketAsyncProxy = proxy;

            this.StartTime = DateTime.Now;
            this.LastActiveTime = StartTime;
            this.AutoReconnectInterval = -1;
        }

        ~AbstractSocketSession()
        {
            doDispose(false);
        }

        ///// <summary>
        ///// 同步对象
        ///// </summary>
        //private object _SyncLock = new object();

        /// <summary>
        /// 远程IP
        /// </summary>
        public string RemoteIP { get; protected set; }

        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotePort { get; protected set; }

        /// <summary>
        /// Socket实例
        /// </summary>
        public Socket Client { get; protected set; }

        /// <summary>
        /// 远程点
        /// </summary>
        //public IPEndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// 开始时间 
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// 最后接收有效数据的时间。
        /// </summary>
        public DateTime LastActiveTime { get; set; }

        /// <summary>
        /// 自动重连间隔时间
        /// </summary>
        public int AutoReconnectInterval { get; set; }

        public override object Session
        {
            get
            {
                return this.Client;
            }
        }

        /// <summary>
        /// 是否联机
        /// </summary>
        public bool IsConnected
        {
            get {
                if (this.Client == null)
                    return false;

                return this.Client.Connected;
            }
        }

        public abstract int TryConnect(int timeout = 0);


        /// <summary>
        /// 关键字
        /// </summary>
        public override string Tag
        {
            get { return String.Format("{0}:{1}", this.RemoteIP, this.RemotePort); }
        }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public override string SessionID { get; protected set; }

        /// <summary>
        /// 通道实例
        /// </summary>
        //public override IChannel Channel
        //{
        //    get { return (IChannel)this; }
        //}

        public override void Close()
        {
            base.Dispose();
        }

        public override CommunicateType CommunicationType
        {
            get { return CommunicateType.NET; }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void onDisposed()
        {
            doDispose(true);
        }

        protected virtual void doDispose(bool disposing)
        {
            if (disposing && Client != null)
            {
                Client.SafeClose();
                Client.Dispose();
                Client = null;
            }
        }

        ///// <summary>
        ///// 代理实例
        ///// </summary>
        //public ISocketAsyncEventArgsProxy SocketAsyncProxy { get; private set; }

        //public event CloseSocketHandler CloseSocket;

        //protected virtual void OnCloseSocket()
        //{
        //    if (CloseSocket != null)
        //    {
        //        CloseSocket(this, this);
        //    }
        //}

        //public event SocketReceiveDataHandler SocketReceiveData;

        //protected virtual void OnSocketReceiveData(IReceivePackage dataPackage)
        //{
        //    if (SocketReceiveData != null)
        //    {
        //        SocketReceiveData(this, this, dataPackage);
        //    }
        //}
#if false
        public abstract void TryReceive();

        public void TrySend(byte[] data, bool type)
        {
            if (type)
            {
                SendAsync(data);
            }
            else
            {
                SendSync(data);
            }
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="data"></param>
        protected abstract void SendAsync(byte[] data);
        /// <summary>
        /// 同步发送
        /// </summary>
        /// <param name="data"></param>
        protected abstract void SendSync(byte[] data);
        /// <summary>
        /// 初始化
        /// </summary>
        //public abstract void Initialize();

        /// <summary>
        /// 事件完成接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void SocketEventArgs_Completed(object sender, SocketAsyncEventArgs e);

        //public object SyncLock
        //{
        //    get { return _SyncLock; }
        //}
#endif

    }
}
