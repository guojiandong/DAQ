
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{
    [Serializable]
    public class CommunicationSocketClientProfile : CommunicationSocketProfile
    {
        public CommunicationSocketClientProfile() : base()
        {
            this.Port = 0;
            this.IpAddress = "";
        }

        public CommunicationSocketClientProfile(string tag) : this(tag, CommunicationSocketProfile.DEFAULT_LISTEN_PORT)
        {
        }

        public CommunicationSocketClientProfile(string tag, int port) : this(tag, "127.0.0.1", port)
        {
        }

        public CommunicationSocketClientProfile(string tag, string ip, int port) : base(tag, port)
        {
            this.IpAddress = ip;
        }

        public CommunicationSocketClientProfile(CommunicationSocketClientProfile other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(CommunicationSocketClientProfile other)
        {
            base.CopyFrom(other);

            this.IpAddress = other.IpAddress;
        }

        public override AbstractCommunicationProfile Clone()
        {
            return new CommunicationSocketClientProfile(this);
        }

        public override string ToShortDescription()
        {
            StringBuilder str = new StringBuilder();
            str.Append("SockClient").Append(",").Append(IpAddress).Append(":").Append(Port);
            return str.ToString();
        }

        //protected override void onVersionUpgrade(int currentVersion)
        //{
        //    //throw new NotImplementedException();
        //}

        /// <summary>
        /// ip
        /// </summary>
        [XmlElement("IpAddress")]
        public string IpAddress { get; set; }
    }
}
