using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading
{
    [Guid("B53CD954-C98F-4A67-8360-AAD4D4A75328")]
    public abstract class ThreadPoolsManagerWithFilter<TKey, TData> : IDisposable where TData : class
    {
        private readonly LinkedList<ThreadInfo> mThreadList;

        private volatile bool mIsDestoryed;

        public int MinThreadSize { get; set; }
        public int MaxThreadSize { get; set; }
        public ThreadPriority Priority { get; set; }

        public int DelayTime { get; set; }

        protected readonly Logging.Log sLog;

        public ThreadPoolsManagerWithFilter(int minthread = 0, int maxthread = 15)
        {
            sLog = new Logging.Log(this.GetType());

            this.MinThreadSize = Math.Max(minthread, 0);
            this.MaxThreadSize = Math.Max(maxthread, 1);
            this.Priority = ThreadPriority.Normal;
            mIsDestoryed = false;

            this.DelayTime = 2000;

            mThreadList = new LinkedList<ThreadInfo>();
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

            //mCacheList.Dispose();

            LinkedList<ThreadInfo> list;
            lock (mThreadList)
            {
                list = new LinkedList<ThreadInfo>(mThreadList);
            }

            //foreach (ThreadImpl item in list)
            LinkedListNode<ThreadInfo> nodeNow = list.First;
            while (nodeNow != null)
            {
                ThreadInfo item = nodeNow.Value;
                try
                {
                    //mSemaphoreCounter.Release();
                    item.CacheList.SetSignal();
                    item.ThreadProc.Interrupt();
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
            LinkedList<ThreadInfo> list;
            lock (mThreadList)
            {
                list = new LinkedList<ThreadInfo>(mThreadList);
            }

            //foreach (ThreadImpl item in list)
            LinkedListNode<ThreadInfo> nodeNow = list.First;
            while (nodeNow != null)
            {
                ThreadInfo item = nodeNow.Value;
                try
                {
                    item.CacheList.SetSignal();
                    item.ThreadProc.Join();
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

        public void Push(TKey key, TData data, int delay = 0)
        {
            if (mIsDestoryed)
            {
                return;
            }

            lock (mThreadList)
            {
                LinkedListNode<ThreadInfo> nodeNow = mThreadList.First;
                while (nodeNow != null)
                {
                    if (key.Equals(nodeNow.Value.Key))
                    {
                        nodeNow.Value.CacheList.Push(data, delay);
                        return;
                    }
                    nodeNow = nodeNow.Next;
                }
#if DEBUG
                if(mThreadList.Count > MaxThreadSize)
                {
                    throw new ArgumentOutOfRangeException("Current thread count larger than MaxThreadSize:"+ MaxThreadSize);
                }
#endif
                Thread thread = new Thread(IdleThreadProc);
                thread.Priority = this.Priority;
                thread.IsBackground = true;

                ThreadInfo info = new ThreadInfo(key, thread);
                info.CacheList.Push(data, delay);

                thread.Start(info);
            }
        }

        protected abstract void OnDataProc(TKey tag, TData data);

        protected void IdleThreadProc(Object stateInfo)
        {
            int delay = 0, idletimes = 0;
            ThreadInfo thread = (ThreadInfo)stateInfo;
            LinkedListNode<ThreadInfo> node = null;
            lock (mThreadList)
                node = mThreadList.AddFirst(thread);

            try
            {
                while (!IsDestoryed())
                {
                    TData item = null;
                    delay = thread.CacheList.PopupWithDelayTime(ref item);
                    if (delay == 0)
                    {
                        try
                        {
                            OnDataProc(thread.Key, item);
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

                    try
                    {
                        if (delay < 0)
                        {
                            idletimes++;
                            thread.CacheList.Wait(Math.Max(10, this.DelayTime));
                        }
                        else if (this.DelayTime < 0)
                        {
                            thread.CacheList.Wait(delay);
                        }
                        else
                        {
                            delay = Math.Min(delay, this.DelayTime);
                            thread.CacheList.Wait(delay);
                        }
                    }
                    catch (Exception e)
                    {
#if true //DEBUG
                        sLog.Warn(String.Format("IdleThreadProc({0:X8}) Wait for delay:" + delay + " Exception: {1}",
                            thread.GetHashCode(), e.ToString()));
#endif
                    }

                    if (idletimes > 3)
                    {
                        lock (mThreadList)
                        {
                            if (thread.CacheList.Count == 0)
                            {
                                mThreadList.Remove(node);
                                node = null;
#if DEBUG
                                sLog.Info(String.Format("IdleThreadProc(222) thread count:{0} > min {1}, and reduce this thread:0x{2:X8}...",
                                    mThreadList.Count, MinThreadSize, thread.GetHashCode()));
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

            try
            {
                lock (mThreadList)
                {
                    thread.CacheList.Dispose();

                    if (node != null)
                        mThreadList.Remove(node);
                }
            }
            catch { }

        }

        protected virtual void onTimeOutHandle()
        {

        }

        private class ThreadInfo
        {
            public readonly Thread ThreadProc;

            public readonly TKey Key;

            public readonly Queue.QueueManager<TData> CacheList;

            public ThreadInfo(TKey key, Thread t)
            {
                this.Key = key;
                this.ThreadProc = t;

                this.CacheList = new Queue.QueueManager<TData>();
            }
        }
    }
}
