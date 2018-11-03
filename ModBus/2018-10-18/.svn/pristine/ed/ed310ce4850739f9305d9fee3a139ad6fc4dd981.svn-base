using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{
    [Serializable]
    public class CommunicationIpcClientProfile : AbstractCommunicationIpcProfile
    {
        public CommunicationIpcClientProfile() : base()
        {
            this.IpAddress = "";
        }

        public CommunicationIpcClientProfile(string tag) : this(tag, CommunicationSocketProfile.DEFAULT_LISTEN_PORT)
        {
        }

        public CommunicationIpcClientProfile(string tag, int port) : this(tag, "127.0.0.1", port)
        {
        }

        public CommunicationIpcClientProfile(string tag, string ip, int port) : base(tag, port)
        {
            this.IpAddress = ip;
        }

        public CommunicationIpcClientProfile(CommunicationIpcClientProfile other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(CommunicationIpcClientProfile other)
        {
            base.CopyFrom(other);

            this.IpAddress = other.IpAddress;
        }

        public override AbstractCommunicationProfile Clone()
        {
            return new CommunicationIpcClientProfile(this);
        }

        public override string ToShortDescription()
        {
            StringBuilder str = new StringBuilder();
            str.Append("IpcClient").Append(",").Append(IpAddress).Append(":").Append(Port);
            return str.ToString();
        }

        /// <summary>
        /// ip地址
        /// </summary>
        [XmlElement("IpAddress")]
        public string IpAddress { get; set; }
    }
}
