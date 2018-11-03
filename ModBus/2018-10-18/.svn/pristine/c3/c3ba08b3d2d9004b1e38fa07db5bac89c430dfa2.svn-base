
using Ksat.AppPlugIn.Model.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Cmd
{
    [Guid("5F240C70-5723-48A9-945B-90017E9CBBC0")]
    public class CommandQueueManager : IDisposable
    {
        #region Member
        private LinkedList<ChildItemWithDelayEventArgs> mDelayItemList;
#if EnableAutoResetEvent
        private AutoResetEvent mSemaphoreCounter;
#else
        private SemaphoreSlim mSemaphoreCounter;
#endif
        private volatile bool mIsDisposed;
        private int mMaxCount;
        public int MaxDelayTime { get; set; }
        #endregion

        public CommandQueueManager() : this(int.MaxValue)
        {
        }

        public CommandQueueManager(int max)
        {
            mMaxCount = Math.Max(2, max);
            this.MaxDelayTime = 3000;

            mIsDisposed = false;
            mDelayItemList = new LinkedList<ChildItemWithDelayEventArgs>();
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
            onDisposed();
        }

        protected virtual void onDisposed()
        {
        }

        public void Clear()
        {
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
#else
            if (delay_time > 0)
                return mSemaphoreCounter.Wait(delay_time);
            else if(delay_time < 0)
                mSemaphoreCounter.Wait();
#endif
            return true;
        }

        public int GetCount()
        {
            lock (mDelayItemList)
            {
                return mDelayItemList.Count;
            }
        }

        public bool CheckExist(AbstractCommandEventArgs item)
        {
            lock (mDelayItemList)
            {
                LinkedListNode<ChildItemWithDelayEventArgs> nodeNow = mDelayItemList.First;
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

        public void Push(AbstractCommandEventArgs item)
        {
            Push(item, 0);
        }

        public void Push(AbstractCommandEventArgs item, int delay_time)
        {
            if (mIsDisposed) return;

            ChildItemWithDelayEventArgs child = new ChildItemWithDelayEventArgs(item, delay_time);
            lock (mDelayItemList)
            {
                LinkedListNode<ChildItemWithDelayEventArgs> nodeNow = mDelayItemList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.StartTime > child.StartTime)
                    {
                        mDelayItemList.AddBefore(nodeNow, child);
                        break;
                    }
                    nodeNow = nodeNow.Next;
                }

                if (nodeNow == null)
                    mDelayItemList.AddLast(child);
            }
#if EnableAutoResetEvent
            mSemaphoreCounter.Set();
#else
            mSemaphoreCounter.Release();
#endif
        }

        public bool Popup(ref AbstractCommandEventArgs item)
        {
            ChildItemWithDelayEventArgs child = null;

            lock (mDelayItemList)
            {
                if (mDelayItemList.First != null)
                {
                    child = mDelayItemList.First.Value;
                    mDelayItemList.RemoveFirst();
                    item = child.Value;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>delay time, 0 found item, -1 no found any data, > 0 for delay time...</returns>
        public int PopupWithDelayTime(ref AbstractCommandEventArgs item)
        {
            ChildItemWithDelayEventArgs child = null;

            lock (mDelayItemList)
            {
                if (mDelayItemList.First != null)
                {
                    child = mDelayItemList.First.Value;
                    if(this.IsDispose() || mDelayItemList.Count > mMaxCount)
                    {
                        mDelayItemList.RemoveFirst();
                        item = child.Value;
                        return 0;
                    }

                    long timeSpace = child.StartTime - DateTime.UtcNow.Ticks;
                    if (timeSpace <= TimeSpan.TicksPerMillisecond)
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

                    return 10;
                }
            }

            return -1;
        }


        private class ChildItemWithDelayEventArgs : GenericEventArgs<AbstractCommandEventArgs>
        {
            public readonly long StartTime;

            public ChildItemWithDelayEventArgs(AbstractCommandEventArgs item) : this(item, 0)
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="item"></param>
            /// <param name="delay_time">Millisecond</param>
            public ChildItemWithDelayEventArgs(AbstractCommandEventArgs item, int delay_time)
                : base(item)
            {
                StartTime = DateTime.UtcNow.Ticks;

                if (delay_time > 0)
                    StartTime += (long)delay_time * TimeSpan.TicksPerMillisecond;
            }
        }
    }
}
