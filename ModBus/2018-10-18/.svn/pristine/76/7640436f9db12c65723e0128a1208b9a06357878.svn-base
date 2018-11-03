using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading
{
    [Guid("B05EDE51-2DAA-43BB-AC1A-7E341EDAA3B2")]
    internal sealed class ThreadImpl : IDisposable
    {
        public ThreadImpl(ParameterizedThreadStart proc)
            : this(proc, false)
        {

        }

        public ThreadImpl(ParameterizedThreadStart proc, bool isBackground)
        {
            this.threadHandle = new Thread(proc);
            this.threadHandle.IsBackground = isBackground;
            this.mCurrentTask = null;
            this.mIsDisposed = false;
        }

        public void doStart()
        {
            if (this.mIsDisposed) return;

            this.threadHandle.Start(this);
        }

        private ThreadTask mCurrentTask = null;
        internal void Execute(ThreadTask task)
        {
            if (this.mIsDisposed) return;

            lock (threadHandle)
            {
                mCurrentTask = task;
            }

            try
            {
                if (task.Priority != this.threadHandle.Priority)
                {
                    threadHandle.Priority = task.Priority;
                }

                task.Execute(this);
            }
            catch (Exception)
            {
                this.mIsDisposed = true;
            }
            finally
            {
                this.mIsDisposed = true;

                lock (threadHandle)
                    mCurrentTask = null;
            }
        }

        internal void Cancel(bool interrupt = true)
        {
            if (interrupt)
            {
                try
                {
                    threadHandle.Interrupt();
                }
                catch (System.Exception e)
                {
                }
            }

            ThreadTask task;
            lock (threadHandle)
            {
                task = mCurrentTask;
            }

            if (task != null)
                task.Cancel(interrupt);
        }

        public void Dispose()
        {
            this.mIsDisposed = true;
            this.Cancel(true);
        }

        public bool IsDisposed()
        {
            return (this.mIsDisposed || this.threadHandle == null);
        }

        public bool IsIdle()
        {
            lock (threadHandle)
            {
                if (mCurrentTask == null)
                    return true;
            }
            return false;
        }

        public bool IsBackground
        {
            get { return this.threadHandle.IsBackground; }
            set { this.threadHandle.IsBackground = value; }
        }

        private readonly Thread threadHandle;

        public Thread ThreadHandle { get { return threadHandle; } }

        private volatile bool mIsDisposed;
    }
}
