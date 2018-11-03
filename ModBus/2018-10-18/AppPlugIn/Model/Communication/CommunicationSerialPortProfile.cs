
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{
    [Serializable]
    public class CommunicationSerialPortProfile : AbstractCommunicationProfile
    {
        public CommunicationSerialPortProfile() : this("", "")
        {
        }

        public CommunicationSerialPortProfile(string tag, string portname, int baudrate = 115200) : base(tag)
        {
            this.PortName = portname;
            this.BaudRate = baudrate;
            this.DtrEnable = true;
            this.RtsEnable = true;

            this.StopBits = StopBits.One;
            this.Parity = Parity.None;
            this.DataBits = 8;


            this.DelayReadTime = 0;

            this.LoopReadInterval = 100;

            this.ReadTimeout = -1;
            this.RepeatReadCount = 0;
            this.ReadBufferSize = 4096;

            this.WriteBufferSize = 2048;
            this.WriteTimeout = -1;
        }

        public CommunicationSerialPortProfile(CommunicationSerialPortProfile other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(CommunicationSerialPortProfile other)
        {
            base.CopyFrom(other);
            

            this.PortName = other.PortName;
            this.BaudRate = other.BaudRate;
            this.DtrEnable = other.DtrEnable;
            this.RtsEnable = other.RtsEnable;


            this.StopBits = other.StopBits;
            this.Parity = other.Parity;
            this.DataBits = other.DataBits;


            this.RepeatReadCount = other.RepeatReadCount;

            this.DelayReadTime = other.DelayReadTime;

            this.LoopReadInterval = other.LoopReadInterval;
            this.ReadTimeout = other.ReadTimeout;
            this.ReadBufferSize = other.ReadBufferSize;

            this.WriteBufferSize = other.WriteBufferSize;
            this.WriteTimeout = other.WriteTimeout;
        }

        public override AbstractCommunicationProfile Clone()
        {
            return new CommunicationSerialPortProfile(this);
        }

        public override string ToShortDescription()
        {
            StringBuilder str = new StringBuilder();
            str.Append("SerialPort").Append(",").Append(PortName).Append(":").Append(BaudRate);
            return str.ToString();
        }

        //protected override void onVersionUpgrade(int currentVersion)
        //{
        //    //throw new NotImplementedException();
        //}

        /// <summary>
        /// 枪串名称
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// 串口波特率
        /// </summary>
        public int BaudRate { get; set; }


        public int DataBits { get; set; }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        /// 校验位
        /// </summary>
        public Parity Parity { get; set; }

        public bool DtrEnable { get; set; }


        public bool RtsEnable { get; set; }

        /// <summary>
        /// 串口 读取时间
        /// </summary>
        public int ReadTimeout { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int ReadBufferSize { get; set; }

        /// <summary>
        /// 串口 重复读取次数
        /// </summary>
        public int RepeatReadCount { get; set; }


        public int WriteBufferSize { get; set; }

        public int WriteTimeout { get; set; }

        /// <summary>
        /// 串口 延迟时间
        /// </summary>
        [XmlElement("DelayReadTime")]
        public int DelayReadTime { get; set; }


        /// <summary>
        /// 串口 循环读取间隔时间
        /// </summary>
        [XmlElement("LoopReadInterval")]
        public int LoopReadInterval { get; set; }
    }
}
