using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task
{
    [Guid("B9FC329E-D0E9-4201-A1A7-F242CF57A59A")]
    public class MainTaskBase<TReq, TResp> : CommandThreadTask<TReq, TResp>, IThreadTaskStatusChangedListener
    {

        public delegate void OnTaskStartingHandler(MainTaskBase<TReq, TResp> task);

        public delegate void OnTaskStopingHandler(MainTaskBase<TReq, TResp> task);


        private readonly LinkedList<ThreadTask> mChildThreadTask;
        private bool mThreadStarted = false;

        public event OnTaskStartingHandler OnStartHandler;

        public event OnTaskStopingHandler OnStopHandler;

        //private ResponseCommandWithFilterManager mRespListenerList;

        public MainTaskBase(MainTaskBase<TReq, TResp> maintask = null, ProcessRequestCommandHandler handler = null) : base(maintask, handler)
        {
            mChildThreadTask = new LinkedList<ThreadTask>();
        }

        public void AddChildTask(ThreadTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("AddChildTask(), task can't be null...");
            }

            lock (mChildThreadTask)
            {
                LinkedListNode<ThreadTask> nodeNow = mChildThreadTask.First;
                while (nodeNow != null)
                {
                    if (task == nodeNow.Value)
                    {
                        return;
                    }

                    nodeNow = nodeNow.Next;
                }

                mChildThreadTask.AddLast(task);

                task.RegisterTaskStatusListener(this);

                if (mThreadStarted)
                {
                    MultiThreadManager.Instance().AddTask(task);
                }
            }
        }

        public ThreadTask GetChildTask(string tag)
        {
            if (String.IsNullOrEmpty(tag)) return null;
            lock (mChildThreadTask)
            {
                LinkedListNode<ThreadTask> nodeNow = mChildThreadTask.First;
                while (nodeNow != null)
                {
                    if (tag == nodeNow.Value.GetTag())
                    {
                        return nodeNow.Value;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            return null;
        }

        public T GetChildTask<T>() where T : ThreadTask
        {
            lock (mChildThreadTask)
            {
                LinkedListNode<ThreadTask> nodeNow = mChildThreadTask.First;
                while (nodeNow != null)
                {
                    if (typeof(T) == nodeNow.Value.GetType())
                    {
                        return (T)nodeNow.Value;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            return null;
        }

        public void RemoveChildTask<T>(string tag = "") where T : ThreadTask
        {
            if (String.IsNullOrEmpty(tag))
                tag = typeof(T).FullName;

            lock (mChildThreadTask)
            {
                LinkedListNode<ThreadTask> nodeNow = mChildThreadTask.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.GetTag().Equals(tag) && typeof(T) == nodeNow.Value.GetType())
                    {
                        mChildThreadTask.Remove(nodeNow.Value);
                        if (!nodeNow.Value.IsCanceled())
                            nodeNow.Value.Cancel(true);

                        break;
                    }
                    nodeNow = nodeNow.Next;
                }
            }
        }

        /// <summary>
        /// 线程启动前会进入该函数，
        /// 如果需要在启动线程的时候，初始化部分工作，可以重载该函数
        /// </summary>
        protected override void onTaskStarted(object param)
        {
            base.onTaskStarted(param);

            if (OnStartHandler != null)
                OnStartHandler(this);

            onStartChildTask();
        }

        /// <summary>
        /// 线程结束时会进入该函数
        /// </summary>
        protected override void onTaskStoped(object param)
        {
            onStopChildTask();

            if (OnStopHandler != null)
                OnStopHandler(this);

            base.onTaskStoped(param);
        }
        
        /// <summary>
        /// 线程启动前会进入该函数，
        /// 如果需要在启动线程的时候，初始化部分工作，可以重载该函数
        /// </summary>
        protected virtual void onStartChildTask()
        {
            lock (mChildThreadTask)
            {
                LinkedListNode<ThreadTask> nodeNow = mChildThreadTask.First;
                while (nodeNow != null)
                {
                    MultiThreadManager.Instance().AddTask(nodeNow.Value);

                    nodeNow = nodeNow.Next;
                }

                mThreadStarted = true;
            }
        }

        /// <summary>
        /// 线程结束时会进入该函数
        /// </summary>
        protected virtual void onStopChildTask()
        {
            lock (mChildThreadTask)
            {
                LinkedListNode<ThreadTask> nodeNow = mChildThreadTask.First;
                while (nodeNow != null)
                {
                    nodeNow.Value.UnregisterTaskStatusListener(this);

                    nodeNow.Value.Cancel(true);

                    nodeNow = nodeNow.Next;
                }
                mChildThreadTask.Clear();
            }
        }

        public void onTaskStatusChangedListener(ThreadTask task, ThreadTaskStatus status)
        {
            if(status == ThreadTaskStatus.Finished)
            {
                lock (mChildThreadTask)
                    mChildThreadTask.Remove(task);
            }
        }
    }
}
