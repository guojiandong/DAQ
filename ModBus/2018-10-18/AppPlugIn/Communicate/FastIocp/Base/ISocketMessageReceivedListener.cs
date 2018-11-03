using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base
{
    public interface ISocketMessageReceivedListener<TMessage> where TMessage : class, Messaging.IMessage
    {
        /// <summary>
        /// 当接收到客户端新消息时，会调用此方法.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="message"></param>
        void OnReceived(IConnection connection, TMessage message);
    }
}
