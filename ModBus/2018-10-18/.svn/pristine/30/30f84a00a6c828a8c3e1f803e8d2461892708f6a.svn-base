using Ksat.AppPlugIn.Communicate.SuperIoc.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Net
{
    /// <summary>
    /// 侦听错误事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void ErrorHandler(object source, Exception e);

    /// <summary>
    /// 接收客户端事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="client"></param>
    /// <param name="state"></param>
    public delegate void NewClientAcceptHandler(object source, Socket client, object state);

    /// <summary>
    /// 关闭Socket事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void CloseSocketHandler(object source, ISocketSession socketSession);

    ///// <summary>
    ///// 接收数据
    ///// </summary>
    ///// <param name="source"></param>
    ///// <param name="socketSession"></param>
    ///// <param name="dataPackage"></param>
    //public delegate void SocketReceiveDataHandler(object source, ISocketSession socketSession, IReceivePackage dataPackage);


    public interface ISocketSession : ISession
    {

        #region 属性
        /// <summary>
        /// 远程网络连接的IP
        /// </summary>
        string RemoteIP { get; }

        /// <summary>
        /// 远程网络连接的Port
        /// </summary>
        int RemotePort { get; }

        /// <summary>
        /// Socket实例
        /// </summary>
        Socket Client { get; }

        /// <summary>
        /// 远程点
        /// </summary>
        //IPEndPoint RemoteEndPoint { get; }


        /// <summary>
        /// 是否联机
        /// </summary>
        bool IsConnected { get; }

        ///// <summary>
        ///// 异步代理
        ///// </summary>
        //ISocketAsyncEventArgsProxy SocketAsyncProxy { get; }

        /// <summary>
        /// 关闭连接
        /// </summary>
        //event CloseSocketHandler CloseSocket;

        /// <summary>
        /// 接收数据
        /// </summary>
        //event SocketReceiveDataHandler SocketReceiveData;

        /////// <summary>
        /////// 接收数据
        /////// </summary>
        //void TryReceive();

        ///// <summary>
        ///// 发送数据
        ///// </summary>
        ///// <param name="type">true:异步，false:同步</param>
        //void TrySend(byte[] data, bool type);

        /// <summary>
        /// 链接的时间 
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// 接收有效数据的时间
        /// </summary>
        DateTime LastActiveTime { get; set; }

        /// <summary>
        /// 自动重连间隔时间, 
        /// -1 : 禁止自动重连
        /// </summary>
        int AutoReconnectInterval { get; set; }

        #endregion

        int TryConnect(int timeout = 0);
    }
}
