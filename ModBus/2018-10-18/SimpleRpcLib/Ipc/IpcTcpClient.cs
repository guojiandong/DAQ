using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
namespace Ksat.SimpleRpcLib.Ipc
{
    [Guid("0DA97633-7052-4447-B65B-565B249C67FA")]
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
