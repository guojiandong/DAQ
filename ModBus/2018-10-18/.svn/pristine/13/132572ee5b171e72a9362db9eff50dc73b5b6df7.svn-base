using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcTesterClient
{
    class Program
    {
        static void Main(string[] args)
        {
#if false
            Logger.SetLoggerPath("D:\\Log");
            Logger.LogPriority = System.Threading.ThreadPriority.Normal;
            Logger.EnableLogger(LoggerType.Console, LoggerType.File);
#else
            Logger.EnableLogger(LoggerType.Console);
#endif
            System.Threading.Thread.Sleep(1000);

            // 配置远程服务器ip地址和端口
            // 192.168.1.252
            //KsatRpcLibrary.SingleClientApi.Instance().SetUrl(9805, "127.0.0.1", "192.168.253.1");
            KsatRpcLibrary.SingleClientApi.Instance().SetUrl(9805, "172.17.212.27");


            for (int i=0; i<30; i++)
            {
                var thread = new System.Threading.Thread(backgroundThreadProc) { IsBackground = true };
                //mThread = thread;
                thread.IsBackground = true;
                //thread.Priority = Logger.LogPriority;
                thread.Start(i);
            }
            

            Console.WriteLine("Press any ke to exit.");
            Console.ReadKey();

        }

        static Dictionary<int, long> mCacheCount = new Dictionary<int, long>();
        static Dictionary<int, long> mStartTime = new Dictionary<int, long>();

        static int gCurrentThreadCount = 0;

        private static void backgroundThreadProc(object obj)
        {
            lock(mStartTime)
                gCurrentThreadCount++;

            int index = (int)obj;
            System.Diagnostics.Stopwatch totalrunwatch = new System.Diagnostics.Stopwatch();
            totalrunwatch.Start();

            //mStartTime.Add(index, );

            for (int i = 0; i < 2000; i++)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                // 提交数据
                KsatRpcLibrary.ResponseEventArgs resp = null;
#if true
                resp = KsatRpcLibrary.SingleClientApi.Instance().SendAction("index["+ index + "]:" + i, "test[" + index + "]:" + i);
#else
                if (i % 2 == 0)
                    resp = KsatRpcLibrary.SingleClientApi.Instance().SendAction("index:" + i, "test:" + i);
                else
                    resp = KsatRpcLibrary.SingleClientApi.Instance().SendAction("index:" + i, "test:" + i, 1000);
#endif
                stopwatch.Stop();

                string slowtime = "";
                if (stopwatch.ElapsedMilliseconds >= 5)
                    slowtime = "Slow";
                //else if (stopwatch.ElapsedMilliseconds >= 5)
                //    slowtime = "Slow";
                int key = 0;
                if (stopwatch.ElapsedMilliseconds >= 5000)
                    key = 5000;
                else if (stopwatch.ElapsedMilliseconds >= 4000)
                    key = 4000;
                else if (stopwatch.ElapsedMilliseconds >= 3000)
                    key = 3000;
                else if (stopwatch.ElapsedMilliseconds >= 2000)
                    key = 2000;
                else if (stopwatch.ElapsedMilliseconds >= 1000)
                    key = 1000;
                else if (stopwatch.ElapsedMilliseconds >= 500)
                    key = 500;
                else if(stopwatch.ElapsedMilliseconds >= 400)
                    key = 400;
                else if (stopwatch.ElapsedMilliseconds >= 300)
                    key = 300;
                else if (stopwatch.ElapsedMilliseconds >= 200)
                    key = 200;
                else if (stopwatch.ElapsedMilliseconds >= 100)
                    key = 100;
                else if (stopwatch.ElapsedMilliseconds >= 90)
                    key = 90;
                else if (stopwatch.ElapsedMilliseconds >= 80)
                    key = 80;
                else if (stopwatch.ElapsedMilliseconds >= 70)
                    key = 70;
                else if (stopwatch.ElapsedMilliseconds >= 60)
                    key = 60;
                else if (stopwatch.ElapsedMilliseconds >= 50)
                    key = 50;
                else if (stopwatch.ElapsedMilliseconds >= 40)
                    key = 40;
                else if (stopwatch.ElapsedMilliseconds >= 30)
                    key = 30;
                else if (stopwatch.ElapsedMilliseconds >= 20)
                    key = 20;
                else if (stopwatch.ElapsedMilliseconds >= 10)
                    key = 10;
                else if (stopwatch.ElapsedMilliseconds >= 5)
                    key = 5;
                else if (stopwatch.ElapsedMilliseconds >= 1)
                    key = 1;

                lock (mCacheCount)
                {
                    if (mCacheCount.ContainsKey(key))
                        mCacheCount[key]++;
                    else
                        mCacheCount.Add(key, 1);
                }

                if (resp.IsSuccess())
                {
                    Logger.Info("IpcClient", "Thread[" + index + "]:" + System.Threading.Thread.CurrentThread.GetHashCode().ToString("X4") + ", Send message:" + i + ", "+ slowtime + " time:" + stopwatch.ElapsedMilliseconds + " Ms, success, :" + resp.ToString());
                }
                else
                {
                    Logger.Info("IpcClient", "Thread[" + index + "]:" + System.Threading.Thread.CurrentThread.GetHashCode().ToString("X4") + ", Send message:" + i + "," + slowtime + " time:" + stopwatch.ElapsedMilliseconds + " Ms, failed, :" + resp.ToString());
                }

                System.Threading.Thread.Sleep(10);
            }

            totalrunwatch.Stop();

            
            lock(mStartTime)
            {
                mStartTime.Add(index, totalrunwatch.ElapsedMilliseconds / 1000);
                gCurrentThreadCount--;
            }
            

            Logger.Info("IpcClientTime", "Thread[" + index + "]:" + System.Threading.Thread.CurrentThread.GetHashCode().ToString("X8") 
                + ", total time:" + totalrunwatch.ElapsedMilliseconds
                + " (" + totalrunwatch.Elapsed.Seconds+")"
                + ", gCurrentThreadCount:" + gCurrentThreadCount);

            if(gCurrentThreadCount == 0)
            {
                foreach (KeyValuePair<int, long> item in mStartTime)
                {
                    Logger.Info("IpcClientResult", " Thread:" + item.Key
                        + ", Time: " + item.Value + " S ");
                }

                long totalcount = 0;
                foreach (KeyValuePair<int, long> item in mCacheCount)
                {
                    totalcount += item.Value;
                }

                foreach (KeyValuePair<int, long> item in mCacheCount.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value))
                {
                    Logger.Info("IpcClientResult",
                            String.Format("Time:{0:d4} = {1:d8}, Total:{2:d10}, Rate:{3:f1}",
                            item.Key, item.Value, totalcount, ((float)item.Value * 100 / totalcount)));

                    //if (item.Key < 10)
                    //{
                        
                    //    Logger.Info("IpcClientResult", 
                    //        String.Format("Time:{0:-4} = {1:-8}, Total:{2:-10}, Rate:{3:f1}", 
                    //        item.Key, item.Value, totalcount, ((float)item.Value * 100 / totalcount)));
                    //}
                    //else if(item.Key < 100)
                    //{
                    //    Logger.Info("IpcClientResult", " Time:00" + item.Key
                    //    + " = " + item.Value + ", " + totalcount + ", " + ((float)item.Value * 100 / totalcount));
                    //}
                    //else if (item.Key < 1000)
                    //{
                    //    Logger.Info("IpcClientResult", " Time:0" + item.Key
                    //    + " = " + item.Value + ", " + totalcount + ", " + ((float)item.Value * 100 / totalcount));
                    //}
                    //else
                    //{
                    //    Logger.Info("IpcClientResult", " Time:" + item.Key
                    //    + " = " + item.Value + ", " + totalcount + ", " + ((float)item.Value * 100 / totalcount));
                    //}
                }
            }
        }
    }
}
