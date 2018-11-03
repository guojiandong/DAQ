using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging;
using Ksat.AppPlugIn.Utils;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol
{
    /// <summary>
    /// 欧姆龙FinsTcp协议
    /// </summary>
    public class FinsTcpProtocol : AbstractProtocolBase<Messaging.AbstractFinsTcpMessage>
    {
        //public static readonly byte[] HEADER = new byte[] { 0x46, 0x49, 0x4E, 0x53 };

        public FinsTcpProtocol()
        {
        }

        public override AbstractFinsTcpMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            readlength = 0;
            if (buffer.Count < (AbstractFinsTcpMessage.HEADER_FLAG.Length + 12))
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
                    if (receiveBuffer.Mark(buffer.Offset, buffer.Count, loopIndex, AbstractFinsTcpMessage.HEADER_FLAG))
                    {
                        loopIndex += AbstractFinsTcpMessage.HEADER_FLAG.Length;

                        AbstractFinsTcpMessage msg = AbstractFinsTcpMessage.TryParse(receiveBuffer, loopIndex, maxIndex - loopIndex, out readlength);
                        if (msg == null)
                        {
                            Logging.Logger.Warn(this.GetType().Name, "parse FinsTcpMessage failed:" 
                                + (connection.Tag != null ? connection.Tag : "null")
                                + ", beginIndex:"+ loopIndex+ ", endIndex:" + maxIndex+", count:"+(maxIndex - loopIndex)
                                + ", "+ (connection.RemoteEndPoint != null ? connection.RemoteEndPoint.ToString() : "unkown"));

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
            catch(Exception ex)
            {
                Logging.Logger.Warn(this.GetType().Name, "parse exception:"+ex.ToString());
            }
            

            readlength = 0;
            return null;
        }
    }
}
