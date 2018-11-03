using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol
{
    public class StringProtocol : AbstractProtocolBase<Messaging.StringMessage>
    {
        public override StringMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            var messageLength = buffer.Count;// - buffer.Offset;// Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset);
            if (messageLength < 1)
            {
                readlength = 0;
                return null;
            }

            //var payload = new byte[messageLength];
            //Buffer.BlockCopy(buffer.Array, buffer.Offset, payload, 0, messageLength);

            var payload = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, messageLength);

            readlength = messageLength;

            return new Messaging.StringMessage(payload);
        }
    }
}
