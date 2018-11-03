
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Sock
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketClientTask : AbstractSocketBinaryTask
    {
        public delegate void OnConnectedCallback(Communicate.FastIocp.Base.IConnection connection);


        private Communicate.FastIocp.Client.SocketClient<Communicate.FastIocp.Base.Messaging.BinaryMessage> mSocket;

        /// <summary>
        /// proposal set to maintask
        /// </summary>
        /// <param name="maintask"></param>
        public SocketClientTask(object maintask = null) : base(maintask)
        {
            mSocket = new Communicate.FastIocp.Client.SocketClient<Communicate.FastIocp.Base.Messaging.BinaryMessage>(CreateProtocol());
            mSocket.RegisterListener(this);
        }

        protected virtual Communicate.FastIocp.Base.Protocol.BinaryProtocol CreateProtocol()
        {
            return new Communicate.FastIocp.Base.Protocol.BinaryProtocol();
        }

        protected override Communicate.FastIocp.Base.AbstractSessionBase GetSession()
        {
            return mSocket;
        }
        
        /// <summary>
        /// try register endPoint
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">socketClient</exception>
        public bool TryConnect(string tag, string ip = "localhost", int port = SocketServerTask.DEFAULT_LISTEN_PORT, OnConnectedCallback callback = null)
        {
            if (String.IsNullOrEmpty(ip))
            {
                ip = "localhost";
            }

            if (String.IsNullOrEmpty(tag))
            {
                tag = String.Format("{0}:{1}", ip, port);
            }

            mSocket.UnRegisterEndPoint(tag);
            if (callback != null)
            {
                return mSocket.TryRegisterEndPoint(tag, new EndPoint[] { new IPEndPoint(IPAddress.Parse(ip), port) },
                    connection =>
                    {
                        var source = new TaskCompletionSource<bool>();
                        if (callback != null)
                            callback(connection);
                        return source.Task;
                    });
            }

            return mSocket.TryRegisterEndPoint(tag, new EndPoint[] { new IPEndPoint(IPAddress.Parse(ip), port) }, null);
        }
        
        /// <summary>
        /// un register endPoint
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">socketClient</exception>
        public bool Disconnect(string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("invalid tag, it can't be empty.");
            }

            return this.mSocket.UnRegisterEndPoint(tag);
        }

        /// <summary>
        /// get all registered endPoint
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, EndPoint[]>[] GetAllRegisteredEndPoint()
        {
            return mSocket.GetAllRegisteredEndPoint();
        }

        public bool CheckExisting(string tag)
        {
            return this.mSocket.CheckExisting(tag);
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="packet"></param>
        public bool BeginSend(byte[] buffer, int offset, int count)
        {
            Communicate.FastIocp.Base.Messaging.BinaryMessage msg = new Communicate.FastIocp.Base.Messaging.BinaryMessage(buffer, offset, count);
            return mSocket.Send(msg.ToPacket());
        }
    }
}
