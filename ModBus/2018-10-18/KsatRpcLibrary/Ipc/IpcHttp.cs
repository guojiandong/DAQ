using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Text;

namespace KsatRpcLibrary.Ipc
{
    [Guid("B0D43A16-5417-4E41-9EC2-CD75F74270B1")]
    [ComVisible(true)]
    public class IpcHttpClient : IpcClient
    {
        public string RemoteUri { get; set; }

        public IpcHttpClient() : this("localhost")
        {
        }

        public IpcHttpClient(string remote_uri) : this(remote_uri, IpcServer.CONST_DEFAULT_PORT)
        {
        }

        public IpcHttpClient(string remote_uri, int port) : base(port)
        {
            this.RemoteUri = remote_uri;
        }

        public override IpcMode GetIpcMode()
        {
            return IpcMode.Http;
        }

        protected override string GetIpcUri()
        {
            if (String.IsNullOrEmpty(this.RemoteUri))
                return "127.0.0.1";
            return this.RemoteUri;
        }

        protected override IChannel createIpcChannel()
        {
            return new HttpClientChannel(this.GetChannelName(), null);
        }
    }

    [Guid("E14F06E1-EE30-4A11-8686-48C0D7516609")]
    [ComVisible(true)]
    public class IpcHttpServer : IpcServer
    {

        public IpcHttpServer() : this(CONST_DEFAULT_PORT)
        {
        }

        public IpcHttpServer(int port) : base(port)
        {
        }

        public override IpcMode GetIpcMode()
        {
            return IpcMode.Http;
        }

        protected override IChannel createIpcChannel()
        {
            return new HttpServerChannel(this.GetChannelName(), this.Port);
        }
    }
}
