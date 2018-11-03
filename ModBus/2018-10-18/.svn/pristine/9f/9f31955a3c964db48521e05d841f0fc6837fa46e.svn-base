using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging.log
{
    internal sealed class LoggerImpl : IDisposable
    {
        //Log Message queue  
        private Queue<LogMessage> mLogMessages;

        /// <summary>  
        /// Wait enqueue wirte log message semaphore will release  
        /// </summary>  
        private SemaphoreSlim mSemaphore;

        private volatile bool mIsDestoryed;

        private LinkedList<ILogger> mLoggerList;
        private LogMessageType mFilterType;

        public int Holding_Log_Day { get; private set; }

        public void SetHolding_Log_Day(int day)
        {
            this.Holding_Log_Day = Math.Max(5, day);
        }

        private static LoggerImpl gLogger;
        public static LoggerImpl Instance()
        {
            lock (typeof(LoggerImpl))
            {
                if(gLogger == null)
                {
                    gLogger = new LoggerImpl();
                }
            }

            return gLogger;
        }

        private Thread mThread;

        LoggerImpl()
        {
            this.Holding_Log_Day = 30;

            mIsDestoryed = false;
            mLogMessages = new Queue<LogMessage>();
            mSemaphore = new SemaphoreSlim(0, int.MaxValue);

            mLoggerList = new LinkedList<ILogger>();

#if DEBUG
            mLoggerList.AddLast(new ConsoleLogger());
#endif
            mFilterType = 0;

            var thread = new Thread(backgroundThreadProc) { IsBackground = true };
            mThread = thread;
            thread.IsBackground = true;
            thread.Priority = Logger.LogPriority;
            thread.Start();
        }

        public void SetLoggerFilter(LogMessageType t)
        {
            mFilterType = t;
        }

        public void ShowLoggerType()
        {
            lock (mLoggerList)
            {
                foreach(ILogger t in mLoggerList)
                {
                    Console.WriteLine("LoggerType:" + t.GetLoggerType());
                }
            }
        }

        public void SetLoggerType(LoggerType t, bool enable)
        {
            if (enable)
            {
                if (mIsDestoryed)
                {
                    throw new InvalidOperationException("LoggerImpl::SetLoggerType(), object had destoryed...");
                }

                lock (mLoggerList)
                {
                    LinkedListNode<ILogger> nodeNow = mLoggerList.First;
                    while (nodeNow != null)
                    {
                        if (nodeNow.Value.GetLoggerType().Equals(t))
                        {
                            return;
                        }

                        nodeNow = nodeNow.Next;
                    }

                    //foreach (ILogger l in mLoggerList)
                    //{
                    //    if (l.getLoggerType().Equals(t))
                    //    {
                    //        return;
                    //    }
                    //}
                }
                    
                ILogger log = null;
                switch (t)
                {
                    case LoggerType.Console:
                        log = new ConsoleLogger();
                        break;
                    case LoggerType.File:
                        log = new FileLogger();
                        break;
                    case LoggerType.Trace:
                        log = new DiagnosticLogger();
                        break;
                    default:
                        throw new NotSupportedException("Not support the type:"+t.ToString());                        
                }

                lock (mLoggerList)
                {
                    mLoggerList.AddLast(log);
                }
                
            }
            else
            {
                lock (mLoggerList)
                {
                    //foreach (ILogger l in mLoggerList)
                    LinkedListNode<ILogger> nodeNow = mLoggerList.First;
                    while(nodeNow != null)
                    {
                        if (nodeNow.Value.GetLoggerType().Equals(t))
                        {
                            mLoggerList.Remove(nodeNow);
                            break;
                        }

                        nodeNow = nodeNow.Next;
                    }
                }
            }
        }

        public void RegisterLogger(ILogger logger)
        {
            lock (mLoggerList)
            {
                LinkedListNode<ILogger> nodeNow = mLoggerList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Equals(logger))
                    {
                        return;
                    }

                    nodeNow = nodeNow.Next;
                }

                mLoggerList.AddLast(logger);
            }
        }

        public void UnRegisterLogger(ILogger logger)
        {
            lock (mLoggerList)
            {
                LinkedListNode<ILogger> nodeNow = mLoggerList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Equals(logger))
                    {
                        mLoggerList.Remove(nodeNow);
                        break;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }


        public void Dispose()
        {
            if (!mIsDestoryed)
                mThread.Priority = ThreadPriority.Normal;

            mIsDestoryed = true;

            mSemaphore.Release();

            try
            {
                Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoggerImpl::Dispose(), sleep exception:"+ex.ToString());
            }

            mSemaphore.Release();
        }

        public void Write(LogMessageType type, string tag, string message)
        {
            Write(type, tag, message, null);
        }

        public void Write(LogMessageType type, string tag, string message, Exception ex)
        {
            if (message == null) return;// throw new ArgumentNullException("message");

            if (mIsDestoryed)
            {
                //throw new InvalidOperationException("LoggerImpl::Write(), object had destoryed...");
                return;
            }

            if(type < mFilterType)
            {
                return;
            }

            try
            {
                LogMessage msg = new LogMessage(type, tag, message, ex);
                lock (mLogMessages)
                {
                    mLogMessages.Enqueue(msg);

                    if (mLogMessages.Count > 4096)
                    {
                        mLogMessages.Dequeue();
                    }
                }

                mSemaphore.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine("LoggerImpl::Write(), exception:" + e.ToString());
            }
        }

        private void write_log(LogMessage msg)
        {
            string str = msg.ToString();

            Model.Args.LogMessageEventArgs info = msg.ToMessage();

            lock (mLoggerList)
            {
                //mLoggerList.ForEach(c => c.Write(str, msg.Type));
                LinkedListNode<ILogger> nodeNow = mLoggerList.First;
                while (nodeNow != null)
                {
                    try
                    {
                        //nodeNow.Value.Write(str, msg.Tag, msg.LogType);
                        nodeNow.Value.Write(info);
                    }
                    catch (ObjectDisposedException)
                    {
                        LinkedListNode<ILogger> nodDel  = nodeNow;
                        nodeNow = nodeNow.Next;

                        mLoggerList.Remove(nodDel);

                        continue;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("LoggerImpl::write_log(), exception:" + e.ToString());
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            //return true;
        }

        private void backgroundThreadProc()
        {
            Console.WriteLine(">>>>>>>>>>>>>LoggerImpl::ThreadProc(start)>>>>>>>>>>>>>");
            while (true)
            {
                try
                {
                    LogMessage msg = null;
                    lock (mLogMessages)
                    {
                        if (mLogMessages.Count > 0)
                        {
                            msg = mLogMessages.Dequeue();
                        }
                    }

                    if (msg != null)
                    {
                        //mLoggerList.ForEach(c => c.Write(msg));
                        write_log(msg);

                        continue;
                    }
                    else
                    {
                        if (mIsDestoryed)
                            break;

                        remove_old_data();
                    }

                    mSemaphore.Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("LoggerImpl::ThreadProc error:"+ex.ToString());
                }
            }

            try
            {
                write_log(new LogMessage("LoggerImpl::ThreadProc() ready to exit...."));
            }
            catch (Exception)
            {
            }
            
            lock (mLoggerList)
            {
                LinkedListNode<ILogger> nodeNow = mLoggerList.First;
                while (nodeNow != null)
                {
                    nodeNow.Value.Dispose();
                    nodeNow = nodeNow.Next;
                }
                mLoggerList.Clear();
            }

            Console.WriteLine("<<<<<<<<<<<<<LoggerImpl::ThreadProc(end)<<<<<<<<<<<<<<<<<<");
        }

        private long mLastEntryRemoveTimeTicks = 0;
        private void remove_old_data()
        {
            double daycount = new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(mLastEntryRemoveTimeTicks)).TotalHours;

            if (daycount < 5)
            {
                return;
            }

            mLastEntryRemoveTimeTicks = DateTime.Now.Ticks;

            lock (mLoggerList)
            {
                //foreach (ILogger l in mLoggerList)
                LinkedListNode<ILogger> nodeNow = mLoggerList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.GetLoggerType() == LoggerType.File)
                    {
                        ((FileLogger)nodeNow.Value).Remove_old_file();
                        break;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

    }
}
