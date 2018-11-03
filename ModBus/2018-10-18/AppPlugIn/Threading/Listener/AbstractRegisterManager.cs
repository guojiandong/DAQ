using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Listener
{
    [Guid("BD621D7C-BC5B-475F-829F-B1ADF0F0B3E0")]
    public abstract class AbstractRegisterManager<TListenerParam, TListenerInterface> : IDisposable
    {
        protected readonly LinkedList<TListenerInterface> mListenerList;
        //private List<TListenerItem> mInvalidList;
        public AbstractRegisterManager()
        {
            mListenerList = new LinkedList<TListenerInterface>();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
            onDisposed();

            lock (mListenerList)
            {
                mListenerList.Clear();
            }
        }

        protected virtual void onDisposed()
        {

        }

        protected virtual void doAddListener(TListenerInterface item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("doAddListener(), listener item can't be null...");
            }

            lock (mListenerList)
            {
                if (mListenerList.Contains(item))
                {
                    return;
                }

                mListenerList.AddLast(item);
            }
        }

        protected virtual void doRemoveListener(TListenerInterface item)
        {
            if (item == null) return;

            lock (mListenerList)
            {
                mListenerList.Remove(item);
            }
        }

        public void RegisterListener(TListenerInterface listener)
        {
            doAddListener(listener);
        }

        public void UnregisterListener(TListenerInterface listener)
        {
            doRemoveListener(listener);
        }

        //public abstract void RegisterListener(IListenerCallback<TListenerParam> listener);
        //{
        //    if (listener == null)
        //    {
        //        throw new ArgumentNullException("RegisterListener(), listener can't be null...");
        //    }
        //    //lock (mListenerList)
        //    //{
        //    //    if (mListenerList.Contains(listener))
        //    //    {
        //    //        return;
        //    //    }

        //    //    mListenerList.Add(listener);
        //    //}
        //}

        //public abstract void UnregisterListener(IListenerCallback<TListenerParam> listener);
        //{
        //    if (listener == null)
        //    {
        //        throw new ArgumentNullException("UnregisterListener(), listener can't be null...");
        //    }

        //    //lock (mListenerList)
        //    //{
        //    //    mListenerList.Remove(listener);
        //    //}
        //}

        public void NotifyListener(object sender, TListenerParam args)
        {
            lock (mListenerList)
            {
                LinkedListNode<TListenerInterface> nodeNow = mListenerList.First;
                while (nodeNow != null)
                {
                    try
                    {
                        NotifyListener(nodeNow.Value, sender, args);
                    }
                    catch (ObjectDisposedException)
                    {
                        LinkedListNode<TListenerInterface> nodeDel = nodeNow;
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

        public abstract void NotifyListener(TListenerInterface listener, object sender, TListenerParam args);
    }
}
