using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Ipc
{
    [Guid("9CB71B9D-9B8E-4FF2-BCCE-F66D877FD94C")]
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
