
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{
    [Serializable]
    public class CommunicationSocketTcpClientProfile : CommunicationSocketClientProfile
    {
        public CommunicationSocketTcpClientProfile() : base()
        {
        }

        public CommunicationSocketTcpClientProfile(string tag) : this(tag, CommunicationSocketProfile.DEFAULT_LISTEN_PORT)
        {
        }

        public CommunicationSocketTcpClientProfile(string tag, int port) : this(tag, "127.0.0.1", port)
        {
        }

        public CommunicationSocketTcpClientProfile(string tag, string ip, int port) : base(tag, ip, port)
        {
            this.AutoReconnectInterval = 1000;
            this.KeepAliveInterval = 3000;
        }

        public CommunicationSocketTcpClientProfile(CommunicationSocketTcpClientProfile other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(CommunicationSocketTcpClientProfile other)
        {
            base.CopyFrom(other);

            this.AutoReconnectInterval = other.AutoReconnectInterval;
            this.KeepAliveInterval = other.KeepAliveInterval;
        }

        public override AbstractCommunicationProfile Clone()
        {
            return new CommunicationSocketTcpClientProfile(this);
        }

        public override string ToShortDescription()
        {
            StringBuilder str = new StringBuilder();
            str.Append("TcpClient").Append(",").Append(IpAddress).Append(":").Append(Port);
            return str.ToString();
        }

        /// <summary>
        /// 自动重连间隔时间, 
        /// -1 : 禁止自动重连
        /// </summary>
        public int AutoReconnectInterval { get; set; }

        /// <summary>
        /// TCP心跳间隔, 探测时间间隔
        /// </summary>
        public uint KeepAliveInterval { get; set; }
    }
}
