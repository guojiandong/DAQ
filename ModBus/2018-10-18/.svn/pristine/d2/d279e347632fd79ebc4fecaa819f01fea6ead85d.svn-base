using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base
{
    /// <summary>
    /// abstract socket status changed interface.
    /// </summary>
    public class AbstractSocketStatusChangedListener : ISocketStatusChangedListener
    {
        /// <summary>
        /// 当建立socket连接时，会调用此方法
        /// </summary>
        /// <param name="connection"></param>
        public virtual void OnConnected(Base.IConnection connection)
        {
        }
        
        /// <summary>
        /// 当socket连接断开时，会调用此方法
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ex"></param>
        public virtual void OnDisconnected(Base.IConnection connection, Exception ex)
        {
        }
        /// <summary>
        /// 当发生异常时，会调用此方法
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ex"></param>
        public virtual void OnException(Base.IConnection connection, Exception ex)
        {
        }

        /// <summary>
        /// 发送回调
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="packet"></param>
        /// <param name="isSuccess"></param>
        public virtual void OnSendCallback(Base.IConnection connection, Base.Packet packet, bool isSuccess)
        {
        }
    }
}
