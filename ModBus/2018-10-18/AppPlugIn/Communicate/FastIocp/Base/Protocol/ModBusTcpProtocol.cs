using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn;
using Ksat.AppPlugIn.Communicate.FastIocp.Base;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol;
using Ksat.AppPlugIn.Utils;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol
{
    public class ModBusTcpProtocol : AbstractProtocolBase<AbstractModBusTcpMessage>
    {
        public static readonly byte[] HEADER = new byte[] { 0x00, 0x00, 0x00, 0x00 };

        public ModBusTcpProtocol()
        {
        }


        public override AbstractModBusTcpMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            readlength = 0;
            if (buffer.Count < (AbstractModBusTcpMessage.HEADER_FLAG.Length))
            {
                return null;
            }

            var receiveBuffer = buffer.Array;
            int maxIndex = buffer.Offset + buffer.Count;
            int loopIndex = buffer.Offset;
            try
            {
                while (loopIndex < maxIndex)
                {
                    if (receiveBuffer.Mark(buffer.Offset, buffer.Count, loopIndex, AbstractModBusTcpMessage.HEADER_FLAG))
                    {
                        loopIndex += AbstractModBusTcpMessage.HEADER_FLAG.Length;

                        AbstractModBusTcpMessage msg = AbstractModBusTcpMessage.TryParse(receiveBuffer, loopIndex, maxIndex - loopIndex, out readlength);
                        if (msg == null)
                        {
                            Ksat.AppPlugIn.Logging.Logger.Warn(this.GetType().Name, "parse FinsTcpMessage failed:"
                                + (connection.Tag != null ? connection.Tag : "null")
                                + ", beginIndex:" + loopIndex + ", endIndex:" + maxIndex + ", count:" + (maxIndex - loopIndex)
                                + ", " + (connection.RemoteEndPoint != null ? connection.RemoteEndPoint.ToString() : "unkown"));

                            break;
                        }

                        readlength += loopIndex - buffer.Offset;

                        return msg;
                    }
                    else
                    {
                        loopIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                Ksat.AppPlugIn.Logging.Logger.Warn(this.GetType().Name, "parse exception:" + ex.ToString());
            }

            readlength = 0;
            return null;
        }
    }
}
