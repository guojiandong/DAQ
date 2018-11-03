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
    public class CommandLineProtocol : AbstractProtocolBase<Messaging.CommandLineMessage>
    {
        static protected readonly string[] SPLITER =
            new string[] { " " };

        /// <summary>
        /// parse
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="readlength"></param>
        /// <returns></returns>
        /// <exception cref="BadProtocolException">bad command line protocol</exception>
        public override Messaging.CommandLineMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength)
        {
            if (buffer.Count < 2)
            {
                readlength = 0;
                return null;
            }

            //查找\r\n标记符
            for (int i = buffer.Offset, len = buffer.Offset + buffer.Count; i < len; i++)
            {
                if (buffer.Array[i] == 13 && i + 1 < len && buffer.Array[i + 1] == 10)
                {
                    readlength = i + 2 - buffer.Offset;

                    if (readlength == 2) return new Messaging.CommandLineMessage(DefaultSyncSeqID, string.Empty);
                    //if (readlength > maxMessageSize) throw new Base.Protocol.BadProtocolException("message is too long");

                    string command = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, readlength - 2);
                    var arr = command.Split(SPLITER, StringSplitOptions.RemoveEmptyEntries);

                    if (arr.Length == 0) return new Messaging.CommandLineMessage(DefaultSyncSeqID, string.Empty);
                    if (arr.Length == 1) return new Messaging.CommandLineMessage(DefaultSyncSeqID, arr[0]);
                    return new Messaging.CommandLineMessage(DefaultSyncSeqID, arr[0], arr.Skip(1).ToArray());
                }
            }
            readlength = 0;
            return null;
        }
    }
}
