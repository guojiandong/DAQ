using System;
using System.Runtime.InteropServices;

namespace Ksat.SimpleRpcLib
{
    [Guid("807E0C0A-B0C6-4CE2-8398-E614D3484221")]
    public sealed class SingleClientApi : IpcRequestApi, IpcActionRequestApi, IDisposable
    {
        private static SingleClientApi sInstance;

        public static SingleClientApi Instance()
        {
            lock (typeof(SingleClientApi))
            {
                if (sInstance == null)
                    sInstance = new SingleClientApi();
            }
            return sInstance;
        }

        private Ipc.IpcTcpClient mIpcTcpClient;

        private SingleClientApi()
        {
            this.Host = "localhost";
            this.BackupHost = "";
            this.Port = Ipc.IpcServer.CONST_DEFAULT_PORT;

            //mIpcTcpClient = new Ipc.IpcTcpClient();
        }

        /// <summary>
        /// 默认服务器  localhost
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 默认服务器  localhost
        /// </summary>
        public string BackupHost { get; set; }

        /// <summary>
        /// 默认端口 9952
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 设置服务器地址
        /// </summary>
        /// <param name="host">主服务器ip</param>
        /// <param name="port">监听端口</param>
        /// <param name="backuphost">备用服务器ip</param>
        public void SetUrl(string host, int port)
        {
            this.Host = host;
            this.Port = port;
            this.BackupHost = "";
        }

        public void SetBackupUrl(string host)
        {
            this.BackupHost = host;
        }

        /// <summary>
        /// 启动客户端
        /// </summary>
        public void Start()
        {
            lock (typeof(SingleClientApi))
            {
                if (mIpcTcpClient == null)
                    mIpcTcpClient = new Ipc.IpcTcpClient(this.Host, this.Port);
            }

            mIpcTcpClient.Start();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Dispose()
        {
            lock (typeof(SingleClientApi))
            {
                if (mIpcTcpClient != null)
                {
                    mIpcTcpClient.Stop();
                    mIpcTcpClient = null;
                }
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="action">标识</param>
        /// <param name="value">发送内容</param>
        /// <returns></returns>
        public ResponseEventArgs SendAction(string action, string value)
        {
            return this.SendAction(new RequestActionEventArgs(action, value));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResponseEventArgs SendAction(RequestActionEventArgs message)
        {
            if (message == null)
                throw new ArgumentNullException("invalid parameters. message can't be null.");

            if (mIpcTcpClient == null || !mIpcTcpClient.IsStarted())
                this.Start();

            try
            {
                IpcActionRequestApi api = mIpcTcpClient.FindApiObject<IpcActionRequestApi>();

                if (api == null)
                {
                    Console.WriteLine("SendAction(0), find IpcActionRequestApi failed...");
                    return message.ToResponse(ResponseEventArgs.NOT_CONNECT_TO_SERVER, "no connect to server");
                }

                return api.SendAction(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendAction(2), send exception:" + ex.ToString());

                return message.ToResponse(ResponseEventArgs.NOT_CONNECT_TO_SERVER, ex.ToString());
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="machineid">工站编号</param>
        /// <param name="stationid">工位编号</param>
        /// <param name="value">发送内容</param>
        /// <param name="action">标识</param>
        /// <returns></returns>
        public ResponseEventArgs SendMessage(int machineid, int stationid, string value, string action = "")
        {
            return this.SendMessage(new RequestEventArgs()
            {
                MachineID = machineid,
                StationID = stationid,
                Value = value,
                Action = action,
            });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResponseEventArgs SendMessage(RequestEventArgs message)
        {
            if (message == null)
                throw new ArgumentNullException("invalid parameters. message can't be null.");

            if (mIpcTcpClient == null || !mIpcTcpClient.IsStarted())
                this.Start();

            try
            {
                IpcRequestApi api = mIpcTcpClient.FindApiObject<IpcRequestApi>();

                if (api == null)
                {
                    Console.WriteLine("SendMessage(0), find IpcRequestApi failed...");
                    return message.ToResponse(ResponseEventArgs.NOT_CONNECT_TO_SERVER, "no connect to server");
                }

                return api.SendMessage(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendMessage(2), send exception:"+ex.ToString());

                return message.ToResponse(ResponseEventArgs.NOT_CONNECT_TO_SERVER, ex.ToString());
            }
        }

        
    }
}
