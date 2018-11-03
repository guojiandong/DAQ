using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task
{
    public abstract class CommandThreadTask<TReq, TResp> : ThreadTask
    {
        private Model.Cmd.CommandQueueManager mCommandList;

        private Model.Cmd.ResponseCommandWithFilterManager<TResp> mRespListenerList;


        /// <summary>
        /// 处理消息事件
        /// </summary>
        /// <param name="cmd">请求消息内容</param>
        /// <returns>处理完的返回参数，默认为null, 主要是为同步消息提供支持</returns>
        public delegate object ProcessRequestCommandHandler(Model.Cmd.RequestCommandEventArgs<TReq> cmd);

        /// <summary>
        /// 处理消息事件委托函数
        /// </summary>
        public event ProcessRequestCommandHandler RequestHandler;

        public CommandThreadTask(object maintask = null, ProcessRequestCommandHandler handler = null) : base(maintask)
        {
            this.mCommandList = new Model.Cmd.CommandQueueManager();
            this.mRespListenerList = new Model.Cmd.ResponseCommandWithFilterManager<TResp>();

            if (handler != null)
                this.RequestHandler += handler;
        }


        protected override void onCanceled()
        {
            base.onCanceled();

            this.mCommandList.Dispose();
        }

        protected virtual void onTaskStarted(object param)
        {
        }

        protected virtual void onTaskStoped(object param)
        {
        }

        protected override void doProc(object param)
        {
            int delay_time = 0;
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
                    Model.Cmd.AbstractCommandEventArgs cmd = null;

                    delay_time = mCommandList.PopupWithDelayTime(ref cmd);
                    if (delay_time == 0 && cmd != null)
                    {
                        if (cmd.IsRequest)
                        {
                            this.doRequestCommand((Model.Cmd.RequestCommandEventArgs<TReq>)cmd);
                        }
                        else
                        {
                            this.notifyResponseCommandEvent((Model.Cmd.ResponseCommandEventArgs<TResp>)cmd);
                        }
                    }
                    else
                    {
                        onProcIdle();
                    }

#if EnableAutoResetEvent
                    if (delay_time == 0)
                    {
                        continue;
                    }
#endif

                    if (delay_time < 0)
                        mCommandList.Wait(getDefaultThreadTimeout());
                    else if (getDefaultThreadTimeout() < 0)
                        mCommandList.Wait(delay_time);
                    else
                        mCommandList.Wait(Math.Min(delay_time, getDefaultThreadTimeout()));
                }
                catch (Exception ex)
                {
                    sLog.Error("doProc(4), handle child event exception:", ex);
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

        /// <summary>
        /// 线程空闲是调用该函数
        /// </summary>
        protected virtual void onProcIdle()
        {

        }

        /// <summary>
        /// 重写线程空闲等待时间，默认一直等待
        /// </summary>
        /// <returns></returns>
        protected virtual int getDefaultThreadTimeout()
        {
            return 1000;
        }

        #region do request command
        private void doRequestCommand(Model.Cmd.RequestCommandEventArgs<TReq> cmd)
        {
            object obj = null;

            try
            {
                obj = onRequestCommandHandler(cmd);
            }
            catch (Exception ex)
            {
                sLog.Error("doRequestCommand(" + cmd.Command + ") event exception:", ex);
            }
            finally
            {
                if (cmd is Model.Cmd.RequestSyncCommandEventArgs<TReq>)
                {
                    Model.Cmd.RequestSyncCommandEventArgs<TReq> arg = (Model.Cmd.RequestSyncCommandEventArgs<TReq>)cmd;
                    arg.Result = obj;
                    arg.Release();
                }
            }
        }

        public void ClearAllCommand()
        {
            lock (mCommandList)
            {
                mCommandList.Clear();
            }
            sLog.Info("ClearAllCommand() clear all !");
        }

        public int GetCommandCount()
        {
            return mCommandList.GetCount();
        }

        protected virtual object onRequestCommandHandler(Model.Cmd.RequestCommandEventArgs<TReq> cmd)
        {
            if (RequestHandler != null)
                return RequestHandler(cmd);

            return null;
        }

        #endregion

        #region RequestCommand

        public object RequestSyncCommand(TReq cmd, object value, int timeout)
        {
            Model.Cmd.RequestSyncCommandEventArgs<TReq> arg = new Model.Cmd.RequestSyncCommandEventArgs<TReq>(cmd, value);
            if (requestCommand(arg, 0))
            {
                if (timeout > 0)
                    arg.WaitEvent.WaitOne(timeout);
                else
                    arg.WaitEvent.WaitOne();
            }

            return arg.Result;
        }


        public bool RequestCommand(TReq cmd)
        {
            return RequestCommand(cmd, null);
        }

        public bool RequestCommand(TReq cmd, object value)
        {
            return RequestCommand(cmd, value, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="value"></param>
        /// <param name="delay">Millisecond</param>
        public bool RequestCommand(TReq cmd, object value, int delay)
        {
            return requestCommand(new Model.Cmd.RequestCommandEventArgs<TReq>(cmd, value), delay);
        }

        protected virtual bool requestCommand(Model.Cmd.RequestCommandEventArgs<TReq> arg, int delay)
        {
            if (IsCanceled())
            {
                sLog.Info("RequestCommand(), task had canceled...");
                return false;
            }

            mCommandList.Push(arg, delay);
            return true;
        }
        #endregion

        #region ResponseCommand
        public void ResponseCommand(TResp cmd)
        {
            this.ResponseCommand(cmd, null);
        }

        public void ResponseCommand(TResp cmd, object value, int delay = 0)
        {
            this.responseCommand(new Model.Cmd.ResponseCommandEventArgs<TResp>(cmd, value), delay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="filter">Model.Cmd.ResponseCommandWithFilterEventArgs.ALL_FILTER_KEY</param>
        /// <param name="value"></param>
        /// <param name="delay"></param>
        public void ResponseCommandWithFilter(TResp cmd, int filter, object value, int delay = 0)
        {
            responseCommand(new Model.Cmd.ResponseCommandWithFilterEventArgs<TResp>(cmd, filter, value), delay);
        }

        protected virtual void responseCommand(Model.Cmd.ResponseCommandEventArgs<TResp> arg, int delay = 0)
        {
            if (IsCanceled())
            {
                sLog.Info("ResponseCommand(), task had canceled...");
                return;
            }

            mCommandList.Push(arg, delay);
        }


        protected virtual void notifyResponseCommandEvent(Model.Cmd.ResponseCommandEventArgs<TResp> arg)
        {
            mRespListenerList.NotifyListener(this, arg);
        }


        /// <summary>
        /// if(slot >= 0) then only receive the station response command...
        /// </summary>
        /// <param name="listener"></param>
        public virtual void RegisterResponseCommandEventListener(Model.Cmd.IResponseCommandEventListener<TResp> listener)
        {
            if (IsCanceled())
            {
                sLog.Info("registerResponseCommandEventListener(), task had canceled...");
                return;
            }

            mRespListenerList.RegisterListener(listener);
        }

        /// <summary>
        /// if(slot >= 0) then only receive the station response command...
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterResponseCommandEventListener(Model.Cmd.IResponseCommandEventListener<TResp> listener, int filter)
        {
            if (IsCanceled())
            {
                sLog.Info("registerResponseCommandEventListener(), task had canceled...");
                return;
            }

            mRespListenerList.RegisterListener(listener, filter);
        }

        public virtual void UnregisterResponseCommandEventListener(Model.Cmd.IResponseCommandEventListener<TResp> listener)
        {
            mRespListenerList.UnregisterListener(listener);
        }

        public void UnregisterResponseCommandEventListener(Model.Cmd.IResponseCommandEventListener<TResp> listener, int filter)
        {
            mRespListenerList.UnregisterListener(listener);
        }
        #endregion
    }
}
