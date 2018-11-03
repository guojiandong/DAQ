using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol
{
    /// <summary>
    /// tcp协议接口
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IProtocol<TMessage> where TMessage : class, Messaging.IMessage
    {
        /// <summary>
        /// true此协议为异步协议
        /// </summary>
        bool IsAsync { get; }

        /// <summary>
        /// 当IsAsync=false时，表示默认的seqID
        /// </summary>
        int DefaultSyncSeqID { get; }

        /// <summary>
        /// parse protocol message
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="readlength"></param>
        /// <returns></returns>
        TMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength);
    }
}
