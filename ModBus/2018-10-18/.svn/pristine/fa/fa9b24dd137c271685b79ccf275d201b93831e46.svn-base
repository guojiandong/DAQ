using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Utils;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol.Filter
{
    /// <summary>
    /// 消息有结束字符
    /// </summary>
    public class FixedEndBinaryProtocol : BinaryProtocol
    {
        public FixedEndBinaryProtocol(byte[] endBytes)
        {
            if (endBytes == null || endBytes.Length == 0) throw new ArgumentNullException("endBytes");

            EndBytes = new byte[endBytes.Length];
            Buffer.BlockCopy(endBytes, 0, EndBytes, 0, endBytes.Length);
            //HeadBytes = headBytes;
        }

        private byte[] EndBytes { set; get; }

        public override Messaging.BinaryMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            //var messageLength = buffer.Count;// - buffer.Offset;// Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset);
            if (buffer.Count < EndBytes.Length)
            {
                readlength = 0;
                return null;
            }

            var receiveBuffer = buffer.Array;

            int maxIndex = buffer.Offset + buffer.Count;// - EndBytes.Length;
            int loopIndex = buffer.Offset;
            while (loopIndex < maxIndex)
            {
                if (receiveBuffer.Mark(buffer.Offset, buffer.Count, loopIndex, EndBytes))
                {
                    loopIndex += EndBytes.Length;
                    
                    readlength = loopIndex - buffer.Offset;
                    if(readlength > EndBytes.Length)
                    {
                        var payload = new byte[readlength - EndBytes.Length];

                        Buffer.BlockCopy(receiveBuffer, buffer.Offset, payload, 0, payload.Length);

                        return new Messaging.BinaryMessage(payload);
                    }
                    return null;
                }
                else
                {
                    loopIndex++;
                }
            }

            readlength = 0;
            return null;
        }

    }
}
