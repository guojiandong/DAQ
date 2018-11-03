
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{
    [Serializable]
    public class CommunicationIpcServerProfile : AbstractCommunicationIpcProfile
    {
        public CommunicationIpcServerProfile() : this("")
        {
        }

        public CommunicationIpcServerProfile(string tag) : base(tag)
        {
        }

        public CommunicationIpcServerProfile(string tag, int port) : base(tag, port)
        {
            this.ApiTypeList = new List<string>();
        }

        public CommunicationIpcServerProfile(CommunicationIpcServerProfile other) : this(other.Tag, other.Port)
        {
            CopyFrom(other);
        }

        public void CopyFrom(CommunicationIpcServerProfile other)
        {
            base.CopyFrom(other);

            lock (this.ApiTypeList)
            {
                this.ApiTypeList.Clear();
                foreach (string item in other.ApiTypeList)
                {
                    this.ApiTypeList.Add(item);
                }
            }
        }

        public override AbstractCommunicationProfile Clone()
        {
            return new CommunicationIpcServerProfile(this);
        }

        public override string ToShortDescription()
        {
            StringBuilder str = new StringBuilder();
            str.Append("IpcServer").Append(",").Append(Port);
            return str.ToString();
        }

        /// <summary>
        /// 交互信息的 api class的fullname
        /// </summary>
        [XmlArray("ApiTypeList")]
        public List<string> ApiTypeList { get; set; }


        public void AddMessageType<T>()
        {
            this.AddMessageType(typeof(T));
        }

        public void AddMessageType(Type msg)
        {
            lock (this.ApiTypeList)
            {
                string name = msg.FullName;

                if (this.ApiTypeList.Contains(name))
                    return;

                this.ApiTypeList.Add(name);
            }
        }
    }
}
