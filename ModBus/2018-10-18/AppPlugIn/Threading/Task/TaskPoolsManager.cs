using System;
using System.Collections.Generic;

namespace Ksat.AppPlugIn.Threading.Task
{
    public class TaskPoolsManager: ThreadPoolsManager<QueueAsyncTask.AsyncTaskDelegateInfo>
    {
        
        public TaskPoolsManager(int minthread = 2, int maxthread = 5) : base(minthread, maxthread)
        {
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


            base.Push(new QueueAsyncTask.AsyncTaskDelegateInfo(task, param), delay);
        }

        protected override void OnDataProc(QueueAsyncTask.AsyncTaskDelegateInfo data)
        {
            data.DoCallback(this);
        }
    }
}
