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
    [Guid("9988F927-D69B-4B7C-9505-861248B73657")]
    [ComVisible(true)]
    public class IpcTcpClient : IpcClient
    {
        public string RemoteUri { get; set; }

        public IpcTcpClient() : this("localhost")
        {
        }

        public IpcTcpClient(string remote_uri) : this(remote_uri, IpcServer.CONST_DEFAULT_PORT)
        {
        }

        public IpcTcpClient(string remote_uri, int port) : base(port)
        {
            this.RemoteUri = remote_uri;
        }

        public override IpcMode GetIpcMode()
        {
            return IpcMode.Tcp;
        }

        protected override string GetIpcUri()
        {
            if (String.IsNullOrEmpty(this.RemoteUri))
                return "127.0.0.1";
            return this.RemoteUri;
        }

        protected override IChannel createIpcChannel()
        {
            return new TcpClientChannel(this.GetChannelName(), null);
        }
    }
}
