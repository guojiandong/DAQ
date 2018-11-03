using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Protocol
{
    public abstract class AbstractProtocolBase<TMessage> : IProtocol<TMessage> where TMessage : class, Messaging.IMessage
    {
        /// <summary>
        /// return false
        /// </summary>
        public virtual bool IsAsync
        {
            get { return false; }
        }

        /// <summary>
        /// return 1
        /// </summary>
        public virtual int DefaultSyncSeqID
        {
            get { return 0; }
        }


        public abstract TMessage Parse(IConnection connection, ArraySegment<byte> buffer, out int readlength);
        //{
        //    throw new NotImplementedException();
        //}
    }
}
