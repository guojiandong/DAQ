using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace Ksat.SimpleRpcLib.Ipc
{
    [Guid("5BF93469-96A9-473E-97A7-FE5F873F248F")]
    [ComVisible(true)]
    public class IpcTcpServer : IpcServer
    {

        public IpcTcpServer() : this(CONST_DEFAULT_PORT)
        {
        }

        public IpcTcpServer(int port) : base(port)
        {
        }

        public override IpcMode GetIpcMode()
        {
            return IpcMode.Tcp;
        }

        protected override IChannel createIpcChannel()
        {
            return new TcpServerChannel(this.GetChannelName(), this.Port);
        }
    }
}
