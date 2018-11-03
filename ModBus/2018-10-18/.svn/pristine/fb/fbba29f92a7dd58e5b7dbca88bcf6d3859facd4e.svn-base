using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging
{
    /// <summary>
    /// 原始的字节流数据
    /// </summary>
    public class BinaryMessage : AbstractMessageBase
    {
        /// <summary>
        /// payload
        /// </summary>
        public byte[] Payload { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqID"></param>
        /// <param name="payload"></param>
        public BinaryMessage(byte[] payload, int seqID = 0) : base(seqID)
        {
            if (payload == null) throw new ArgumentNullException("payload");
            this.Payload = payload;
        }

        public BinaryMessage(byte[] payload, int offset, int count, int seqID = 0) : base(seqID)
        {
            if (payload == null || count <= 0) throw new ArgumentNullException("payload");

            this.Payload = new byte[count];

            Buffer.BlockCopy(payload, offset, this.Payload, 0, count);
        }

        public BinaryMessage(string payload, int seqID = 0) : base(seqID)
        {
            if (String.IsNullOrEmpty(payload)) throw new ArgumentNullException("payload");
            this.Payload = Encoding.UTF8.GetBytes(payload);
        }

        public BinaryMessage(BinaryMessage other) : base(other.SeqID)
        {
            this.CopyFrom(other);
        }

        public void CopyFrom(BinaryMessage other)
        {
            base.CopyFrom(other);
            this.Payload = new byte[other.Payload.Length];
            Buffer.BlockCopy(other.Payload, 0, this.Payload, 0, this.Payload.Length);
        }

        public override AbstractMessageBase Clone()
        {
            return new BinaryMessage(this);
        }
        //public BinaryMessage(int seqID, byte[] payload, int offset, int length) : base(seqID)
        //{
        //    if (payload == null) throw new ArgumentNullException("payload");
        //    this.Payload = payload;
        //}

        public string GetPayloadString()
        {
            if (this.Payload != null)
            {
                return Encoding.UTF8.GetString(this.Payload);
            }

            return "";
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("BinaryMessage").Append(",");

            if (this.Payload != null)
            {
                str.AppendFormat("{0:X4}", this.Payload.Length).Append(",");

                for (int i = 0; i < Math.Min(16, this.Payload.Length); i++)
                {
                    str.AppendFormat("{0:X2} ", this.Payload[i]);

                    if (i % 4 == 0)
                        str.Append(" ");
                }
            }

            return str.ToString();
        }

        public override Packet ToPacket()
        {
            Base.Packet pkg = new Base.Packet(this.Payload, this.SeqID);
            pkg.Tag = this.GetType().Name;
            return pkg;
            //return new Base.Packet(this.Payload, SeqID);
        }
    }
}
