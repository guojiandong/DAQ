using Ksat.AppPlugIn.Communicate.SuperIoc.Protocol.Filter;
using Ksat.AppPlugIn.Model.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Base
{
    public interface ISession : IDisposable
    {
        void Initialize(AbstractCommunicationProfile profile);

        /// <summary>
        /// 同步锁
        /// </summary>
        object SyncLock { get; }

        /// <summary>
        /// IO关键字，如果是串口通讯为串口号，如：COM1;如果是网络通讯为IP和端口，例如：127.0.0.1:8080
        /// </summary>
        string Tag { get; }

        /// <summary>
        /// 唯一ID
        /// </summary>
        string SessionID { get; }

        /// <summary>
        /// 是否开启log功能
        /// </summary>
        bool EnableLog { get; set; }

        ///// <summary>
        ///// 通道实例，可以是COM，也可以是SOCKET
        ///// </summary>
        object Session { get; }
#if false
        /// <summary>
        /// 读IO，带过滤器
        /// </summary>
        /// <param name="receiveFilter"></param>
        /// <returns></returns>
        IList<byte[]> Read(IReceiveFilter receiveFilter);

        /// <summary>
        /// 写IO
        /// </summary>
        int Write(byte[] data);
#else
        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Read(byte[] data);

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        int Read(byte[] data, int offset, int length);

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Write(byte[] data);

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        int Write(byte[] data, int offset, int length);
#endif
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        /// <summary>
        /// IO类型
        /// </summary>
        CommunicateType CommunicationType { get; }

        /// <summary>
        /// 是否被释放了
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// 注册状态变化监听器
        /// </summary>
        /// <param name="listener"></param>
        void RegisterSessionStatusListener(ISessionStatusListener listener);

        /// <summary>
        /// 取消状态变化监听器
        /// </summary>
        /// <param name="listener"></param>
        void UnregisterSessionStatusListener(ISessionStatusListener listener);
    }
}
