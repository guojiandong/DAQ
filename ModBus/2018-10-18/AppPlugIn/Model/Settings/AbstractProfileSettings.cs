using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugIn.Model.Settings
{
    //[XmlInclude(typeof(AbstractProfileSettings))]
    [Serializable]
    public abstract class AbstractProfileSettings
    {
        public delegate void OnProfileSettingsChanged(AbstractProfileSettings profile);

        public event OnProfileSettingsChanged OnSettingChangedEvent;


        public AbstractProfileSettings()
        {
            this.VersionCode = this.GetLastVersionCode();
        }

        public void CopyFrom(AbstractProfileSettings other)
        {
            this.VersionCode = other.VersionCode;

            //this.onCopyFrom(other);
        }

        public abstract AbstractProfileSettings Clone();

        //protected abstract void onCopyFrom<T>(T other) where T : AbstractProfileSettings;

        //protected abstract void onCopyFrom(AbstractProfileSettings other);


        [XmlAttribute("VersionCode")]
        public int VersionCode { get; set; }


        protected abstract void onVersionUpgrade(int currentVersion);

        public virtual void CheckVersionUpgrade()
        {
            if (this.VersionCode < this.GetLastVersionCode())
            {
                this.onVersionUpgrade(this.VersionCode);

                this.VersionCode = this.GetLastVersionCode();
            }
        }

        public virtual int GetLastVersionCode()
        {
            return 0;
        }

        public virtual void onLoaded()
        {

        }

        public virtual void onFirstLoaded()
        {

        }

        public virtual void onSaved(AbstractProfileSettings obj)
        {
            if (!obj.Equals(this))
                this.CopyFrom(obj);

            if (OnSettingChangedEvent != null)
                OnSettingChangedEvent(obj);
        }

    }
}
