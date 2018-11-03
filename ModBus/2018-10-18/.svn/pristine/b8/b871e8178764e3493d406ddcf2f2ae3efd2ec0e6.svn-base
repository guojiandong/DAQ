using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.PluginEngine
{
    /// <summary>
    /// 插件依赖插件
    /// <see cref="PluginInfo.Assembly"/> Plugin Assembly
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.Guid("67BE3F7C-1636-43F9-AD58-480F21E7BF38")]
    [System.Runtime.InteropServices.ComVisible(true)]
    [System.AttributeUsage(System.AttributeTargets.Class,AllowMultiple = true)]
    public class PluginDepandAttribute : System.Attribute
    {
        //public PluginDepandAttribute(params string[] depands)
        public PluginDepandAttribute(string depands)
        {
            this.Depand = depands;
        }

        /// <summary>
        /// 依赖
        /// </summary>
        public string Depand { get; set; }
    }
}
