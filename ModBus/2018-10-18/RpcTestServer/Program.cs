using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcTestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.SetLoggerPath("D:\\Log");
            Logger.LogPriority = System.Threading.ThreadPriority.Normal;
            Logger.EnableLogger(LoggerType.Console, LoggerType.File);

            LocalIpcRequestServer server = new LocalIpcRequestServer(9805);
            server.Start();

            Console.WriteLine("Press any ke to exit.");
            Console.ReadKey();

            server.Stop();
        }


        public class LocalIpcRequestServer : KsatRpcLibrary.Ipc.IpcTcpServer
        {
            public LocalIpcRequestServer() : this(KsatRpcLibrary.Ipc.IpcTcpServer.CONST_DEFAULT_PORT)
            {
            }

            public LocalIpcRequestServer(int port) : base(port)
            {
                //sInstance = this;
            }


            protected override void OnStart()
            {
                base.OnStart();

                RegisterApi<RequestApiServer>(typeof(KsatRpcLibrary.IpcActionRequestApi));

                Console.WriteLine("IpcServer", "Started...");
            }
        }

        static int gMaxThreadCount = 0;
        static List<int> gCacheThreadId = new List<int>();

        public class RequestApiServer : KsatRpcLibrary.AbstractRequestApiImpl
        {
            public override KsatRpcLibrary.ResponseEventArgs SendAction(KsatRpcLibrary.RequestActionEventArgs message)
            {
                int current_threadid = System.Threading.Thread.CurrentThread.GetHashCode();
                lock(gCacheThreadId)
                {
                    if (!gCacheThreadId.Contains(current_threadid))
                    {
                        gCacheThreadId.Add(current_threadid);
                        Logger.Info("IpcServerNewThread", "Thread:" + current_threadid.ToString("X4") + ", Count:" + gCacheThreadId.Count + ", Recv Data:" + message.ToString());
                    }
                    else
                    {
                        Logger.Info("IpcServer", "Thread:" + current_threadid.ToString("X4") + ", Recv Data:" + message.ToString());
                    }
                }

                return message.ToResponse("Resp:"+message.Value, KsatRpcLibrary.ResponseEventArgs.SUCCESS);
            }
        }
    }
}
