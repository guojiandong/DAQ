using System;
namespace Ksat.AppPlugIn.PluginEngine
{
    /// <summary>
    /// 服务提供标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    [System.Runtime.InteropServices.Guid("F9851EE2-7119-43CD-B214-C69BC0E4B1A2")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public class PluginAttribute : System.Attribute
    {
        /// <summary>
        /// 创建 PluginAttribute class 的新实例
        /// </summary>
        public PluginAttribute(string author, string contact, string name, string copyRight, string description)
        {
            Description = new PluginDescription(author, contact, name, copyRight, description);
        }

        /// <summary>
        /// 创建 PluginAttribute class 的新实例
        /// </summary>
        public PluginAttribute(string author, string contact, string name, string copyRight, string description, bool defaultEnabled)
        {
            Description = new PluginDescription(author, contact, name, copyRight, description, defaultEnabled);
        }

        /// <summary>
        /// 描述
        /// </summary>
        public PluginDescription Description { get; set; }


    }
}
