// Gary.Zhang
// Gary@jzspace.com
// 2018/7/28
using System;
namespace MemoryEditor
{
    [Serializable]
    public abstract class AbstractEventNodeBase : IEventNode
    {
        public AbstractEventNodeBase() : this("")
        {
        }

        public AbstractEventNodeBase(string name) : this(name, 0,0)
        {
        }

        public AbstractEventNodeBase(string name, int begin,int end)
        {
            this.EventName = name;
            this.MemoryBegin = begin;
            this.MemoryEnd = end;
            this.DataSize = 0;
            this.Description = "";
        }

        public AbstractEventNodeBase(AbstractEventNodeBase other) : this()
        {
            CopyFrom(other);
        }

        public void CopyFrom(AbstractEventNodeBase other)
        {
            this.EventName = other.EventName;
            this.MemoryBegin = other.MemoryBegin;
            this.MemoryEnd = other.MemoryEnd;
            this.DataSize = other.DataSize;
            this.Description = other.Description;
        }

        public abstract AbstractEventNodeBase DeepCopy();

        public abstract AbstractEventNodeBase Clone();

        /// <summary>
        /// 节点名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 内存开始位置
        /// </summary>
        public int MemoryBegin { get; set; }

        /// <summary>
        /// 内存结束位置
        /// </summary>
        public int MemoryEnd { get; set; }

        /// <summary>
        /// 数据大小  word
        /// </summary>
        public int DataSize { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }


        public virtual bool HasChanged(AbstractEventNodeBase other)
        {
            if (other == null) return false;

            if(this.EventName != other.EventName
               || this.MemoryBegin != other.MemoryBegin
               || this.DataSize != other.DataSize
               || this.Description != other.Description
               || this.MemoryEnd != other.MemoryEnd)
            {
                return true;
            }

            return false;
        }


        public virtual string ToPropertyString()
        {
            return String.Empty;
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}, {2}", this.GetType().Name, this.EventName, this.DataSize);
        }
    }
}
