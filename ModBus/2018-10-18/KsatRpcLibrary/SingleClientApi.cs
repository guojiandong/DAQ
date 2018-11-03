using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace KsatRpcLibrary
{
    [Guid("9AC38D9F-C88A-4041-B9B8-184A832407BA")]
    public class SingleClientApi : IpcActionRequestApi, IDisposable
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

        private Ipc.IpcTcpClient mCurrentClient;
        private Ipc.IpcTcpClient mIpcTcpClient;
        private Ipc.IpcTcpClient mIpcBackupTcpClient;

        private System.Threading.Thread mThread;

        public SingleClientApi()
        {
            sInstance = this;

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
        /// <param name="port">监听端口</param>
        /// <param name="host">主服务器ip</param>
        /// <param name="backuphost">备用服务器ip</param>
        public void SetUrl(int port, string host, string backuphost = "")
        {
            this.Host = host;
            this.Port = port;
            this.BackupHost = backuphost;
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

                if(mIpcBackupTcpClient == null && !String.IsNullOrEmpty(this.BackupHost))
                    mIpcBackupTcpClient = new Ipc.IpcTcpClient(this.BackupHost, this.Port);

                mCurrentClient = mIpcTcpClient;
            }

            mIpcTcpClient.Start();

            if (mIpcBackupTcpClient != null)
            {
                mIpcBackupTcpClient.Start();
#if false
                if (mThread != null)
                {
                    var thread = new System.Threading.Thread(backgroundThreadProc);
                    mThread = thread;
                    thread.IsBackground = true;
                    thread.Priority = System.Threading.ThreadPriority.BelowNormal;
                    thread.Start();
                }
#endif
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Dispose()
        {
            lock (typeof(SingleClientApi))
            {
                mCurrentClient = null;

                if (mIpcTcpClient != null)
                {
                    mIpcTcpClient.Stop();
                    mIpcTcpClient = null;
                }

                if (mIpcBackupTcpClient != null)
                {
                    mIpcBackupTcpClient.Stop();
                    mIpcBackupTcpClient = null;
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

        public ResponseEventArgs SendAction(string action, string value, int timeout)
        {
            return this.SendAction(new RequestActionEventArgs(action, value), timeout);
        }

        public ResponseEventArgs SendAction(RequestActionEventArgs message)
        {
            try
            {
                return doSendAction(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendAction(2),SeqID:" + message.SeqID+" send failed, exception:" + ex.Message);

                if (ex is System.Net.Sockets.SocketException)
                {
#if DEBUG
                    Console.WriteLine("SendAction(5), SocketErrorCode:" + (ex as System.Net.Sockets.SocketException).SocketErrorCode);
#endif
                    if ((ex as System.Net.Sockets.SocketException).SocketErrorCode > System.Net.Sockets.SocketError.NetworkDown
                            && (ex as System.Net.Sockets.SocketException).SocketErrorCode < System.Net.Sockets.SocketError.TryAgain)
                    {
                        bool resend = false;
                        lock (typeof(SingleClientApi))
                        {
                            if (mIpcBackupTcpClient != null)
                            {
                                if (mCurrentClient.Equals(mIpcTcpClient))
                                    mCurrentClient = mIpcBackupTcpClient;
                                else
                                    mCurrentClient = mIpcTcpClient;

                                resend = true;
                                Console.WriteLine("SendAction(6), switch server.");
                            }
                        }
                        
                        if(resend)
                        {
                            try
                            {
                                return doSendAction(message);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("SendAction(8),SeqID:" + message.SeqID + " resend exception:" + e.Message);
                            }
                        }
                    }

                    if ((ex as System.Net.Sockets.SocketException).SocketErrorCode == System.Net.Sockets.SocketError.ConnectionRefused)
                    {
                        return message.ToResponse(ex.Message, ResponseEventArgs.CONNECT_REFUSED);
                    }
                }

                return message.ToResponse(ex.Message, ResponseEventArgs.NOT_CONNECT_TO_SERVER);
            }
        }

        private ResponseEventArgs doSendAction(RequestActionEventArgs message)
        {
            if (message == null)
                throw new ArgumentNullException("invalid parameters. message can't be null.");

            bool mustStart = false;
            lock (typeof(SingleClientApi))
            {
                if (mCurrentClient == null || !mCurrentClient.IsStarted())
                    mustStart = true;
            }

            if (mustStart)
                this.Start();

            IpcActionRequestApi api = mCurrentClient.FindApiObject<IpcActionRequestApi>();

            if (api == null)
            {
                Console.WriteLine("SendAction(0), find IpcActionRequestApi failed...");
                return message.ToResponse("ipc init failed, please call start first", ResponseEventArgs.IPC_INIT_FAILED);
            }

            return api.SendAction(message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResponseEventArgs SendAction(RequestActionEventArgs message, int timeout)
        {
            if (timeout == 0)
                return SendAction(message);

            try
            {
                System.Threading.AutoResetEvent waiting_event = new System.Threading.AutoResetEvent(false);

                ResponseEventArgs resp = message.ToResponse("Waiting for server response timeout:" + message.SeqID, ResponseEventArgs.RESPONSE_TIMEOUT);

                System.Threading.ThreadPool.QueueUserWorkItem(_ => doAsyncSendAction(message, waiting_event, out resp));

                waiting_event.WaitOne(timeout);

                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendAction(3), send with timeou exception:" + ex.ToString());

                return message.ToResponse(ex.ToString(), ResponseEventArgs.CAUSE_EXCEPTION);
            }
        }

        private void doAsyncSendAction(RequestActionEventArgs req, System.Threading.AutoResetEvent waiting_event, out ResponseEventArgs resp)
        {
            resp = this.SendAction(req);

            if(waiting_event != null)
                waiting_event.Set();
        }


        private void backgroundThreadProc()
        {
            while(mCurrentClient != null)
            {
                try
                {

                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
