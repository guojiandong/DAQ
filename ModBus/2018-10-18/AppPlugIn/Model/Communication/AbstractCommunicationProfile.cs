
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Communication
{
    public enum CommunicationDeviceType : int
    {
        Unkown = 0,

        /// <summary>
        /// 扫码枪
        /// </summary>
        BarCodeScanner,

        /// <summary>
        /// 喷码器
        /// </summary>
        BarCodePrinter,

        /// <summary>
        /// 
        /// </summary>
        Plc,

        /// <summary>
        /// pdca上传终端
        /// </summary>
        MacMini,

        /// <summary>
        /// 工控机
        /// </summary>
        IPC,

        /// <summary>
        /// 嵌入式工控机
        /// </summary>
        EIPC,

        Count,
    }

    [Serializable]
    public abstract class AbstractCommunicationProfile// : Settings.AbstractProfileSettings
    {
        public AbstractCommunicationProfile()
        {
            this.Tag = "";
            this.Name = "";
            this.Remark = "";

            this.Device = CommunicationDeviceType.Unkown;
            this.Enable = true;

            this.Priority = ThreadPriority.Normal;
        }

        public AbstractCommunicationProfile(string tag) : this()
        {
            this.Tag = tag;
        }
        

        public void CopyFrom(AbstractCommunicationProfile other)
        {
            //base.CopyFrom(other);

            this.Tag = other.Tag;
            this.Name = other.Name;
            this.Remark = other.Remark;

            this.Device = other.Device;
            this.Enable = other.Enable;

            this.Priority = other.Priority;
        }

        public abstract AbstractCommunicationProfile Clone();
        //public abstract AbstractCommunicationProfile Clone()
        //{
        //    throw new InvalidCastException("invalid class for AbstractCommunicationProfile");
        //}

        public virtual string ToShortDescription()
        {
            StringBuilder str = new StringBuilder();
            str.Append(Tag).Append(",").Append(Name).Append(",").Append(Device.ToString()).Append(",");
            return str.ToString();
            //return "";
        }

        /// <summary>
        /// 编号
        /// </summary>
        [XmlElement("Tag")]
        public string Tag { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 备注、说明
        /// </summary>
        [XmlElement("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        [XmlElement("Device")]
        public CommunicationDeviceType Device { get; set; }

        /// <summary>
        /// 串口 延迟时间
        /// </summary>
        [XmlElement("Enable")]
        public bool Enable { get; set; }

        /// <summary>
        /// Specifies the scheduling priority of a System.Threading.Thread.
        /// </summary>
        [XmlElement("Priority")]
        public ThreadPriority Priority { get; set; }
    }
}
