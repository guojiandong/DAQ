using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base
{
    public interface ISocketDataChangedListener<TMessage> : Base.ISocketStatusChangedListener where TMessage : class, Messaging.IMessage
    {
        ///// <summary>
        ///// 发送回调
        ///// </summary>
        ///// <param name="connection"></param>
        ///// <param name="packet"></param>
        ///// <param name="isSuccess"></param>
        //void OnSendCallback(IConnection connection, Base.Packet packet, bool isSuccess);

        /// <summary>
        /// 当接收到客户端新消息时，会调用此方法.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="message"></param>
        void OnReceived(IConnection connection, TMessage message);
    }
}
