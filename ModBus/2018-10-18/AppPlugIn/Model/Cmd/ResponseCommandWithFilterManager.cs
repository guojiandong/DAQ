using System;
using System.Collections.Generic;

namespace Ksat.AppPlugIn.Model.Cmd
{
    public class ResponseCommandWithFilterManager<TCommand>
    {
        public const int ALL_FILTER_KEY = -1;

        private readonly LinkedList<KeyValuePair<IResponseCommandEventListener<TCommand>, int>> mListenerList;
        private volatile bool mIsDisposed = false;

        protected readonly Logging.Log sLog;

        public ResponseCommandWithFilterManager()
        {
            sLog = new Logging.Log(this.GetType());
            mListenerList = new LinkedList<KeyValuePair<IResponseCommandEventListener<TCommand>, int>>();
        }

        public bool IsDisposed { get { return mIsDisposed; } }

        public void Dispose()
        {
            if (!mIsDisposed)
            {
                mIsDisposed = true;

                onDisposed();

                lock (mListenerList)
                {
                    mListenerList.Clear();
                }
            }
        }

        protected virtual void onDisposed()
        {
        }

        public int GetCount()
        {
            lock (mListenerList)
                return mListenerList.Count;
        }

        #region register
        protected virtual void onRegisteredListener(IResponseCommandEventListener<TCommand> listener)
        {
            onRegisteredListener(listener, ALL_FILTER_KEY);
        }

        protected virtual void onRegisteredListener(IResponseCommandEventListener<TCommand> listener, int filter)
        {

        }

        protected virtual void onUnregisteredListener(IResponseCommandEventListener<TCommand> listener)
        {
            onUnregisteredListener(listener, ALL_FILTER_KEY);
        }

        protected virtual void onUnregisteredListener(IResponseCommandEventListener<TCommand> listener, int filter)
        {
        }

        public void RegisterListener(IResponseCommandEventListener<TCommand> listener)
        {
            RegisterListener(listener, ALL_FILTER_KEY);
        }

        public void RegisterListener(IResponseCommandEventListener<TCommand> listener, int filter)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("RegisterListener(), listener can't be null...");
            }

            if (IsDisposed)
            {
                sLog.Warn("RegisterListener() failed, because this object has Disposed");
                return;
            }

            lock (mListenerList)
            {
                LinkedListNode<KeyValuePair<IResponseCommandEventListener<TCommand>, int>> nodeNow = mListenerList.First;
                while (nodeNow != null)
                {
                    KeyValuePair<IResponseCommandEventListener<TCommand>, int> item = nodeNow.Value;
                    if (item.Key.Equals(listener) && item.Value == filter)
                    {
                        return;
                    }
                    nodeNow = nodeNow.Next;
                }

                mListenerList.AddLast(new KeyValuePair<IResponseCommandEventListener<TCommand>, int>(listener, filter));
            }

            onRegisteredListener(listener, filter);
        }

        public void UnregisterListener(IResponseCommandEventListener<TCommand> listener)
        {
            UnregisterListener(listener, ALL_FILTER_KEY);
        }

        public void UnregisterListener(IResponseCommandEventListener<TCommand> listener, int filter)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("UnregisterListener(), listener can't be null...");
            }

            if (IsDisposed)
            {
                sLog.Warn("UnregisterListener() failed, because this object has Disposed");
                return;
            }

            //bool flag = false;
            lock (mListenerList)
            {
                LinkedListNode<KeyValuePair<IResponseCommandEventListener<TCommand>, int>> nodeNow = mListenerList.First;
                while (nodeNow != null)
                {
                    KeyValuePair<IResponseCommandEventListener<TCommand>, int> item = nodeNow.Value;
                    if (item.Key.Equals(listener) && (filter == ALL_FILTER_KEY || filter == item.Value))
                    {
                        LinkedListNode<KeyValuePair<IResponseCommandEventListener<TCommand>, int>> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mListenerList.Remove(nodeDel);

                        onUnregisteredListener(listener, item.Value);
                        continue;
                    }
                    nodeNow = nodeNow.Next;
                }
            }

            //if (flag)
            //    onUnregisteredListener(listener);
        }
        #endregion

        #region notify
        public virtual void doNotifyListener(IResponseCommandEventListener<TCommand> listener, object sender, TCommand cmd, object value)
        {
#if false
            Console.WriteLine(this.GetType().Name + "::doNotifyListener(), filter:" + filter 
                + ", listener:" + listener.GetType()
                 + ", cmd:" + cmd
                  + ", value:" + value);
#endif
            if (typeof(System.ComponentModel.ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
                            && ((System.ComponentModel.ISynchronizeInvoke)listener).InvokeRequired)
            {
                ((System.ComponentModel.ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                {
                    listener.onResponseCallback(sender, cmd, value);
                }), null);
            }
            else
            {
                listener.onResponseCallback(sender, cmd, value);
            }
        }

        public void NotifyListener(object sender, ResponseCommandEventArgs<TCommand> args)
        {
            int filter = ALL_FILTER_KEY;
            if (args is ResponseCommandWithFilterEventArgs<TCommand>)
            {
                filter = ((ResponseCommandWithFilterEventArgs<TCommand>)args).Filter;
            }

            LinkedList<KeyValuePair<IResponseCommandEventListener<TCommand>, int>> list;
            lock (mListenerList)
            {
                list = new LinkedList<KeyValuePair<IResponseCommandEventListener<TCommand>, int>>(mListenerList);
            }

            //lock (mListenerList)
            {
                LinkedListNode<KeyValuePair<IResponseCommandEventListener<TCommand>, int>> nodeNow = list.First;
                while (nodeNow != null)
                {
                    KeyValuePair<IResponseCommandEventListener<TCommand>, int> item = nodeNow.Value;
                    try
                    {
#if false
                    Console.WriteLine(this.GetType().Name + "::NotifyListener(), filter:" + filter + ", callback filter:" + item.Value);
#endif
                        if (item.Value == ALL_FILTER_KEY || filter == item.Value)
                            doNotifyListener(item.Key, sender, args.Command, args.Value);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        LinkedListNode<KeyValuePair<IResponseCommandEventListener<TCommand>, int>> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        lock (mListenerList)
                            mListenerList.Remove(nodeDel);

#if true
                        sLog.Warn("NotifyListener(6) ObjectDisposedException:" + ex.ToString());
#endif
                        continue;
                    }
                    catch (Exception ex)
                    {
#if true
                        sLog.Warn("NotifyListener(7) exception:" + ex.ToString());
#endif
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        #endregion
    }
}
