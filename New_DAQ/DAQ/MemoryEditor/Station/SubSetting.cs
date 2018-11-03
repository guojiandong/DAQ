using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryEditor.Station
{
    [Serializable]
    public class SubSetting : AbstractEventNodeBase
    {
        public SubSetting() : this("")
        {
        }
        public SubSetting(string name) : this(name, 0,0)
        {
        }

        public SubSetting(string name, int begin, int end) : base(name, begin, end)
        {
        }

        public SubSetting(SubSetting other) : this(other.EventName, other.MemoryBegin, other.MemoryEnd)
        {
            this.CopyFrom(other);
        }

        public void CopyFrom(Setting other)
        {
            base.CopyFrom(other);
        }

        public override AbstractEventNodeBase Clone()
        {
            return new SubSetting(this);
        }

        public override AbstractEventNodeBase DeepCopy()
        {
            SubSetting info = (SubSetting)this.Clone();
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
