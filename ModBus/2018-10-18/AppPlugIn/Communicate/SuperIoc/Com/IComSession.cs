using Ksat.AppPlugIn.Communicate.SuperIoc.Base;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Com
{
    /// <summary>
    /// 打开串口事件
    /// </summary>
    /// <param name="com"></param>
    /// <param name="port"></param>
    /// <param name="baud"></param>
    /// <param name="openSuccess"></param>
    internal delegate void COMOpenHandler(IComSession com, int port, int baud, bool openSuccess);

    /// <summary>
    /// 关闭串口事件
    /// </summary>
    /// <param name="com"></param>
    /// <param name="port"></param>
    /// <param name="baud"></param>
    /// <param name="closeSuccess"></param>
    internal delegate void COMCloseHandler(IComSession com, int port, int baud, bool closeSuccess);

    /// <summary>
    /// 串口错误
    /// </summary>
    /// <param name="com"></param>
    /// <param name="port"></param>
    /// <param name="baud"></param>
    /// <param name="error"></param>
    internal delegate void COMErrorHandler(IComSession com, int port, int baud, string error);


    internal interface IComSession : ISession
    {
        #region 属性
        /// <summary>
        /// 是否打开
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// 串口号
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 波特率
        /// </summary>
        int BaudRate { get; }

        /// <summary>
        /// 数据位
        /// </summary>
        int DataBits { get; }

        /// <summary>
        /// 停止位
        /// </summary>
        StopBits StopBits { get; }

        /// <summary>
        /// 检验位
        /// </summary>
        Parity Parity { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 尝试打开串口
        /// </summary>
        void Open();
#if false
        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int InternalRead(byte[] data);

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        int InternalRead(byte[] data, int offset, int length);

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int InternalWrite(byte[] data);

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        int InternalWrite(byte[] data, int offset, int length);
#endif
        /// <summary>
        /// 清空发送和接收缓冲区
        /// </summary>
        /// <returns></returns>
        void ClearBuffer();

        /// <summary>
        /// 配制串口
        /// </summary>
        /// <param name="port"></param>
        /// <param name="baud"></param>
        /// <returns></returns>
        bool Settings(int baud);

        /// <summary>
        /// 配制串口
        /// </summary>
        /// <param name="port"></param>
        /// <param name="baud"></param>
        /// <param name="databits"></param>
        /// <param name="stopbits"></param>
        /// <param name="parity"></param>
        /// <returns></returns>
        bool Settings(int baud, int databits, StopBits stopbits, Parity parity);
#endregion

#region 事件

        event SerialDataReceivedEventHandler DataReceived;

        ///// <summary>
        ///// 串口打开事件
        ///// </summary>
        //event COMOpenHandler COMOpen;

        ///// <summary>
        ///// 串口打开事件
        ///// </summary>
        //event COMCloseHandler COMClose;

        ///// <summary>
        ///// 串口异常
        ///// </summary>
        //event COMErrorHandler COMError;
#endregion
    }
}
