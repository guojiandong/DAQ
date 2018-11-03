using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Sock
{
    /// <summary>
    /// Abstract socket task.
    /// </summary>
    public abstract class AbstractSocketTask<TMessage> : ThreadTask, 
        Communicate.FastIocp.Base.ISocketDataChangedListener<TMessage>, Communicate.FastIocp.Base.ISession where TMessage : class, Communicate.FastIocp.Base.Messaging.IMessage
    {
        private readonly LinkedList<Communicate.FastIocp.Base.ISocketStatusChangedListener> mStatusListenerList;
        private readonly LinkedList<Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage>> mDataListenerList;

        private readonly Threading.Queue.QueueManager<SocketItemEventArgs> mCacheList;

        //protected readonly Logging.Log sLog;

        /// <summary>
        /// proposal set to maintask
        /// </summary>
        /// <param name="maintask"></param>
        public AbstractSocketTask(object maintask = null) : base(maintask)
        {
            mStatusListenerList = new LinkedList<Communicate.FastIocp.Base.ISocketStatusChangedListener>();
            mDataListenerList = new LinkedList<Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage>>();
            mCacheList = new Threading.Queue.QueueManager<SocketItemEventArgs>();
        }

        protected abstract Communicate.FastIocp.Base.AbstractSessionBase GetSession();

        protected override void onCanceled()
        {
            mCacheList.Dispose();
            try
            {
                this.GetSession().Stop();
            }
            catch(Exception ex)
            {
                sLog.Warn("onCanceled(), stop socket session fail:"+ex.ToString());
            }

            base.onCanceled();
        }

        protected virtual void onTaskStarted(object param)
        {
            this.GetSession().RegisterListener(this);

            this.GetSession().Start();
        }

        protected virtual void onTaskStoped(object param)
        {
            this.GetSession().Stop();
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
                    SocketItemEventArgs item = null;
                    //if (mCacheList.Popup(ref item))
                    delay = mCacheList.PopupWithDelayTime(ref item);
                    if (delay == 0)
                    {
#if false //DEBUG
                        sLog.Info("Popup message:"+item.GetType().FullName);
#endif
                        if (item is SocketRecvDataEventArgs<TMessage>)
                        {
                            SocketRecvDataEventArgs<TMessage> args = (SocketRecvDataEventArgs<TMessage>)item;
                            this.NotifyDataListener(args.Value, args.Msg);
                        }
                        else if (item is SocketCustomRequestEventArgs)
                        {
                            onCustomRequestCommand((SocketCustomRequestEventArgs)item);
                        }
                        else
                        {
                            this.NotifyStatusListener(item);
                        }

                        continue;
                    }

                    if (onProcIdle())
                    {
                        continue;
                    }

                    //mCacheList.Wait(getDefaultThreadTimeout());
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

            sLog.Info("<<<<<<<<<<<<<<<<<<<<<<"+this.GetType().FullName+"<<doProc(8)<<<<<<<<<<<<<<<<<<<<<< ");
        }

        protected virtual bool onProcIdle()
        {
            return false;
        }

        /// <summary>
        /// Ons the custom request command.
        /// </summary>
        /// <param name="args">Arguments.</param>
        protected virtual void onCustomRequestCommand(SocketCustomRequestEventArgs args)
        {
        }

        /// <summary>
        /// 重写线程空闲等待时间，默认一直等待
        /// </summary>
        /// <returns></returns>
        protected virtual int getDefaultThreadTimeout()
        {
            return 2000;
        }

        public void Push(SocketItemEventArgs item, int delay_time = 0)
        {
            if (IsCanceled())
            {
                sLog.Warn("Push(), thread had canceled...");
                return;
            }

            mCacheList.Push(item, delay_time);
        }

        #region impl for ISocketDataChangedListener
        public virtual void OnConnected(Communicate.FastIocp.Base.IConnection connection)
        {
            //throw new NotImplementedException();
            if (IsCanceled())
            {
                sLog.Warn("OnConnected(), thread had canceled...");
                return;
            }

            mCacheList.Push(new SocketConnectedEventArgs(connection));
        }

        public virtual void OnDisconnected(Communicate.FastIocp.Base.IConnection connection, Exception ex)
        {
            if (IsCanceled())
            {
                sLog.Warn("OnDisconnected(), thread had canceled...");
                return;
            }

            mCacheList.Push(new SocketDisconnectedEventArgs(connection, ex));
        }

        public virtual void OnException(Communicate.FastIocp.Base.IConnection connection, Exception ex)
        {
            if (IsCanceled())
            {
                sLog.Warn("OnException(), thread had canceled...");
                return;
            }

            mCacheList.Push(new SocketExceptionEventArgs(connection, ex));
        }

        public virtual void OnReceived(Communicate.FastIocp.Base.IConnection connection, TMessage message)
        {
            if (IsCanceled())
            {
                sLog.Warn("OnReceived(), thread had canceled...");
                return;
            }

            mCacheList.Push(new SocketRecvDataEventArgs<TMessage>(connection, message));
        }

        public virtual void OnSendCallback(Communicate.FastIocp.Base.IConnection connection, Communicate.FastIocp.Base.Packet packet, bool isSuccess)
        {
            if (IsCanceled())
            {
                sLog.Warn("OnSendCallback(), thread had canceled...");
                return;
            }

            mCacheList.Push(new SocketSendCallbackEventArgs(connection, packet, isSuccess));
        }
        #endregion

        #region impl for ISession
        public int SocketBufferSize { get { return this.GetSession().SocketBufferSize; } }

        public int MessageBufferSize { get { return this.GetSession().MessageBufferSize; } }

        public virtual Communicate.FastIocp.Base.IConnection NewConnection(Socket socket)
        {
            return this.GetSession().NewConnection(socket);
        }

        public Communicate.FastIocp.Base.IConnection GetConnectionByID(long connectionID)
        {
            return this.GetSession().GetConnectionByID(connectionID);
        }

        public Communicate.FastIocp.Base.IConnection GetConnectionByTag(string tag)
        {
            return this.GetSession().GetConnectionByTag(tag);
        }

        public Communicate.FastIocp.Base.IConnection[] ListAllConnection()
        {
            return this.GetSession().ListAllConnection();
        }

        public int CountConnection()
        {
            return this.GetSession().CountConnection();
        }

        public virtual void Start()
        {
            this.GetSession().Start();
        }

        public virtual void Stop()
        {
            this.GetSession().Stop();
        }
        #endregion

        #region notify status
        protected virtual void onStatusRegisteredListener(Communicate.FastIocp.Base.ISocketStatusChangedListener listener)
        {
        }

        protected virtual void onStatusUnregisteredListener(Communicate.FastIocp.Base.ISocketStatusChangedListener listener)
        {
        }

        public ThreadTask RegisterStatusListener(Communicate.FastIocp.Base.ISocketStatusChangedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("RegisterStatusListener(), listener can't be null...");
            }

            //if (IsDisposed)
            //{
            //    Logger.Warn(TAG, "RegisterListener() failed, because this object has Disposed");
            //    return;
            //}

            lock (mStatusListenerList)
            {
                LinkedListNode<Communicate.FastIocp.Base.ISocketStatusChangedListener> nodeNow = mStatusListenerList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Equals(listener))
                    {
                        return this;
                    }
                    nodeNow = nodeNow.Next;
                }

                mStatusListenerList.AddLast(listener);
            }

            onStatusRegisteredListener(listener);

            return this;
        }

        public void UnregisterStatusListener(Communicate.FastIocp.Base.ISocketStatusChangedListener listener)
        {
            if (listener == null) return;

            bool flag = false;
            lock (mStatusListenerList)
            {
                flag = mStatusListenerList.Remove(listener);
            }

            if (flag)
                onStatusUnregisteredListener(listener);
        }

        public ThreadTask RegisterDataListener(Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage> listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("RegisterDataListener(), listener can't be null...");
            }

            //if (IsDisposed)
            //{
            //    Logger.Warn(TAG, "RegisterListener() failed, because this object has Disposed");
            //    return;
            //}

            lock (mDataListenerList)
            {
                LinkedListNode<Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage>> nodeNow = mDataListenerList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Equals(listener))
                    {
                        return this;
                    }
                    nodeNow = nodeNow.Next;
                }

                mDataListenerList.AddLast(listener);
            }

            return this;
        }

        public bool UnregisterDataListener(Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage> listener)
        {
            if (listener == null) return false;

            bool flag = false;
            lock (mDataListenerList)
            {
                flag = mDataListenerList.Remove(listener);
            }
            return flag;
        }

        #region notify
        public virtual void doNotifyStatusListener(Communicate.FastIocp.Base.ISocketStatusChangedListener listener, SocketItemEventArgs args)
        {
            if (typeof(System.ComponentModel.ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
                            && ((System.ComponentModel.ISynchronizeInvoke)listener).InvokeRequired)
            {
                ((System.ComponentModel.ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                {
                    if (args is SocketConnectedEventArgs)
                    {
                        listener.OnConnected(args.Value);
                    }
                    else if (args is SocketDisconnectedEventArgs)
                    {
                        listener.OnDisconnected(args.Value, ((SocketDisconnectedEventArgs)args).Ex);
                    }
                    else if (args is SocketExceptionEventArgs)
                    {
                        listener.OnException(args.Value, ((SocketExceptionEventArgs)args).Ex);
                    }
                    else if (args is SocketSendCallbackEventArgs)
                    {
                        listener.OnSendCallback(args.Value, ((SocketSendCallbackEventArgs)args).Packet, ((SocketSendCallbackEventArgs)args).Success);
                    }
                    else
                    {
                        sLog.Warn("doNotifyStatusListener(), unkown EventArgs:" + args.GetType().FullName);
                    }
                }), null);
            }
            else
            {
                if (args is SocketConnectedEventArgs)
                {
                    listener.OnConnected(args.Value);
                }
                else if (args is SocketDisconnectedEventArgs)
                {
                    listener.OnDisconnected(args.Value, ((SocketDisconnectedEventArgs)args).Ex);
                }
                else if (args is SocketExceptionEventArgs)
                {
                    listener.OnException(args.Value, ((SocketExceptionEventArgs)args).Ex);
                }
                else if (args is SocketSendCallbackEventArgs)
                {
                    listener.OnSendCallback(args.Value, ((SocketSendCallbackEventArgs)args).Packet, ((SocketSendCallbackEventArgs)args).Success);
                }
                else
                {
                    sLog.Warn("doNotifyStatusListener(), unkown EventArgs:"+args.GetType().FullName);
                }
            }
        }

        public virtual void NotifyStatusListener(SocketItemEventArgs args)
        {
#if false //DEBUG
            sLog.Info("NotifyStatusListener(1), Name:" + args.GetType().Name);
#endif
            LinkedList<Communicate.FastIocp.Base.ISocketStatusChangedListener> list;
            lock (mStatusListenerList)
            {
                list = new LinkedList<Communicate.FastIocp.Base.ISocketStatusChangedListener>(mStatusListenerList);
            }

            LinkedListNode<Communicate.FastIocp.Base.ISocketStatusChangedListener> nodeNow = list.First;
            while (nodeNow != null)
            {
                try
                {
#if false //DEBUG
                        sLog.Info("NotifyStatusListener(3), Name:" + args.GetType().Name);
#endif
                    doNotifyStatusListener(nodeNow.Value, args);

                }
                catch (ObjectDisposedException ex)
                {
                    LinkedListNode<Communicate.FastIocp.Base.ISocketStatusChangedListener> nodeDel = nodeNow;
                    nodeNow = nodeNow.Next;

                    lock (mStatusListenerList)
                        mStatusListenerList.Remove(nodeDel);

#if DEBUG
                    sLog.Warn("NotifyStatusListener(6) ObjectDisposedException:" + ex.ToString());
#endif
                    continue;
                }
                catch (Exception ex)
                {
#if DEBUG
                    sLog.Warn("NotifyStatusListener(7) exception:" + ex.ToString());
#endif
                }

                nodeNow = nodeNow.Next;
            }
        }


        public virtual void NotifyDataListener(Communicate.FastIocp.Base.IConnection connection, TMessage message)
        {
            LinkedList<Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage>> list;
            lock (mDataListenerList)
            {
                list = new LinkedList<Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage>>(mDataListenerList);
            }

            LinkedListNode<Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage>> nodeNow = list.First;
            while (nodeNow != null)
            {
                try
                {

                    Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage> listener = nodeNow.Value;

                    if (typeof(System.ComponentModel.ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
                                                && ((System.ComponentModel.ISynchronizeInvoke)listener).InvokeRequired)
                    {
                        ((System.ComponentModel.ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                        {
                            listener.OnReceived(connection, message);
                        }), null);
                    }
                    else
                    {
                        listener.OnReceived(connection, message);
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    LinkedListNode<Communicate.FastIocp.Base.ISocketMessageReceivedListener<TMessage>> nodeDel = nodeNow;
                    nodeNow = nodeNow.Next;

                    lock (mDataListenerList)
                        mDataListenerList.Remove(nodeDel);

#if DEBUG
                    sLog.Warn("NotifyDataListener(6) ObjectDisposedException:" + ex.ToString());
#endif
                    continue;
                }
                catch (Exception ex)
                {
#if DEBUG
                    sLog.Warn("NotifyDataListener(7) exception:" + ex.ToString());
#endif
                }

                nodeNow = nodeNow.Next;
            }
        }

        #endregion
        #endregion
    }
}
