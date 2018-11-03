using System;
using System.Net;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base
{
    /// <summary>
    /// a connection interface.
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// disconnected event
        /// </summary>
        event DisconnectedHandler Disconnected;

        string Tag { get; set; }

        /// <summary>
        /// return the connection is active.
        /// </summary>
        bool Active { get; }
        /// <summary>
        /// get the connection latest active time.
        /// </summary>
        DateTime LatestActiveTime { get; }
        /// <summary>
        /// get the connection id.
        /// </summary>
        long ConnectionID { get; }
        /// <summary>
        /// 获取本地IP地址
        /// </summary>
        IPEndPoint LocalEndPoint { get; }
        /// <summary>
        /// 获取远程IP地址
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }
        /// <summary>
        /// 获取或设置与用户数据
        /// </summary>
        object UserData { get; set; }

        /// <summary>
        /// 产生不重复的seqID
        /// </summary>
        /// <returns></returns>
        int NextRequestSeqID();

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="packet"></param>
        void BeginSend(Packet packet);
        /// <summary>
        /// 异步接收数据
        /// </summary>
        void BeginReceive();
        /// <summary>
        /// 异步断开连接, 如果ex == null， 就不会自动重连。直接删除该连接
        /// </summary>
        /// <param name="ex"></param>
        void BeginDisconnect(Exception ex = null);
    }
}