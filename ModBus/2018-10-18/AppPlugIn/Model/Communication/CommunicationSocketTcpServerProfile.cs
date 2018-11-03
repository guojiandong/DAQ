
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{
    [Serializable]
    public class CommunicationSocketTcpServerProfile : CommunicationSocketProfile
    {
        
        public CommunicationSocketTcpServerProfile() : this("")
        {
        }

        public CommunicationSocketTcpServerProfile(string tag) : base(tag)
        {
            this.KeepAliveInterval = 3000;
            this.MaxConnCount = 100;
            this.CheckOverTime = 0;
        }

        public CommunicationSocketTcpServerProfile(string tag, int port) : base(tag, port)
        {
            this.KeepAliveInterval = 3000;
            this.MaxConnCount = 100;
            this.CheckOverTime = 0;
        }

        public CommunicationSocketTcpServerProfile(CommunicationSocketTcpServerProfile other)
        {
            CopyFrom(other);

            this.KeepAliveInterval = other.KeepAliveInterval;
            this.MaxConnCount = other.MaxConnCount;

            this.CheckOverTime = other.CheckOverTime;
        }

        public override AbstractCommunicationProfile Clone()
        {
            //throw new NotImplementedException();

            return new CommunicationSocketTcpServerProfile(this);
        }

        public override string ToShortDescription()
        {
            StringBuilder str = new StringBuilder();
            str.Append("TcpServer").Append(",").Append(Port);
            return str.ToString();
        }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnCount { get; set; }


        /// <summary>
        /// TCP心跳间隔, 探测时间间隔
        /// </summary>
        public uint KeepAliveInterval { get; set; }

        /// <summary>
        /// 超时时长,单位秒.(每10秒检查一次)，当值为0时，不设置超时
        /// </summary>
        public int CheckOverTime { get; set; }
    }
}
