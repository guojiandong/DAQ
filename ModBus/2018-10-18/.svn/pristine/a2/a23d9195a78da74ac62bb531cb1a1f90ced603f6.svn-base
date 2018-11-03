using System;
namespace Ksat.AppPlugIn.Threading.Task
{

    /// <summary>
    /// Async task event handler.
    /// </summary>
    public delegate void AsyncTaskEventHandler(object sender, object args);

    /// <summary>
    /// Queue async task.
    /// </summary>
    public class QueueAsyncTask : ThreadTask
    {
        #region Members
        private readonly Threading.Queue.QueueManager<AsyncTaskDelegateInfo> mCacheList;
        //protected readonly Logging.Log sLog;
        #endregion

        #region Constructor
        public QueueAsyncTask()
        {
            mCacheList = new Queue.QueueManager<AsyncTaskDelegateInfo>();
        }
        #endregion

        protected override void onCanceled()
        {
            mCacheList.Dispose();

            base.onCanceled();
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
                    AsyncTaskDelegateInfo item = null;

                    lock(mCacheList)
                        delay = mCacheList.PopupWithDelayTime(ref item);

                    if (delay == 0)
                    {
                        try
                        {
                            item.DoCallback(this);
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
                        mCacheList.Wait(getDefaultThreadTimeout());
                    else if (getDefaultThreadTimeout() < 0)
                        mCacheList.Wait(delay);
                    else
                        mCacheList.Wait(Math.Min(delay, getDefaultThreadTimeout()));
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


        public void AddTask(AsyncTaskEventHandler task)
        {
            AddTask(task, null);
        }

        public void AddTask(AsyncTaskEventHandler task, object param)
        {
            AddTask(task, param, 0);
        }

        public void AddTaskDelay(AsyncTaskEventHandler task, int delay)
        {
            AddTask(task, null, delay);
        }

        /// <summary>
        /// 带延时执行的功能
        /// </summary>
        /// <param name="task">委托运行的函数</param>
        /// <param name="param"></param>
        /// <param name="delay">毫秒</param>
        public void AddTask(AsyncTaskEventHandler task, object param, int delay)
        {
            if (IsCanceled()) return;

            if (task == null)
                throw new ArgumentNullException("AsyncTaskEventHandler can't null...");

            lock (mCacheList)
                mCacheList.Push(new AsyncTaskDelegateInfo(task, param), delay);
        }

        public int RemoveTask(AsyncTaskEventHandler task)
        {
            if (IsCanceled()) return 0;

            if (task == null)
                throw new ArgumentNullException("AsyncTaskEventHandler can't null...");

            lock (mCacheList)
                return mCacheList.Remove(new AsyncTaskDelegateInfo(task));
        }


        public class AsyncTaskDelegateInfo
        {
            #region Members
            public readonly object Value;
            public readonly AsyncTaskEventHandler TaskHandler;
            #endregion

            public AsyncTaskDelegateInfo(AsyncTaskEventHandler handler)
                : this(handler, null)
            {
            }

            public AsyncTaskDelegateInfo(AsyncTaskEventHandler handler, object param)
            {
                this.Value = param;
                this.TaskHandler += handler;
            }

            public void DoCallback(object sender){
                this.TaskHandler(sender, this.Value);
            }

            public override bool Equals(object obj)
            {
                if(obj is AsyncTaskDelegateInfo)
                {
                    return this.TaskHandler.Equals(((AsyncTaskDelegateInfo)obj).TaskHandler);
                }

                return base.Equals(obj);
            }
        }
    }
}
