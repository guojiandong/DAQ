using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging
{
    public abstract class AbstractMessageBase : IMessage
    {
        /// <summary>
        /// get seqID
        /// </summary>
        public int SeqID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqID"></param>
        public AbstractMessageBase(int seqID = 0)
        {
            if (seqID == 0)
                this.SeqID = this.GetHashCode();
            else
                this.SeqID = seqID;
        }

        public AbstractMessageBase(AbstractMessageBase other)
        {
            this.SeqID = other.SeqID;
        }

        public virtual string ToLogString()
        {
            //StringBuilder str = new StringBuilder();

            //str.Append(this.GetType().Name).Append(",").Append(SeqID);

            return ToString();
        }

        public void CopyFrom(AbstractMessageBase other)
        {
            this.SeqID = other.SeqID;
        }

        public abstract AbstractMessageBase Clone();

        public virtual Base.Packet ToPacket()
        {
            //if (value == null) throw new ArgumentNullException("value");
            return null;// new Base.Packet(Encoding.UTF8.GetBytes(string.Concat(value, Environment.NewLine)));
        }
    }
}
