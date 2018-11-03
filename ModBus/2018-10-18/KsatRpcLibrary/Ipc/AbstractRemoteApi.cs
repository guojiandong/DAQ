using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KsatRpcLibrary.Ipc
{
    [Guid("A65F0F59-A799-4462-814C-0EF1E62113DD")]
    public abstract class AbstractRemoteApi : MarshalByRefObject
    {

    }

    [Guid("6500ACAE-27C8-4D6D-9284-BC03290C6C36")]
    public abstract class AbstractRemoteApiWithCallback<T> : AbstractRemoteApi
    {
        private static readonly LinkedList<T> mListenerList = new LinkedList<T>();

        //protected virtual void onRegisterListener(T listener)
        //{
        //}

        //protected virtual void onUnregisterListener(T listener)
        //{
        //}

        public static void RegisterListener(T listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("RegisterListener(), listener can't be null...");
            }

            lock (mListenerList)
            {
                if (mListenerList.Contains(listener))
                {
                    return;
                }

                mListenerList.AddLast(listener);
            }

            //onRegisterListener(listener);
        }
        
        public static void UnregisterListener(T listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("UnregisterListener(), listener can't be null...");
            }

            lock (mListenerList)
            {
                LinkedListNode<T> nodeNow = mListenerList.First;
                while(nodeNow != null)
                {
                    if (nodeNow.Value.Equals(listener))
                    {
                        mListenerList.Remove(nodeNow);
                        break;
                    }
                    nodeNow = nodeNow.Next;
                }
            }

            //onUnregisterListener(listener);
        }

        protected abstract void doNotifyListener(T listener, params object[] value);

        //protected virtual void doNotifyListener(T listener, params object[] value)
        //{
        //    if (typeof(ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
        //                && ((ISynchronizeInvoke)listener).InvokeRequired)
        //    {
        //        ((ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
        //        {
        //            listener.onCallback(sender, args);
        //        }), null);
        //    }
        //    else
        //    {
        //        listener.onCallback(sender, args);
        //    }
        //}

        public void NotifyListener(params object[] value)
        {
            lock (mListenerList)
            {
                LinkedListNode<T> nodeNow = mListenerList.First;
                while (nodeNow != null)
                {
                    try
                    {
                        doNotifyListener(nodeNow.Value, value);
                    }
                    catch (ObjectDisposedException)
                    {
                        LinkedListNode<T> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mListenerList.Remove(nodeDel);
                        continue;
                    }
                    catch (Exception)
                    {
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }
    }
}
