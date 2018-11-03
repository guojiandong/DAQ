using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task
{
    /// <summary>
    /// Async task event handler.
    /// </summary>
    public delegate void MonitorEventCallbackHandler(string key, Model.Args.SyncInformationEventArgs args);

    /// <summary>
    /// 
    /// </summary>
    public class SyncInfoMonitorTask : ThreadTask
    {
        public readonly LinkedList<MonitorEventItemInfo> mCacheList;
        private System.Threading.SemaphoreSlim mSemaphoreCounter;


        public SyncInfoMonitorTask()
        {
            mCacheList = new LinkedList<MonitorEventItemInfo>();
            mSemaphoreCounter = new System.Threading.SemaphoreSlim(0, int.MaxValue);
        }

        protected override void onCanceled()
        {
            mSemaphoreCounter.Release();
            base.onCanceled();

            mSemaphoreCounter.Release();
        }

        protected virtual void onTaskStarted(object param)
        {
        }

        protected virtual void onTaskStoped(object param)
        {
        }

        protected override void doProc(object param)
        {
            int delay = 0;
            try
            {
                onTaskStarted(param);
            }
            catch (Exception ex)
            {
                sLog.Warn("doProc(1), call onTaskStarted exception:" + ex.ToString());
            }

            while (!IsCanceled())
            {
                try
                {
                    MonitorEventItemInfo item = null;
                    delay = this.popup(ref item);

                    if (delay == 0)
                    {
                        try
                        {
                            item.DoCallback();
                        }
                        catch (Exception ex)
                        {
                            sLog.Warn("doProc(4), run EventHandler exception:" + ex.ToString());
                        }

                        continue;
                    }

                    if (onProcIdle())
                    {
                        continue;
                    }

                    if (delay < 0)
                        mSemaphoreCounter.Wait(getDefaultThreadTimeout());
                    else if (getDefaultThreadTimeout() < 0)
                        mSemaphoreCounter.Wait(delay);
                    else
                        mSemaphoreCounter.Wait(Math.Min(delay, getDefaultThreadTimeout()));
                }
                catch (Exception ex)
                {
                    sLog.Warn("doProc(4) exception:" + ex.ToString());
                }
            }

            try
            {
                onTaskStoped(param);
            }
            catch (Exception ex)
            {
                sLog.Warn("doProc(6), call onTaskStoped exception:" + ex.ToString());
            }

            sLog.Info("<<<<<<<<<<<<<<<<<<<<<<doProc(8)<<<<<<<<<<<<<<<<<<<<<< ");
        }

        protected virtual bool onProcIdle()
        {
            return false;
        }

        /// <summary>
        /// 重写线程空闲等待时间，默认一直等待
        /// </summary>
        /// <returns></returns>
        protected virtual int getDefaultThreadTimeout()
        {
            return 2000;
        }

        public void Push(string key, MonitorEventCallbackHandler callback, Model.Args.SyncInformationEventArgs value)
        {
            if (IsCanceled() || String.IsNullOrEmpty(key))
            {
                sLog.Warn("Push() failed, because this Disposed");
                return;
            }

            MonitorEventItemInfo child = new MonitorEventItemInfo(key, callback, value);
            lock (mCacheList)
            {
                // 去重
                LinkedListNode<MonitorEventItemInfo> nodeNow = mCacheList.First;
                if(value.Timeout < 0)
                {
                    nodeNow = null;
                }
                else
                {
                    nodeNow = mCacheList.First;
                    while (nodeNow != null)
                    {
                        if (nodeNow.Value.TimeOutTicks < 0 || nodeNow.Value.TimeOutTicks > child.TimeOutTicks)
                        {
                            mCacheList.AddBefore(nodeNow, child);
                            break;
                        }
                        nodeNow = nodeNow.Next;
                    }
                }
                
                if (nodeNow == null)
                    mCacheList.AddLast(child);
            }

            mSemaphoreCounter.Release();
        }

        public int SetResult(string key, object value)
        {
            int timeout = -1;

            lock (mCacheList)
            {
                LinkedListNode<MonitorEventItemInfo> nodeNow = mCacheList.First;
                while (nodeNow != null)
                {
                    if (key == nodeNow.Value.Key)
                    {
                        mCacheList.Remove(nodeNow);

                        timeout = nodeNow.Value.SetResult(value);

                        mCacheList.AddFirst(nodeNow);

                        mSemaphoreCounter.Release();
                        return timeout;
                    }
                    nodeNow = nodeNow.Next;
                }
            }

            return timeout;
        }

        //public object Remove(string key)
        //{
        //    lock (mCacheList)
        //    {
        //        LinkedListNode<MonitorEventItemInfo> nodeNow = mCacheList.First;
        //        while (nodeNow != null)
        //        {
        //            if (key == nodeNow.Value.Key)
        //            {
        //                object value = nodeNow.Value.Value;
        //                mCacheList.Remove(nodeNow);

        //                nodeNow.Value.DoCallback(false);
        //                return value;
        //            }
        //            nodeNow = nodeNow.Next;
        //        }
        //    }

        //    return null;
        //}

        private int popup(ref MonitorEventItemInfo item)
        {
            MonitorEventItemInfo child = null;
            lock (mCacheList)
            {
                if (mCacheList.First != null)
                {
                    child = mCacheList.First.Value;

                    if (child != null)
                    {
                        if (child.TimeOutTicks < 0)
                            return -1;

                        long timeSpace = child.TimeOutTicks - DateTime.UtcNow.Ticks;
                        if (IsCanceled() || timeSpace <= TimeSpan.TicksPerMillisecond)
                        {
                            mCacheList.RemoveFirst();

                            item = child;
                            return 0;
                        }

                        try
                        {
                            return Convert.ToInt32(timeSpace / TimeSpan.TicksPerMillisecond);
                        }
                        catch { }
                    }
                    else
                        mCacheList.RemoveFirst();

                    return 100;
                }
            }

            return -1;
        }

        public class MonitorEventItemInfo
        {
            #region Members

            public long TimeOutTicks { get; private set; }


            public readonly Model.Args.SyncInformationEventArgs Value;

            public readonly MonitorEventCallbackHandler EventHandler;

            public readonly string Key;
            #endregion

            public MonitorEventItemInfo(string key, MonitorEventCallbackHandler handler)
                : this(key, handler, null)
            {
            }

            public MonitorEventItemInfo(string key, MonitorEventCallbackHandler handler, Model.Args.SyncInformationEventArgs value)
            {
                if(value != null && value.Timeout > 0)
                    this.TimeOutTicks = DateTime.UtcNow.Ticks + (value.Timeout+100) * TimeSpan.TicksPerMillisecond;
                else
                    this.TimeOutTicks = -1;

                this.Key = key;
                this.Value = value;
                //this.TimeOut = timeout;
                this.EventHandler += handler;
            }

            public void DoCallback()
            {
                this.EventHandler(this.Key, this.Value);
            }

            public int SetResult(object value)
            {
                this.Value.SetResult(value);
                this.TimeOutTicks = 1;

                return this.Value.TimeInterval;
            }

            public override bool Equals(object obj)
            {
                if (obj is MonitorEventItemInfo)
                {
                    return this.Key.Equals(((MonitorEventItemInfo)obj).Key);
                }

                return base.Equals(obj);
            }
        }
    }
}
