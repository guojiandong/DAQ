using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{
    [Serializable]
    public class CommunicationSocketProfile : AbstractCommunicationProfile
    {
        public const int DEFAULT_LISTEN_PORT = 9865;

        public CommunicationSocketProfile() : this("")
        {
        }

        public CommunicationSocketProfile(string tag, int port = DEFAULT_LISTEN_PORT) : base(tag)
        {
            this.Port = port;

            this.ReadTimeout = -1;
            this.ReadBufferSize = 8192;

            this.WriteBufferSize = 8192;
            this.WriteTimeout = -1;
        }

        public CommunicationSocketProfile(CommunicationSocketProfile other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(CommunicationSocketProfile other)
        {
            base.CopyFrom(other);
            
            this.Port = other.Port;

            this.ReadTimeout = other.ReadTimeout;
            this.ReadBufferSize = other.ReadBufferSize;

            this.WriteBufferSize = other.WriteBufferSize;
            this.WriteTimeout = other.WriteTimeout;
        }

        public override AbstractCommunicationProfile Clone()
        {
            return new CommunicationSocketProfile(this);
        }

        //protected override void onVersionUpgrade(int currentVersion)
        //{
        //    //throw new NotImplementedException();
        //}

        public override string ToShortDescription()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Socket").Append(",").Append(Port);
            return str.ToString();
        }


        /// <summary>
        /// 监听端口
        /// </summary>
        [XmlElement("Port")]
        public int Port { get; set; }

        public int WriteBufferSize { get; set; }

        public int WriteTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ReadTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ReadBufferSize { get; set; }
    }
}
