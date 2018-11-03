using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Ipc
{
    [Guid("EDFEF516-E192-48E7-B042-F90C0D7E65F0")]
    public abstract class AbstractRemoteApi : MarshalByRefObject
    {

    }

    [Guid("69785887-28B0-4A3B-A5B7-55BAEA080BA1")]
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
