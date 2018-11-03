using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Queue
{
    [Guid("362D0066-4552-459D-981A-D5740BF3351A")]
    public class QueueManager<TChild> : IDisposable
    {
        private const bool CONST_SUPPORT_QUEUE = false;

        private LinkedList<ChildItemWithDelay> mDelayItemList;
        private Queue<ChildItemWithDelay> mReqItemList;
#if EnableAutoResetEvent
        private AutoResetEvent mSemaphoreCounter;
#else
        private SemaphoreSlim mSemaphoreCounter;
#endif
        private volatile bool mIsDisposed;
        private int mMaxCount;
        public int MaxDelayTime { get; set; }
        protected readonly Logging.Log sLog;
        public QueueManager() : this(4096)
        {
        }

        public QueueManager(int max)
        {
            mMaxCount = Math.Max(1, max);

            this.MaxDelayTime = 3000;
            mIsDisposed = false;
#if true
            if(CONST_SUPPORT_QUEUE)
                mReqItemList = new Queue<ChildItemWithDelay>();
#endif
            mDelayItemList = new LinkedList<ChildItemWithDelay>();
#if EnableAutoResetEvent
            mSemaphoreCounter = new AutoResetEvent(false);
#else
            mSemaphoreCounter = new SemaphoreSlim(0, int.MaxValue);
#endif
        }

        public void Dispose()
        {
            mIsDisposed = true;
            //throw new NotImplementedException();
#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif
        }

        public void Clear()
        {
            if (CONST_SUPPORT_QUEUE && mReqItemList != null)
            {
                lock (mReqItemList)
                {
                    mReqItemList.Clear();
                }
            }

            lock (mDelayItemList)
            {
                mDelayItemList.Clear();
            }

#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif
        }

        public bool IsDispose()
        {
            return mIsDisposed;
        }

        public void SetSignal()
        {
#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif
        }

        public void Wait()
        {
            if (mIsDisposed) return;

            this.Wait(-1);
        }

        public bool Wait(int delay_time)
        {
            if (mIsDisposed) return false;

#if EnableAutoResetEvent
            if (delay_time > 0)
                mSemaphoreCounter.WaitOne(delay_time);
            else if (delay_time < 0)
                mSemaphoreCounter.WaitOne(Math.Max(1000, this.MaxDelayTime));
            //else
            //    mSemaphoreCounter.WaitOne(1);
#else
            if (delay_time > 0)
                return mSemaphoreCounter.Wait(delay_time);
            else if(delay_time < 0)
                mSemaphoreCounter.Wait();
#endif

            return false;
        }

        public int Count {
            get { return this.GetCount(); }
        }

        public int GetCount()
        {
            int count = 0;
            if (CONST_SUPPORT_QUEUE && mReqItemList != null)
            {
                lock (mReqItemList)
                {
                    count += mReqItemList.Count;
                }
            }

            lock (mDelayItemList)
            {
                count += mDelayItemList.Count;
            }

            return count;
        }

        public bool CheckExist(TChild item)
        {
            lock (mDelayItemList)
            {
                LinkedListNode<ChildItemWithDelay> nodeNow = mDelayItemList.First;
                while (nodeNow != null)
                {
                    if (item.Equals(nodeNow.Value.Value))
                    {
                        return true;
                    }
                    nodeNow = nodeNow.Next;
                }
            }

            return false;
        }

        public void Push(TChild item)
        {
            Push(item, 0);
        }

        public void Push(TChild item, int delay_time)
        {
            if (IsDispose())
            {
                Logger.Warn(this.GetType().Name, "Push() failed, because this Disposed");
                return;
            }

#region support queue
            if(CONST_SUPPORT_QUEUE && mReqItemList != null)
            {
                lock (mReqItemList)
                {
                    mReqItemList.Enqueue(new ChildItemWithDelay(item, delay_time));

                    if (mReqItemList.Count > mMaxCount)
                    {
                        mReqItemList.Dequeue();
                    }
                }
            }
#endregion
            else
            {
                ChildItemWithDelay child = new ChildItemWithDelay(item, delay_time);
                lock (mDelayItemList)
                {
                    LinkedListNode<ChildItemWithDelay> nodeNow = mDelayItemList.First;
                    while (nodeNow != null)
                    {
                        if(nodeNow.Value.StartTime > child.StartTime)
                        {
                            mDelayItemList.AddBefore(nodeNow, child);
                            break;
                        }
                        nodeNow = nodeNow.Next;
                    }

                    if (nodeNow == null)
                        mDelayItemList.AddLast(child);
                }
            }
#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif
        }

        public bool Popup(ref TChild item)
        {
            ChildItemWithDelay child = null;
#region support queue
            if (CONST_SUPPORT_QUEUE && mReqItemList != null)
            {
                lock (mReqItemList)
                {
                    if (mReqItemList.Count > 0)
                    {
                        child = mReqItemList.Dequeue();
                        item = child.Value;
                        return true;
                    }
                }
            }
#endregion
            else
            {
                lock (mDelayItemList)
                {
                    if(mDelayItemList.First != null)
                    {
                        child = mDelayItemList.First.Value;
                        mDelayItemList.RemoveFirst();
                        item = child.Value;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>delay time, 0 found item, -1 no found any data, > 0 for delay time...</returns>
        public int PopupWithDelayTime(ref TChild item)
        {
            ChildItemWithDelay child = null;
#region support queue
            if (CONST_SUPPORT_QUEUE && mReqItemList != null)
            {
                lock (mReqItemList)
                {
                    if (mReqItemList.Count > 0)
                    {
                        child = mReqItemList.Dequeue();
                    }
                }

                if (child != null)
                {
                    if (child.StartTime > DateTime.UtcNow.Ticks)
                    {
                        lock (mDelayItemList)
                        {
                            if (mDelayItemList.Count > 0)
                            {
                                LinkedListNode<ChildItemWithDelay> nodeNow = mDelayItemList.First;
                                LinkedListNode<ChildItemWithDelay> nodeLast = mDelayItemList.Last;

                                while (nodeNow != nodeLast)
                                {
                                    if (nodeNow.Value.StartTime > child.StartTime)
                                    {
                                        mDelayItemList.AddBefore(nodeNow, child);
                                        break;
                                    }

                                    nodeNow = nodeNow.Next;
                                }

                                if (nodeNow == nodeLast)
                                {
                                    if (nodeNow.Value.StartTime > child.StartTime)
                                    {
                                        mDelayItemList.AddBefore(nodeNow, child);
                                    }
                                    else
                                    {
                                        mDelayItemList.AddLast(child);
                                    }
                                }
                            }
                            else
                            {
                                mDelayItemList.AddLast(child);
                            }
                        }
                    }
                    else
                    {
                        item = child.Value;
                        return 0;
                    }
                }
            }
#endregion
            lock (mDelayItemList)
            {
                if (mDelayItemList.First != null)
                {
                    child = mDelayItemList.First.Value;

                    if (child != null)
                    {
                        long timeSpace = child.StartTime - DateTime.UtcNow.Ticks;
                        if (IsDispose() || mDelayItemList.Count >= mMaxCount || timeSpace <= TimeSpan.TicksPerMillisecond)
                        {
                            mDelayItemList.RemoveFirst();

                            item = child.Value;
                            return 0;
                        }

                        try
                        {
                            return Convert.ToInt32(timeSpace / TimeSpan.TicksPerMillisecond);
                        }
                        catch { }
                    }
                    else
                        mDelayItemList.RemoveFirst();

                    return 10;
                }
            }

            return -1;
        }



        public int Remove(TChild item)
        {
            int count = 0;
            lock (mDelayItemList)
            {
                LinkedListNode<ChildItemWithDelay> nodeNow = mDelayItemList.First;
                while (nodeNow != null)
                {
                    if (item.Equals(nodeNow.Value.Value))
                    {
                        LinkedListNode<ChildItemWithDelay> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;

                        mDelayItemList.Remove(nodeDel);

                        count++;

                        continue;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            return count;
        }


        private class ChildItemWithDelay
        {
            public readonly long StartTime;
            public readonly TChild Value;

            public ChildItemWithDelay(TChild item) : this(item, 0)
            {

            }

            public ChildItemWithDelay(TChild item, int delay_time)
            {
                StartTime = DateTime.UtcNow.Ticks;

                if (delay_time > 0)
                    StartTime += (long)delay_time * TimeSpan.TicksPerMillisecond;

                this.Value = item;
            }
        }
    }
}
