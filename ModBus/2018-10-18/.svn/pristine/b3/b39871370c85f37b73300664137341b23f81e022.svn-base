using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task
{
    /// <summary>
    /// 异步线程，
    /// TParam : doInBackground(TParam value) 的输入参数类型
    /// TProgress ： onProgressUpdate(TProgress value) 的参数类型
    /// TResult ： TResult doInBackground(TParam value) 的返回参数类型
    /// </summary>
    /// <typeparam name="TResult">doInBackground</typeparam>
    public abstract class AsyncTask<TParam, TProgress, TResult> : ThreadTask
    {
        #region Members
        private readonly ISynchronizeInvoke mFormControl;
        #endregion

        #region Constructor
        /// <summary>
        /// 异步线程，
        /// </summary>
        /// <param name="control">主线程Form或者某个控件</param>
        /// /// <param name="param">输入参数， 将传入doInBackground(TParam value)</param>
        public AsyncTask(ISynchronizeInvoke control, TParam param) : base(param)
        {
            mFormControl = control;
        }
        #endregion

        protected internal ISynchronizeInvoke getMainThreadHandler()
        {
            return mFormControl;
        }

        protected bool InvokeRequired()
        {
            if (mFormControl != null)
                return mFormControl.InvokeRequired;
            return false;
        }

        /// <summary>
        /// 线程运行之前会触发该方法
        /// 该方法运行在UI线程当中,并且运行在UI线程当中 可以对UI空间进行设置  
        /// </summary>
        protected virtual void onPreExecute()
        {
        }


        /// <summary>
        /// 该函数运行在子线程中。
        /// </summary>
        /// <returns></returns>
        protected abstract TResult doInBackground(TParam value);

        /// <summary>
        /// 在线程运行完之后会触发该方法
        /// 该方法运行在UI线程当中,并且运行在UI线程当中 可以对UI空间进行设置  
        /// </summary>
        protected virtual void onPostExecute(TResult result)
        {
        }

        internal override void notifyFinished()
        {
            if (mFormControl != null && mFormControl.InvokeRequired)
            {
                mFormControl.BeginInvoke(new EventHandler(delegate
                {
                    base.notifyFinished();
                }), null);
            }
            else
            {
                base.notifyFinished();
            }
        }

        /// <summary>
        ///  这里的TProgress参数对应AsyncTask中的第二个参数
        ///  在doInBackground方法当中，，每次调用publishProgress方法都会触发onProgressUpdate执行
        ///  onProgressUpdate是在UI线程中执行，所有可以对UI空间进行操作
        /// </summary>
        /// <param name="value"></param>
        protected virtual void onProgressUpdate(TProgress value)
        {
        }

        public void publishProgress(TProgress value)
        {
            BeginInvoke(new EventHandler(delegate
            {
                onProgressUpdate(value);
            }), null);
        }

        public void BeginInvoke(EventHandler handler, EventArgs e)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler can't be null...");
            }

            if (mFormControl != null && mFormControl.InvokeRequired)
            {
                mFormControl.BeginInvoke(handler, new object[] { this, e });
            }
            else
            {
                handler(this, e);
            }
        }

        ///// <summary>
        ///// 让委托 handler 运行在主线程上
        ///// </summary>
        ///// <param name="handler"></param>
        ///// <param name="e"></param>
        //public void BeginInvoke(AsyncTaskEventHandler handler, EventArgs e)
        //{
        //    if (handler == null)
        //    {
        //        throw new ArgumentNullException("handler can't be null...");
        //    }

        //    if (mFormControl != null && mFormControl.InvokeRequired)
        //    {
        //        mFormControl.BeginInvoke(handler, new object[] { this, e });
        //    }
        //    else
        //    {
        //        handler(this, e);
        //    }
        //}

        protected sealed override void doProc(object param)
        {
            try
            {
                if (mFormControl != null && mFormControl.InvokeRequired)
                {
                    mFormControl.Invoke(new EventHandler(delegate
                    {
                        onPreExecute();
                    }), null);
                }
                else
                {
                    onPreExecute();
                }

                TResult ret = doInBackground((TParam)param);

                if (mFormControl != null && mFormControl.InvokeRequired)
                {
                    mFormControl.BeginInvoke(new EventHandler(delegate
                    {
                        onPostExecute(ret);
                    }), null);
                }
                else
                {
                    onPostExecute(ret);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("AsyncTask::doProc(), exception:" + e.ToString());
            }
        }
    }
}
