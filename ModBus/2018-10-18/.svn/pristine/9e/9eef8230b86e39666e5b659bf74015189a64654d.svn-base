using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Settings
{
    [XmlInclude(typeof(PluginEngine.AbstractPluginBaseInfo))]
    [XmlInclude(typeof(PluginEngine.PluginInfo))]
    [Serializable]
    public abstract class MainProfileSettings : AbstractProfileSettings
    {
        public MainProfileSettings()
        {
            this.PlugInSettings = new List<PluginEngine.AbstractPluginBaseInfo>();
        }

        public MainProfileSettings(MainProfileSettings other) : this()
        {
            CopyFrom(other);
        }

        public void CopyFrom(MainProfileSettings other)
        {
            base.CopyFrom(other);

            this.PlugInSettings = new List<PluginEngine.AbstractPluginBaseInfo>(other.PlugInSettings);
        }

        public override void CheckVersionUpgrade()
        {
            base.CheckVersionUpgrade();
            
        }

        #region 模块配置

#if true
        /// <summary>
        /// 模块配置
        /// </summary>
        [System.Xml.Serialization.XmlArray]
        public List<PluginEngine.AbstractPluginBaseInfo> PlugInSettings { get; set; }

        public PluginEngine.AbstractPluginBaseInfo GetPlugin(PluginEngine.AbstractPluginBaseInfo other)
        {
            lock (this.PlugInSettings)
            {
                foreach (PluginEngine.AbstractPluginBaseInfo item in this.PlugInSettings)
                {
                    if (item.Equals(other))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public bool IsPluginEnable(PluginEngine.AbstractPluginBaseInfo other)
        {
            lock (this.PlugInSettings)
            {
                foreach (PluginEngine.AbstractPluginBaseInfo item in this.PlugInSettings)
                {
                    if(item.Equals(other))
                    {
                        return item.Enabled;
                    }
                }
            }

            return true;
        }

        public bool IsPluginSupportAutoAttach(PluginEngine.AbstractPluginBaseInfo other)
        {
            lock (this.PlugInSettings)
            {
                foreach (PluginEngine.AbstractPluginBaseInfo item in this.PlugInSettings)
                {
                    if (item.Equals(other))
                    {
                        return item.AutoAttach;
                    }
                }
            }

            return false;
        }

#else
        /// <summary>
        /// 模块配置
        /// </summary>
        [XmlElement("PlugInSettings")]
        public SerializableDictionary<string, AbstractProfileSettings> PlugInSettings { get; set; }

        public void AddPlugInSettings(string tag, AbstractProfileSettings settings, bool replace = false)
        {
            if(String.IsNullOrEmpty(tag) || settings == null)
            {
                throw new ArgumentNullException("invalid parameters.");
            }

            lock (this.PlugInSettings)
            {
                if (this.PlugInSettings.ContainsKey(tag))
                {
                    if (!replace)
                    {
                        return;
                    }

                    this.PlugInSettings.Remove(tag);
                }

                this.PlugInSettings.Add(tag, settings);
            }
        }

        public AbstractProfileSettings RemovePlugInSettings(string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("invalid parameters.");
            }

            AbstractProfileSettings result = null;
            lock (this.PlugInSettings)
            {
                if (this.PlugInSettings.TryGetValue(tag, out result))
                    this.PlugInSettings.Remove(tag);
            }

            return result;
        }

        public AbstractProfileSettings GetPlugInSettings(string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("invalid parameters.");
            }

            AbstractProfileSettings result = null;
            lock (this.PlugInSettings)
            {
                if (this.PlugInSettings.TryGetValue(tag, out result))
                    return result;
            }

            return result;
        }
#endif
        #endregion

        [Serializable]
        public class PluginSettingsPair
        {
            public string Key { get; set; }
            public string Value { get; set; }

            public PluginSettingsPair()
            {

            }

            public PluginSettingsPair(string key, string value)
            {
                this.Key = key;
                this.Value = value;
            }

            public PluginSettingsPair(PluginSettingsPair other)
            {
                this.Key = other.Key;
                this.Value = other.Value;
            }
        }
    }


}
