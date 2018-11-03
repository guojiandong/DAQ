using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading
{
    [Guid("1D31436F-5529-4578-8C57-0B92C5BFB928")]
    public class MultiThreadManager : Utils.SingletonUtils<MultiThreadManager>, IDisposable
    {
        private ConcurrentQueue<ThreadTask> mThreadTaskList;

        private LinkedList<ThreadTask> mThreadDoingList;
        private LinkedList<ThreadImpl> mThreadList;
#if EnableAutoResetEvent
        private AutoResetEvent mSemaphoreCounter;
#else
        private SemaphoreSlim mSemaphoreCounter = null;
#endif
        protected readonly Logging.Log sLog;


        private volatile bool mIsDestoryed;

        public int MinThreadSize { get; set; }
        public int MaxThreadSize { get; set; }

        public MultiThreadManager() : this(2, 5)
        {
        }

        protected MultiThreadManager(int minthread, int maxthread)
        {
            sLog = new Logging.Log(this.GetType());

            this.MinThreadSize = Math.Max(minthread, 2);
            this.MaxThreadSize = Math.Max(maxthread, 4);

            mIsDestoryed = false;
#if EnableAutoResetEvent
            mSemaphoreCounter = new AutoResetEvent(false);
#else
            mSemaphoreCounter = new SemaphoreSlim(0, int.MaxValue);
#endif
            mThreadDoingList = new LinkedList<ThreadTask>();
            mThreadTaskList = new ConcurrentQueue<ThreadTask>();
            //mThreadBgTaskList = new ConcurrentQueue<ThreadTask>();
            mThreadList = new LinkedList<ThreadImpl>();
        }

        public bool IsDestoryed()
        {
            return mIsDestoryed;
        }

        public void AddTask(ThreadTask task)
        {
            if (mIsDestoryed)
            {
#if true //DEBUG
                sLog.Info("addTask, destoryed...");
#endif
                //Logging.Logger.LoggerInstance.Write("addTask, destoryed...", Logging.MessageType.Warning);
                return;
            }

            if (task == null)
            {
#if true //DEBUG
                sLog.Info("addTask, ThreadTask can't be null...");
#endif
                //Logging.Logger.LoggerInstance.Write("addTask, ThreadTask can't be null...", Logging.MessageType.Warning);
                throw (new NullReferenceException("addTask, ThreadTask can't be null..."));
            }

            //int workerThreads, availabeThreads;
            //ThreadPool.GetAvailableThreads(out workerThreads, out availabeThreads);
#if DEBUG
            sLog.Info(String.Format("addTask(2), minThreadSize: {0}, maxThreadSize:{1}, currentSize:{2}, tasksize:{3}",
                MinThreadSize, MaxThreadSize,
                mThreadList.Count, mThreadTaskList.Count));
#endif

            doPushTaskImpl(task);

            bool allowCreateThread = false;
            lock (mThreadList)
            {
                if (mThreadList.Count < MinThreadSize)
                {
                    allowCreateThread = true;
                }
                else if (mThreadList.Count < MaxThreadSize)
                {
                    //allowCreateThread = true;
                    int count = 0;
                    LinkedListNode<ThreadImpl> nodeNow = mThreadList.First;
                    while (nodeNow != null)
                    {
                        if (nodeNow.Value.IsIdle())
                        {
                            count++;
                        }

                        nodeNow = nodeNow.Next;
                    }

                    if (count <= 2)
                    {
                        allowCreateThread = true;
                    }
                }
            }
#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif
            if (allowCreateThread && !IsDestoryed())
            {
                ThreadImpl thread = new ThreadImpl(IdleThreadProc/*, task.isBackground()*/);
                thread.doStart();
            }

#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif
            //mSemaphoreCounter.Set();
#if DEBUG
            sLog.Info(String.Format("addTask(end), DoingListSize:{0}, threadsize:{1}, tasksize:{2}",
                mThreadDoingList.Count, mThreadList.Count, mThreadTaskList.Count));
#endif
        }

        protected virtual void doPushTaskImpl(ThreadTask task)
        {
            //lock (mThreadTaskList)
            {

                if (mThreadTaskList.Contains(task))
                {
#if DEBUG
                    sLog.Info(String.Format("doPushTaskImpl(), task: {0}, existing...", task));
#endif
                    return;
                }

                mThreadTaskList.Enqueue(task);
            }
        }       

        public void Release()
        {
#if DEBUG
            sLog.Info(String.Format("release(0), mIsDestoryed:{0}...", mIsDestoryed));
#endif
            if (mIsDestoryed)
            {
#if EnableAutoResetEvent
                mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif
                return;
            }

            mIsDestoryed = true;
#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif

#if DEBUG
            sLog.Info(String.Format("release(1), doingsize:{0}, ThreadList:{1}...", mThreadDoingList.Count, mThreadList.Count));
#endif
            LinkedList<ThreadImpl> list;
            lock (mThreadList)
            {
                list = new LinkedList<ThreadImpl>(mThreadList);
            }

            //foreach (ThreadImpl item in list)
            LinkedListNode<ThreadImpl> nodeNow = list.First;
            while (nodeNow != null)
            {
                nodeNow.Value.Dispose();

#if EnableAutoResetEvent
                mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif

                nodeNow = nodeNow.Next;
            }

#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif

            LinkedList<ThreadTask> tasklist;
            lock (mThreadDoingList)
            {
                tasklist = new LinkedList<ThreadTask>(mThreadDoingList);
            }

            //foreach (ThreadTask item in mThreadDoingList)
            LinkedListNode<ThreadTask> nodeTask = tasklist.First;
            while (nodeTask != null)
            {
                nodeTask.Value.Cancel(true);

#if EnableAutoResetEvent
                mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif

                nodeTask = nodeTask.Next;
            }

#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif

            foreach (ThreadTask item in mThreadTaskList)
            {
                item.Cancel(true);
            }

#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif

#if DEBUG
            sLog.Info(String.Format("release(2), doingsize:{0}, ThreadList:{1}...", mThreadDoingList.Count, mThreadList.Count));
#endif
        }

        public void Join()
        {
            LinkedList<ThreadImpl> list;
            lock (mThreadList)
            {
                list = new LinkedList<ThreadImpl>(mThreadList);
            }
#if DEBUG
            sLog.Info(String.Format("join(0), start size:{0}, mIsDestoryed:{1}...", mThreadList.Count, mIsDestoryed));
#endif
            //Logging.Logger.LoggerInstance.Write(String.Format("join, start size:{0}...", list.Count), Logging.MessageType.Information);

            //foreach (ThreadImpl item in list)
            LinkedListNode<ThreadImpl> nodeNow = list.First;
            while (nodeNow != null)
            {
                ThreadImpl item = nodeNow.Value;

                if (!item.IsDisposed())
                {
#if DEBUG
                    sLog.Info(String.Format("join(1), thread:{0:X8}..threadid:{1:X8}, IsIdle:{2}",
                        item.GetHashCode(), item.ThreadHandle, item.IsIdle()));
#endif
                    try
                    {
                        item.ThreadHandle.Join();
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        sLog.Info(String.Format("join(2), ThreadTask Exception:{0}...", e));
#endif
                        //sLog.Info(String.Format("join, ThreadTask Exception:{0}...", e), Logging.MessageType.Warning);
                    }
                }

                nodeNow = nodeNow.Next;
            }
#if DEBUG
            sLog.Info(String.Format("join(3), end...", mThreadList.Count));
#endif
            //sLog.Info(String.Format("join, end..."), Logging.MessageType.Information);
        }

        public void Dispose()
        {
            Release();
            Join();
        }

        public int GetRunningThreadCount()
        {
            //lock (mThreadList)
            {
                return mThreadList.Count;
            }
        }

        public int GetIdleThreadCount()
        {
            int count = 0;
            LinkedList<ThreadImpl> list;
            lock (mThreadList)
            {
                list = new LinkedList<ThreadImpl>(mThreadList);
            }

            //lock (mThreadList)
            {
                //foreach (ThreadImpl item in mThreadList)
                LinkedListNode<ThreadImpl> nodeNow = list.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.IsIdle())
                    {
                        count++;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            return count;
        }

        public ThreadTask GetThreadTask(string tag)
        {
            LinkedList<ThreadTask> tasklist;
            lock (mThreadDoingList)
            {
                tasklist = new LinkedList<ThreadTask>(mThreadDoingList);
            }

            //lock (mThreadDoingList)
            {
                //foreach (ThreadTask item in mThreadDoingList)
                LinkedListNode<ThreadTask> nodeNow = tasklist.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.GetTag().CompareTo(tag) == 0)
                    {
                        return nodeNow.Value;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            //lock (mThreadTaskList)
            {
                foreach (ThreadTask item in mThreadTaskList)
                {
                    if (item.GetTag().CompareTo(tag) == 0)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        protected virtual bool popupTask(out ThreadTask item)
        {
            //lock (mThreadTaskList)
            {
                return mThreadTaskList.TryDequeue(out item);
            }
        }

        // This thread procedure performs the task.
        protected void IdleThreadProc(Object stateInfo)
        {
            ThreadImpl thread = (ThreadImpl)stateInfo;
            LinkedListNode<ThreadImpl> node = null;
            lock (mThreadList)
                node = mThreadList.AddFirst(thread);

#if DEBUG
            sLog.Info(String.Format(">>>>>>>>>>>>>>IdleThreadProc(0)>>>>>>>id:0x{0:X8}>>>>count:{1}>>> thread:{2} entry...",
                thread.GetHashCode(), mThreadList.Count, thread.ToString()));
#endif

          try
            {
                while (!IsDestoryed() && !thread.IsDisposed())
                {
                    ThreadTask item;
                    if (popupTask(out item))
                    {
                        if (item.IsCanceled())
                        {
#if DEBUG
                            sLog.Info(String.Format("IdleThreadProc(11) do thread proc item canceled: {0}", item));
#endif
                            //Logging.Logger.LoggerInstance.Write(String.Format("IdleThreadProc do thread proc item canceled: {0}", item), Logging.MessageType.Information);
                            continue;
                        }

                        lock (mThreadDoingList)
                            mThreadDoingList.AddFirst(item);

                        thread.Execute(item);

                        lock (mThreadDoingList)
                            mThreadDoingList.Remove(item);

                        continue;
                    }

#if false
                    sLog.Info(String.Format("IdleThreadProc(333) SemaphoreCounter.Wait, thread:0x{0:X00000000}, mIsDestoryed:{1}",
                                        thread.GetHashCode(), mIsDestoryed));
#endif
                    try
                    {
                        if (IsDestoryed()) break;
#if EnableAutoResetEvent
                        mSemaphoreCounter.WaitOne(2000);
#else
                        mSemaphoreCounter.Wait(2000);
#endif

                        lock (mThreadList)
                        {
                            if (mThreadList.Count > MinThreadSize)
                            {
#if DEBUG
                                sLog.Info(String.Format("IdleThreadProc(222) thread count:{0} > min {1}, and reduce this thread:0x{2:X8}...",
                                    mThreadList.Count, MinThreadSize, thread.GetHashCode()));
#endif
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        sLog.Info(String.Format("IdleThreadProc({0:X8}.{1}) SemaphoreCounter.Wait Exception: {2}",
                            thread.GetHashCode(), thread.IsDisposed(), e.ToString()));
#endif
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                sLog.Info(String.Format("IdleThreadProc(333) id:0x{0:X8}<<<<Exception:{1}",
                    thread.GetHashCode(), ex));
#endif
            }
            finally
            {
                lock (mThreadList)
                {
                    mThreadList.Remove(node);
                }

                thread.Dispose();
            }

#if false //DEBUG
            sLog.Info(String.Format("<<<<<<<<<<<<<IdleThreadProc<<<<<<<id:0x{0:X8}<<<<ThreadList:{1}<<<< thread:{2},{3} exit...", 
                thread.GetHashCode(), mThreadList.Count, thread, MinThreadSize));
#endif
        }
    }
}
