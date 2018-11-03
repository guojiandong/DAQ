using MemoryEditor;
using MemoryEditor.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryEditor
{
    [Serializable]
    public class Setting : AbstractEventNodeBase
    {
        public List<SubSetting> SubSettings { get; set; }
        public Setting() : this("")
        {
        }
        public Setting(string name) : this(name, 0,0)
        {
        }

        public Setting(string name, int begin, int end) : base(name, begin, end)
        {
            this.SubSettings = new List<SubSetting>();
        }

        public Setting(Setting other) : this(other.EventName, other.MemoryBegin, other.MemoryEnd)
        {
            this.CopyFrom(other);
        }

        public void CopyFrom(Setting other)
        {
            base.CopyFrom(other);
            this.SubSettings = new List<SubSetting>(other.SubSettings);
        }

        public override AbstractEventNodeBase Clone()
        {
            return new Setting(this);
        }

        public override AbstractEventNodeBase DeepCopy()
        {
            Setting info = (Setting)this.Clone();
            info.SubSettings.Clear();
            foreach (SubSetting item in this.SubSettings)
            {
                info.SubSettings.Add((SubSetting)item.DeepCopy());
            }
            return info;
        }

        public override bool HasChanged(AbstractEventNodeBase other)
        {
            if (other == null) return false;

            if (!other.GetType().Equals(this.GetType()))
                return true;

            EventSection o = (EventSection)other;
            return base.HasChanged(other);
        }
    }
   
}
