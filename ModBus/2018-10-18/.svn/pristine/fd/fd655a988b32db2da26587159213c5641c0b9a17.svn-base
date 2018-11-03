using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.Base
{
    public interface IStatusChangedListener
    {
        /// <summary>
        /// 当建立socket连接时，会调用此方法
        /// </summary>
        /// <param name="sender"></param>
        void OnConnected(object sender);

        /// <summary>
        /// 当socket连接断开时，会调用此方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        void OnDisconnected(object sender, Exception ex);

        /// <summary>
        /// 当发生异常时，会调用此方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        void OnException(object sender, Exception ex);
    }
}
