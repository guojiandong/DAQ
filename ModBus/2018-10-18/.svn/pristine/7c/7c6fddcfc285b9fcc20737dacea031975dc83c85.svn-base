using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging
{
    /// <summary>
    /// thrift message.
    /// </summary>
    public class ThriftMessage : AbstractMessageBase
    {
        /// <summary>
        /// payload
        /// </summary>
        public byte[] Payload { get; private set; }

        /// <summary>
        /// new
        /// </summary>
        /// <param name="payload"></param>
        public ThriftMessage(int seqID, byte[] payload) : base(seqID)
        {
            if (payload == null) throw new ArgumentNullException("payload");
            this.Payload = payload;
        }

        public void CopyFrom(ThriftMessage other)
        {
            this.Payload = new byte[other.Payload.Length];
            Buffer.BlockCopy(other.Payload, 0, this.Payload, 0, this.Payload.Length);
        }

        public override AbstractMessageBase Clone()
        {
            ThriftMessage msg = new ThriftMessage(this.SeqID, this.Payload);
            return msg;
        }

        public override Packet ToPacket()
        {
            Base.Packet pkg = new Base.Packet(this.Payload, this.SeqID);
            pkg.Tag = this.GetType().Name;
            return pkg;

            //return base.ToPacket();
        }
    }
}
