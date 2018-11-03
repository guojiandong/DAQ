using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol.Filter
{
    /// <summary>
    /// 最小长度
    /// </summary>
    public class FixedMinLengthBinaryProtocol : BinaryProtocol
    {
        /// <summary>
        /// 最小长度
        /// </summary>
        public int Length { set; private get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length">固定长度</param>
        public FixedMinLengthBinaryProtocol(int length)
        {
            if (length < 1) throw new ArgumentOutOfRangeException("length must larger than 0");

            Length = length;
        }

        public override Messaging.BinaryMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            var messageLength = buffer.Count;// - buffer.Offset;// Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset);
            if (messageLength < this.Length)
            {
                readlength = 0;
                return null;
            }

            readlength = messageLength;

            var payload = new byte[readlength];
            Buffer.BlockCopy(buffer.Array, buffer.Offset, payload, 0, readlength);

            return new Messaging.BinaryMessage(payload);
        }
    }
}
