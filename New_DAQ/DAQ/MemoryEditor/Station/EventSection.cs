using System;
using System.Collections.Generic;

namespace MemoryEditor
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class EventSection : AbstractEventNodeBase
    {
        public EventSection() : this("")
        {
        }

        public EventSection(string name) : this(name, 0,0)
        {
        }

        public EventSection(string name, int begin, int end) : base(name, begin, end)
        {
           this.Settings = new List<Setting>();
        }

        public EventSection(EventSection other) : this(other.EventName, other.MemoryBegin, other.MemoryEnd)
        {
            this.CopyFrom(other);
        }

        public void CopyFrom(EventSection other)
        {
            base.CopyFrom(other);
            this.Settings = new List<Setting>(other.Settings);
        }

        public override AbstractEventNodeBase Clone()
        {
            return new EventSection(this);
        }

        public override AbstractEventNodeBase DeepCopy()
        {
            EventSection info = (EventSection)this.Clone();

            info.Settings.Clear();
            foreach (Setting item in this.Settings)
            {
                info.Settings.Add((Setting)item.DeepCopy());
            }

            return info;
        }

        public List<Setting> Settings { get; set; }

        public override bool HasChanged(AbstractEventNodeBase other)
        {
           return base.HasChanged(other);
        }
    }
}
