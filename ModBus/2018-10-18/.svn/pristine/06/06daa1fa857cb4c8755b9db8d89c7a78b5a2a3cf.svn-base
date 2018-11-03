using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Utils;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol.Filter
{
    /// <summary>
    /// 有头和尾标识符的模式
    /// </summary>
    public class FixedHeadAndEndBinaryProtocol : BinaryProtocol
    {
        public FixedHeadAndEndBinaryProtocol(byte[] headBytes, byte[] endBytes)
        {
            if (headBytes == null || headBytes.Length == 0) throw new ArgumentNullException("headBytes");
            if (endBytes == null || endBytes.Length == 0) throw new ArgumentNullException("endBytes");

            //HeadBytes = headBytes;
            //EndBytes = endBytes;

            HeadBytes = new byte[headBytes.Length];
            Buffer.BlockCopy(headBytes, 0, HeadBytes, 0, headBytes.Length);

            EndBytes = new byte[endBytes.Length];
            Buffer.BlockCopy(endBytes, 0, EndBytes, 0, endBytes.Length);
        }

        private class HeadAndEndMarked
        {
            public HeadAndEndMarked()
            {
                HeadIndex = -1;
                EndIndex = -1;
            }
            public int HeadIndex { set; get; }
            public int EndIndex { set; get; }
        }

        private byte[] HeadBytes { set; get; }

        private byte[] EndBytes { set; get; }

        public override Messaging.BinaryMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            //var messageLength = buffer.Count;// - buffer.Offset;// Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset);
            if (buffer.Count < (HeadBytes.Length + EndBytes.Length))
            {
                readlength = 0;
                return null;
            }

            var receiveBuffer = buffer.Array;

            int maxIndex = buffer.Offset + buffer.Count;// - EndBytes.Length;
            int loopIndex = buffer.Offset;

            int startIndex = -1;

            byte[] CompareBytes = HeadBytes;
            
            while (loopIndex < maxIndex)
            {
                if (receiveBuffer.Mark(buffer.Offset, buffer.Count, loopIndex, CompareBytes))
                {
                    loopIndex += CompareBytes.Length;

                    if(startIndex < 0)
                    {
                        startIndex = loopIndex;

                        CompareBytes = EndBytes;
                    } 
                    else
                    {
                        readlength = loopIndex - buffer.Offset;
                        if(readlength > (EndBytes.Length + HeadBytes.Length))
                        {
                            var payload = new byte[readlength - EndBytes.Length - HeadBytes.Length];

                            Buffer.BlockCopy(receiveBuffer, startIndex, payload, 0, payload.Length);

                            return new Messaging.BinaryMessage(payload);
                        }
                        return null;
                    }

                    continue;
                }

                loopIndex++;
            }

            readlength = 0;
            return null;
        }
    }
}
