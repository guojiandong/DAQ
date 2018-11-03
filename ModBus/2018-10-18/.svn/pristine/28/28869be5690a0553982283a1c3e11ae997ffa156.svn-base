using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading
{
    [Guid("177F03F7-D5F1-45D2-9374-B2DBFAE63E87")]
    public enum ThreadTaskStatus
    {
        Idle,
        Running,
        Finished
    }

    [Guid("DD0EE5A0-1450-499D-838A-610B0FCADD1D")]
    public interface IThreadTaskStatusChangedListener
    {
        void onTaskStatusChangedListener(ThreadTask task, ThreadTaskStatus status);
    }


    [Guid("CDC2301D-6AE5-4326-A5DF-A43D4E97A7E8")]
    public abstract class ThreadTask
    {
        private AutoResetEvent mWaitingExitEvent = null;
        protected readonly Logging.Log sLog;

        #region Constructor
        public ThreadTask()
            : this(null)
        {

        }

        public ThreadTask(object obj)
        {
            Priority = ThreadPriority.Normal;

            sLog = new Logging.Log(this.GetType());

            mIsCanceled = false;
            this.param = obj;
            mThreadTaskStatus = ThreadTaskStatus.Idle;
            mStatusChangedListener = new LinkedList<IThreadTaskStatusChangedListener>();

            mWaitingExitEvent = new AutoResetEvent(false);
        }
        #endregion

        public virtual string GetTag()
        {
            if (String.IsNullOrEmpty(mTag))
                return GetType().FullName;

            return mTag;
        }

        public void SetTag(string tag)
        {
            mTag = tag;
        }

        public ThreadTaskStatus GetStatus()
        {
            return mThreadTaskStatus;
        }

        protected abstract void doProc(object param);


        private Thread mCurrentThread;
        internal void Execute(ThreadImpl thread)
        {
            lock (mLocker)
            {
                mCurrentThread = thread.ThreadHandle;
            }

            if (mWaitingExitEvent != null)
            {
                mWaitingExitEvent.Reset();
            }
#if DEBUG
            sLog.Info(String.Format(">>>>>>>>>>Task(0) {0} start execute by thread:0x{1:X00000000}, IsCanceled:{2}",
                GetTag(), thread.GetHashCode(), mIsCanceled));
#endif
            lock (mStatusChangedListener)
            {
                if (!mThreadTaskStatus.Equals(ThreadTaskStatus.Idle))
                {
#if DEBUG
                    sLog.Info(String.Format("Task(1) {0} status:{1}, can't run again...", GetTag(), mThreadTaskStatus));
#endif
                    throw new ObjectDisposedException(String.Format("Task({0}) status:{1}, task has disposed, can't run again...",
                        GetTag(), mThreadTaskStatus));
                }

                mThreadTaskStatus = ThreadTaskStatus.Running;
            }

            notifyTaskStatusChangedListener(mThreadTaskStatus);

            if (!mIsCanceled)
            {
                try
                {
                    doProc(this.param);
                }
                catch (System.Exception ex)
                {
#if true//DEBUG
                    sLog.Warn(String.Format("Task(2) {0}--0x{1:X00000000} do proc catch error: {2}",
                        GetTag(), thread.GetHashCode(), ex));
#endif
                }

                notifyFinished();
            }

#if DEBUG
            sLog.Info(String.Format("Task(3) {0} finished from thread:0x{1:X00000000}", GetTag(), thread.GetHashCode()));
#endif
            lock (mLocker)
            {
                mCurrentThread = null;
            }

            mThreadTaskStatus = ThreadTaskStatus.Finished;

            notifyTaskStatusChangedListener(mThreadTaskStatus);

            AutoResetEvent waitingevent = null;
            lock (mLocker)
            {
                waitingevent = mWaitingExitEvent;
                mWaitingExitEvent = null;
            }

            if (waitingevent != null)
            {
                waitingevent.Set();
            }
#if DEBUG
            sLog.Info(String.Format("<<<<Task() {0} released from thread:0x{1:X00000000}<<<<<<<<<<<", GetTag(), thread.GetHashCode()));
#endif
        }

        public bool IsIdle()
        {
            lock (mLocker)
            {
                if (mCurrentThread == null)
                {
                    return true;
                }
            }

            return false;
        }
        
        public void Join()
        {
            Join(-1);
        }

        public virtual void Join(int timeout)
        {
            try
            {
                AutoResetEvent waitingevent = null;
                lock (mLocker)
                {
                    waitingevent = mWaitingExitEvent;
                }

                if (waitingevent != null)
                {
                    if (timeout >= 0)
                        waitingevent.WaitOne(timeout);
                    else
                        waitingevent.WaitOne();
                }
            }
            catch (Exception ex)
            {
                sLog.Warn("Join(), wait for exit exception:"+ex.ToString());
            }
        }

        internal virtual void notifyFinished()
        {
            onFinished();
        }

        internal virtual void notifyTaskStatusChangedListener(ThreadTaskStatus status)
        {
            LinkedList<IThreadTaskStatusChangedListener> list;
            lock (mStatusChangedListener)
            {
                list = new LinkedList<IThreadTaskStatusChangedListener>(mStatusChangedListener);
            }

            LinkedListNode<IThreadTaskStatusChangedListener> nodeNow = list.First;
            while (nodeNow != null)
            {
                nodeNow.Value.onTaskStatusChangedListener(this, mThreadTaskStatus);

                nodeNow = nodeNow.Next;
            }
        }

        protected virtual void onCanceled()
        {

        }

        protected virtual void onFinished()
        {

        }

        public void Cancel()
        {
            Cancel(false);
        }

        public void Cancel(bool interrupt)
        {
            if (mIsCanceled)
            {
                return;
            }

            mIsCanceled = true;

            onCanceled();

            if (interrupt)
            {
                try
                {
                    lock (mLocker)
                    {
                        if (mCurrentThread != null)
                            mCurrentThread.Interrupt();
                    }
                }
                catch (System.Exception e)
                {
#if DEBUG
                    sLog.Warn(String.Format("ThreadTask::Cancel() error: {0}", e.ToString()));
#endif
                    //Logging.Logger.LoggerInstance.Write(String.Format("ThreadTask::Cancel() error: {0}", e), Logging.MessageType.Warning);

                }
            }


            bool notify = false;
            lock (mStatusChangedListener)
            {
                if (mThreadTaskStatus.Equals(ThreadTaskStatus.Idle))
                {
                    mThreadTaskStatus = ThreadTaskStatus.Finished;

                    notify = true;
                }
            }

            if (notify)
                notifyTaskStatusChangedListener(mThreadTaskStatus);

        }

        protected void Interrupt()
        {
            try
            {
                lock (mLocker)
                {
                    if (mCurrentThread != null)
                        mCurrentThread.Interrupt();
                }
            }
            catch (System.Exception e)
            {
#if DEBUG
                sLog.Warn(String.Format("ThreadTask::Interrupt() error: {0}", e.ToString()));
#endif
            }
        }

        public void RegisterTaskStatusListener(IThreadTaskStatusChangedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("listener can't be null...");
            }

            lock (mStatusChangedListener)
            {
                //foreach (IThreadTaskStatusChangedListener l in mStatusChangedListener)
                LinkedListNode<IThreadTaskStatusChangedListener> nodeNow = mStatusChangedListener.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Equals(listener))
                    {
                        return;
                    }

                    nodeNow = nodeNow.Next;
                }

                mStatusChangedListener.AddLast(listener);
            }

            listener.onTaskStatusChangedListener(this, this.mThreadTaskStatus);
        }

        public void UnregisterTaskStatusListener(IThreadTaskStatusChangedListener listener)
        {
            lock (mStatusChangedListener)
            {
                //foreach (IThreadTaskStatusChangedListener l in mStatusChangedListener)
                LinkedListNode<IThreadTaskStatusChangedListener> nodeNow = mStatusChangedListener.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Equals(listener))
                    {
                        //LinkedListNode<IThreadTaskStatusChangedListener> nodeDel = nodeNow;
                        //nodeNow = nodeNow.Next;
                        mStatusChangedListener.Remove(nodeNow);
                        break;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        public override string ToString()
        {
            if (IsBackground())
                return String.Format("{0}_{1}_BG", GetTag(), mThreadTaskStatus);// base.ToString();
            return String.Format("{0}_{1}", GetTag(), mThreadTaskStatus);
        }

        public virtual bool IsBackground()
        {
            return false;
        }

        public bool IsCanceled()
        {
            return mIsCanceled;
        }

        #region Members
        private volatile bool mIsCanceled;
        private string mTag;
        private object param;
        public object Param { get { return param; } }

        public ThreadPriority Priority { get; set; }

        private ThreadTaskStatus mThreadTaskStatus;
        public ThreadTaskStatus TaskStatus { get { return mThreadTaskStatus; } }

        private object mLocker = new object();

        private LinkedList<IThreadTaskStatusChangedListener> mStatusChangedListener;
        #endregion
    }
}
