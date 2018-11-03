using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Communicate.SuperIoc.Base;
using Ksat.AppPlugIn.Model.Communication;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Net
{
    public class TcpSocketListener : AbstractSocketListenerBase, ISessionStatusListener
    {
        public TcpSocketListener(int port = 0) 
            : base(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), port)
        {
        }

        public override void Initialize(AbstractCommunicationProfile profile)
        {
            if(profile is CommunicationSocketTcpServerProfile)
            {
                CommunicationSocketTcpServerProfile p = (CommunicationSocketTcpServerProfile)profile;

                if (ListenPort == 0)
                    this.ListenPort = p.Port;

                this.MaxListenCount = p.MaxConnCount;
            }
        }

        public override void StartListen()
        {
            this.Sock.Bind(new IPEndPoint(IPAddress.Any, ListenPort));
            this.Sock.Listen(Math.Max(1, this.MaxListenCount));    //设定最多10个排队连接请求  

            while (!IsDisposed)
            {
                Socket client = this.Sock.Accept();

                TcpSocketSession session = CreateTcpSocketSession(client);

                NotifySessionStatusChanged(Base.SessionStatus.Connected, session);
            }
        }

        public override ISocketSession StartAccept()
        {
            Socket client = this.Sock.Accept();

            TcpSocketSession session = CreateTcpSocketSession(client);

            session.RegisterSessionStatusListener(this);

            NotifySessionStatusChanged(Base.SessionStatus.Connected, session);

            return session;
        }

        protected virtual TcpSocketSession CreateTcpSocketSession(Socket client)
        {
            return new TcpSocketSession(client, (IPEndPoint)client.RemoteEndPoint);
        }
        

        public override int Read(byte[] data, int offset, int length)
        {
            throw new NotImplementedException();
        }

        public override int Write(byte[] data, int offset, int length)
        {
            throw new NotImplementedException();
        }

        public void OnSessionStatusChanged(object sender, SessionStatus status, object value)
        {
            NotifySessionStatusChanged(status, sender);
        }
    }
}
