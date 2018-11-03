using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol
{
    /// <summary>
    /// 命令行协议
    /// </summary>
    public class BinaryProtocol : AbstractProtocolBase<Messaging.BinaryMessage>
    {
        public override BinaryMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            var messageLength = buffer.Count;// - buffer.Offset;// Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset);
            if (messageLength < 1)
            {
                readlength = 0;
                return null;
            }

            var payload = new byte[messageLength];
            Buffer.BlockCopy(buffer.Array, buffer.Offset, payload, 0, messageLength);

            readlength = messageLength;

            return new Messaging.BinaryMessage(payload);
        }
    }
}
