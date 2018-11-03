using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Utils;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol.Filter
{
    /// <summary>
    /// 消息有开始字符
    /// </summary>
    public class FixedHeadBinaryProtocol : BinaryProtocol
    {
        public FixedHeadBinaryProtocol(byte[] headBytes)
        {
            if (headBytes == null || headBytes.Length == 0) throw new ArgumentNullException("headBytes");

            HeadBytes = new byte[headBytes.Length];
            Buffer.BlockCopy(headBytes, 0, HeadBytes, 0, headBytes.Length);
            //HeadBytes = headBytes;
        }

        private byte[] HeadBytes { set; get; }


        public override Messaging.BinaryMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            //var messageLength = buffer.Count;// - buffer.Offset;// Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset);
            if (buffer.Count < HeadBytes.Length * 2)
            {
                readlength = 0;
                return null;
            }

            var receiveBuffer = buffer.Array;

            int maxIndex = buffer.Offset + buffer.Count;// - HeadBytes.Length;
            int loopIndex = buffer.Offset;
            int startIndex = -1, endIndex = -1;
            while (loopIndex < maxIndex)
            {
                if (receiveBuffer.Mark(buffer.Offset, buffer.Count, loopIndex, HeadBytes))
                {
                    if (startIndex >= 0)
                    {
                        endIndex = loopIndex;
                        break;
                    }

                    loopIndex += HeadBytes.Length;
                    
                    startIndex = loopIndex;
                }
                else
                {
                    loopIndex++;
                }
            }

            if(endIndex > 0)
            {
                readlength = endIndex - buffer.Offset;// + HeadBytes.Length;
                if(readlength - HeadBytes.Length > 0)
                {
                    var payload = new byte[readlength - HeadBytes.Length];

                    Buffer.BlockCopy(receiveBuffer, startIndex, payload, 0, payload.Length);

                    return new Messaging.BinaryMessage(payload);
                }
                return null;
            }

            readlength = 0;          
            return null;
        }
    }
}
