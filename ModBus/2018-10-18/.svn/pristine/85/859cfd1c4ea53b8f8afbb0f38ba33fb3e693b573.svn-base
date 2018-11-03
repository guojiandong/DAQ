using Ksat.AppPlugIn.Communicate.SuperIoc.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Net
{
    public abstract class AbstractSocketListenerBase : AbstraceSessionBase
    {
        public AbstractSocketListenerBase(Socket sock, int port = 0) : base()
        {
            this.Sock = sock;
        }

        /// <summary>
        /// Socket实例
        /// </summary>
        public Socket Sock { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public int ListenPort { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public int MaxListenCount { get; protected set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public override string Tag
        {
            get { return String.Format("{0}", this.ListenPort); }
        }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public override string SessionID { get; protected set; }

        /// <summary>
        /// 通道实例
        /// </summary>
        public override object Session
        {
            get
            {
                return this.Sock;
            }
        }

        public override void Close()
        {
            if(this.Sock != null)
            {
                this.Sock.SafeClose();
                this.Sock.Dispose();
                this.Sock = null;
            }

            base.Dispose();
        }

        public abstract void StartListen();
        public abstract ISocketSession StartAccept();


        public override CommunicateType CommunicationType
        {
            get { return CommunicateType.NET; }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void onDisposed()
        {
            if (this.Sock != null)
            {
                this.Sock.SafeClose();
                this.Sock.Dispose();
                this.Sock = null;
            }
        }
        
    }
}
