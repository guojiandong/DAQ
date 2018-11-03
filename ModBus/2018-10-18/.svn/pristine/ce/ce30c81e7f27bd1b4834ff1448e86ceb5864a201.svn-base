using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Model.Communication;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Net
{
    public class UdpSocketSession : AbstractSocketSession
    {
        protected UdpSocketSession(Socket socket, IPEndPoint remoteEndPoint) : base(socket, remoteEndPoint)
        {
        }

        protected UdpSocketSession(Socket socket, string ip, int port) : base(socket, ip, port)
        {
        }

        public override void Initialize(AbstractCommunicationProfile profile)
        {
            //throw new NotImplementedException();
        }

        public override int Read(byte[] data, int offset, int length)
        {
            throw new NotImplementedException();
            //this.Client.ReceiveFrom
        }

        public override int TryConnect(int timeout = 0)
        {
            throw new NotImplementedException();
        }

        public override int Write(byte[] data, int offset, int length)
        {
            if (IsDisposed) return -1;

            IPEndPoint ipe = null;
            if (String.IsNullOrEmpty(RemoteIP))
                ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), RemotePort);
            else
                ipe = new IPEndPoint(IPAddress.Parse(RemoteIP), RemotePort);

            return this.Client.SendDataTo(data, offset, length, ipe);
        }
    }
}
