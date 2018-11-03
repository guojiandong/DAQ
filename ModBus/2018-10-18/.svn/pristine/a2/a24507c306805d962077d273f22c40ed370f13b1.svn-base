using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging
{
    /// <summary>
    /// 字符串
    /// </summary>
    public class StringMessage : AbstractMessageBase
    {
        /// <summary>
        /// payload
        /// </summary>
        public string Payload { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqID"></param>
        public StringMessage(string payload, int seqID = 0) : base(seqID)
        {
            if (String.IsNullOrEmpty(payload)) throw new ArgumentNullException("payload");
            this.Payload = payload;
        }

        public StringMessage(byte[] payload, int seqID = 0) : base(seqID)
        {
            if (payload == null) throw new ArgumentNullException("payload");
            this.Payload = Encoding.UTF8.GetString(payload, 0, payload.Length);
        }

        public StringMessage(StringMessage other) : base(other.SeqID)
        {
            this.CopyFrom(other);
        }

        public void CopyFrom(StringMessage other)
        {
            base.CopyFrom(other);
            this.Payload = other.Payload;
        }

        public override AbstractMessageBase Clone()
        {
            return new StringMessage(this);
        }

        public override string ToString()
        {
            return String.Format("StringMessage({0}), seq:{1}", this.Payload, this.SeqID);
        }

        public override Packet ToPacket()
        {
            Base.Packet pkg = new Base.Packet(Encoding.UTF8.GetBytes(this.Payload), SeqID);
            pkg.Tag = this.GetType().Name;
            return pkg;
        }
    }
}
