using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Utils;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol.Filter
{
    /// <summary>
    /// 有头标识符和固定长度的模式
    /// </summary>
    public class FixedHeadAndLengthBinaryProtocol : BinaryProtocol
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headBytes"></param>
        /// <param name="length">包括SpliterBytes的长度</param>
        public FixedHeadAndLengthBinaryProtocol(byte[] headBytes, int length)
        {
            if (headBytes == null || headBytes.Length == 0) throw new ArgumentNullException("headBytes");
            if (length < 1) throw new ArgumentOutOfRangeException("length must larger than 0");


            HeadBytes = new byte[headBytes.Length];
            Buffer.BlockCopy(headBytes, 0, HeadBytes, 0, headBytes.Length);
            
            Length = length;
        }

        /// <summary>
        /// 分割符字节数组
        /// </summary>
        private byte[] HeadBytes { set; get; }

        /// <summary>
        /// 数据长度
        /// </summary>
        private int Length { set; get; }

        public override Messaging.BinaryMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            //var messageLength = buffer.Count;// - buffer.Offset;// Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset);
            if (buffer.Count < (HeadBytes.Length + Length))
            {
                readlength = 0;
                return null;
            }

            var receiveBuffer = buffer.Array;

            int maxIndex = buffer.Offset + buffer.Count - this.Length;// - EndBytes.Length;
            int loopIndex = buffer.Offset;
            
            while (loopIndex < maxIndex)
            {
                if (receiveBuffer.Mark(buffer.Offset, buffer.Count, loopIndex, HeadBytes))
                {
                    loopIndex += HeadBytes.Length;

                    readlength = loopIndex + Length - buffer.Offset;
                    if (readlength >= buffer.Count)
                    {
                        var payload = new byte[this.Length];

                        Buffer.BlockCopy(receiveBuffer, loopIndex, payload, 0, payload.Length);

                        return new Messaging.BinaryMessage(payload);
                    }

                    break;
                }

                loopIndex++;
            }

            readlength = 0;
            return null;
        }
    }
}
