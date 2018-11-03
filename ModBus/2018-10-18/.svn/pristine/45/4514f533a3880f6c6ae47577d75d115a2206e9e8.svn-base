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
    [Guid("C7494D9D-28FE-4E0E-B9FA-D113D1578345")]
    public abstract class ThreadPoolsManager<TData> : IDisposable where TData : class
    {
        private readonly Threading.Queue.QueueManager<TData> mCacheList;

        //private ConcurrentQueue<TData> mCacheList;
        private LinkedList<Thread> mThreadList;

        //private SemaphoreSlim mSemaphoreCounter = null;

        private volatile bool mIsDestoryed;
        private volatile int mIdleThreadCount = 0;

        public int MinThreadSize { get; set; }
        public int MaxThreadSize { get; set; }
        public ThreadPriority Priority { get; set; }

        public int DelayTime { get; set; }

        protected readonly Logging.Log sLog;

        public ThreadPoolsManager(int minthread = 2, int maxthread = 5)
        {
            sLog = new Logging.Log(this.GetType());

            this.MinThreadSize = Math.Max(minthread, 1);
            this.MaxThreadSize = Math.Max(maxthread, 1);
            this.Priority = ThreadPriority.Normal;
            mIsDestoryed = false;

            mCacheList = new Queue.QueueManager<TData>();

            //mSemaphoreCounter = new SemaphoreSlim(0, int.MaxValue);
            this.DelayTime = 2000;
            //mCacheList = new ConcurrentQueue<TData>();
            mThreadList = new LinkedList<Thread>();
        }

        public void Dispose()
        {
            Release();

            Join();
        }

        public void Cancel()
        {
            this.Release();
        }

        public void Release()
        {
            mIsDestoryed = true;
            //mSemaphoreCounter.Release();

            mCacheList.Dispose();

            LinkedList<Thread> list;
            lock (mThreadList)
            {
                list = new LinkedList<Thread>(mThreadList);
            }

            //foreach (ThreadImpl item in list)
            LinkedListNode<Thread> nodeNow = list.First;
            while (nodeNow != null)
            {
                Thread item = nodeNow.Value;
                try
                {
                    //mSemaphoreCounter.Release();
                    mCacheList.SetSignal();
                    item.Interrupt();
                }
                catch (Exception e)
                {
#if DEBUG
                    sLog.Error("Release(2), ThreadTask Exception:", e);
#endif
                }

                nodeNow = nodeNow.Next;
            }

        }

        public virtual void Join()
        {
            LinkedList<Thread> list;
            lock (mThreadList)
            {
                list = new LinkedList<Thread>(mThreadList);
            }

            //foreach (ThreadImpl item in list)
            LinkedListNode<Thread> nodeNow = list.First;
            while (nodeNow != null)
            {
                Thread item = nodeNow.Value;
                try
                {
                    mCacheList.SetSignal();
                    item.Join();
                }
                catch (Exception e)
                {
#if DEBUG
                    sLog.Info(String.Format("join(2), ThreadTask Exception:{0}...", e));
#endif
                }

                nodeNow = nodeNow.Next;
            }
#if DEBUG
            sLog.Info(String.Format("join(3), end, count:{0}", mThreadList.Count));
#endif
        }

        public bool IsDestoryed()
        {
            return mIsDestoryed;
        }

        public bool IsCanceled()
        {
            return mIsDestoryed;
        }

        public void Push(TData data, int delay = 0)
        {
            if (mIsDestoryed)
            {
                return;
            }

            mCacheList.Push(data, delay);

            bool allowCreateThread = false;
            lock (mThreadList)
            {
                if (mCacheList.GetCount() > 0 
                    && (mThreadList.Count < MinThreadSize 
                    || (mThreadList.Count <= MaxThreadSize && (mIdleThreadCount < 2 || mCacheList.GetCount() > 1) )))
                {
                    allowCreateThread = true;
                }
            }

            if (allowCreateThread)
            {
                Thread thread = new Thread(IdleThreadProc);
                thread.Priority = this.Priority;
                thread.IsBackground = true;
                thread.Start(thread);
            }
#if DEBUG
            else
            {
                sLog.Info("ThreadPoolsManager::Push(3) no create thread, ThreadList.Count:" + mThreadList.Count
                    + ", IdleThreadCount:"+ mIdleThreadCount
                    + ", MinThreadSize:" + MinThreadSize
                    + ", MaxThreadSize:" + MaxThreadSize);
            }
#endif
        }

        protected abstract void OnDataProc(TData data);




        protected void IdleThreadProc(Object stateInfo)
        {
            int delay = 0, idletimes = 0;
            Thread thread = (Thread)stateInfo;
            LinkedListNode<Thread> node = null;
            lock (mThreadList)
                node = mThreadList.AddFirst(thread);

            try
            {
                while (!IsDestoryed())
                {
                    TData item = null;
                    delay = mCacheList.PopupWithDelayTime(ref item);
                    if (delay == 0)
                    {
                        try
                        {
                            OnDataProc(item);
                        }
                        catch (Exception e)
                        {
#if true //DEBUG
                            sLog.Warn(String.Format("IdleThreadProc({0:X8}) Do User Proc Exception: {1}",
                                thread.GetHashCode(), e.ToString()));
#endif
                        }

                        idletimes = 0;

                        continue;
                    }

                    mIdleThreadCount++;

                    try
                    {
                        if (delay < 0)
                        {
                            idletimes++;
                            mCacheList.Wait(Math.Max(10, this.DelayTime));
                        }
                        else if (this.DelayTime < 0)
                        {
                            mCacheList.Wait(delay);
                        }
                        else
                        {
                            delay = Math.Min(delay, this.DelayTime);
                            mCacheList.Wait(delay);
                        }
                    }
                    catch (Exception e)
                    {
#if true //DEBUG
                        sLog.Warn(String.Format("IdleThreadProc({0:X8}) Wait for delay:"+delay+" Exception: {1}",
                            thread.GetHashCode(), e.ToString()));
#endif
                    }

                    mIdleThreadCount--;

                    if (idletimes > 3 && this.MaxThreadSize > this.MinThreadSize)
                    {
                        lock (mThreadList)
                        {
                            if (mThreadList.Count > MinThreadSize && mCacheList.Count == 0)
                            {
                                mThreadList.Remove(node);
                                node = null;
#if DEBUG
                                sLog.Info(String.Format("IdleThreadProc(222) thread count:{0} > min {1}, IdleThreadCount:{2} and reduce this thread:0x{3:X8}...",
                                    mThreadList.Count, MinThreadSize, mIdleThreadCount, thread.GetHashCode()));
#endif
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                sLog.Warn(String.Format("IdleThreadProc(333) id:0x{0:X8}<<<<Exception:{1}",
                    thread.GetHashCode(), ex));
#endif
            }

            lock (mThreadList)
            {
                if(node != null)
                    mThreadList.Remove(node);
            }
        }

        protected virtual void onTimeOutHandle()
        {

        }
    }
}
