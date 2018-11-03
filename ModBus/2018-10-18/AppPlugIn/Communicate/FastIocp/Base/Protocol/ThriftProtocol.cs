using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol
{
    /// <summary>
    /// thrift protocol
    /// </summary>
    public class ThriftProtocol : AbstractProtocolBase<Messaging.ThriftMessage>
    {
        /// <summary>
        /// parse
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="readlength"></param>
        /// <returns></returns>
        /// <exception cref="BadProtocolException">bad thrift protocol</exception>
        public override Messaging.ThriftMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            if (buffer.Count < 4)
            {
                readlength = 0;
                return null;
            }

            //获取message length
            var messageLength = Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset);
            if (messageLength < 14) throw new Protocol.BadProtocolException("bad thrift protocol");
            //if (messageLength > maxMessageSize) throw new Base.Protocol.BadProtocolException("message is too long");

            readlength = messageLength + 4;
            if (buffer.Count < readlength)
            {
                readlength = 0;
                return null;
            }

            var cmdLen = Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset + 8);
            if (messageLength < cmdLen + 13) throw new Base.Protocol.BadProtocolException("bad thrift protocol");

            int seqID = Utils.NetworkBitConverter.ToInt32(buffer.Array, buffer.Offset + 12 + cmdLen);

            var payload = new byte[messageLength];
            Buffer.BlockCopy(buffer.Array, buffer.Offset + 4, payload, 0, messageLength);
            return new Messaging.ThriftMessage(seqID, payload);
        }
    }
}
