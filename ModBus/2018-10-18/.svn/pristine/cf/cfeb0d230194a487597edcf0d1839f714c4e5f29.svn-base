
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Sock
{
    /// <summary>
    /// 服务器端
    /// get <see cref="SocketServer<BinaryMessage>"/> by connectionID
    /// </summary>
    public class SocketServerTask : AbstractSocketBinaryTask
    {
        public const int DEFAULT_LISTEN_PORT = 9087;
        private Communicate.FastIocp.Server.SocketServer<Communicate.FastIocp.Base.Messaging.BinaryMessage> mSocket;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maintask">proposal set to maintask</param>
        public SocketServerTask(int port = DEFAULT_LISTEN_PORT, object maintask = null) : base(maintask)
        {
            if(port <= 0)
            {
                throw new ArgumentOutOfRangeException("invalid port, port must > 0");
            }

            mSocket = new Communicate.FastIocp.Server.SocketServer<Communicate.FastIocp.Base.Messaging.BinaryMessage>(port, CreateProtocol(), this);
        }

        protected virtual Communicate.FastIocp.Base.Protocol.BinaryProtocol CreateProtocol()
        {
            return new Communicate.FastIocp.Base.Protocol.BinaryProtocol();
        }

        protected override Communicate.FastIocp.Base.AbstractSessionBase GetSession()
        {
            return mSocket;
        }

        public bool SetListenPort(int port)
        {
            if (port <= 0)
            {
                throw new ArgumentOutOfRangeException("invalid port, port must > 0");
            }

            if (mSocket.SetListenPort(port))
            {
                mSocket.Stop();

                mSocket.Start();

                return true;
            }

            return false;
        }
    }
}
