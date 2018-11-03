using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.PluginEngine
{
    [Serializable]
    public class AbstractPluginBaseInfo
    {
        public AbstractPluginBaseInfo()
        {
            this.Depands = new List<string>();
            this.AutoAttach = false;
            this.Enabled = true;
            this.TypeName = "";
        }

        public AbstractPluginBaseInfo(PluginInfo other) : this()
        {
            this.Depands = new List<string>(other.Depands);
            this.AutoAttach = other.AutoAttach;
            this.Enabled = other.Enabled;
            this.TypeName = other.TypeName;
        }

        public bool Equals(AbstractPluginBaseInfo other)
        {
            if (other == null) return false;

            if (this.TypeName == other.TypeName)
                return true;

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is AbstractPluginBaseInfo)
            {
                if (this.TypeName == ((AbstractPluginBaseInfo)obj).TypeName)
                    return true;
            }

            return base.Equals(obj);
        }
        
        /// <summary>
        /// 类别名, Type.FullName
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }


        /// <summary>
        /// 启动时自动加载
        /// </summary>
        public bool AutoAttach { get; set; }

        /// <summary>
        /// 依赖项
        /// <see cref="PluginInfo.Assembly"/> Plugin Assembly
        /// </summary>
        [System.Xml.Serialization.XmlArray]
        public List<string> Depands { get; set; }
    }
}
