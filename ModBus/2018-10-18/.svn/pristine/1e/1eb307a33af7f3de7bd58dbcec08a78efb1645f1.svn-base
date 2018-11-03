using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Listener
{
    [Guid("A273FC22-C9CC-421D-BF9C-655441DF7A98")]
    public sealed class CacheListenerItem<TFilterKey, TListenerParam>
    {
        public readonly TFilterKey Key;
        private readonly LinkedList<ChildListenerInfo> mChildList;
        private ushort mShortDelayTime;

        public CacheListenerItem(TFilterKey key, IListenerCallback<TListenerParam> listener, NotifyDelayMode mode)
        {
            this.Key = key;
            mChildList = new LinkedList<ChildListenerInfo>();

            register(listener, mode);
        }

        public void register(IListenerCallback<TListenerParam> listener, NotifyDelayMode mode)
        {
            lock (mChildList)
            {
                //foreach (ChildListenerInfo item in this.mChildList)
                LinkedListNode<ChildListenerInfo> nodeNow = this.mChildList.First;
                while(nodeNow != null)
                {
                    if (nodeNow.Value.Listener == listener)
                    {
                        this.mChildList.Remove(nodeNow);
                        break;
                    }

                    nodeNow = nodeNow.Next;
                }

                mChildList.AddLast(new ChildListenerInfo(listener, mode));
            }

            if (mShortDelayTime > 0)
            {
                ushort time = NotifyDelayTime.GetDelayTime(mode);
                if (mShortDelayTime > time)
                {
                    mShortDelayTime = time;
                }
            }
        }

        public bool unregister(IListenerCallback<TListenerParam> listener)
        {
            bool flag = false;
            lock (mChildList)
            {
                //foreach (ChildListenerInfo item in this.mChildList)
                LinkedListNode<ChildListenerInfo> nodeNow = this.mChildList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Listener == listener)
                    {
                        this.mChildList.Remove(nodeNow);
                        flag = true;
                        break;
                    }

                    nodeNow = nodeNow.Next;
                }

                if (flag && this.mChildList.Count > 0)
                {
                    NotifyDelayMode checkFastMode = NotifyDelayMode.Slow;
                    //foreach (ChildListenerInfo item in this.mChildList)
                    nodeNow = this.mChildList.First;
                    while (nodeNow != null)
                    {
                        if (nodeNow.Value.DelayMode < checkFastMode)
                            checkFastMode = nodeNow.Value.DelayMode;

                        nodeNow = nodeNow.Next;
                    }

                    mShortDelayTime = NotifyDelayTime.GetDelayTime(checkFastMode);
                }
            }

            return flag;
        }

        public void notifyListener(object sender, TListenerParam args)
        {
            lock (mChildList)
            {
                if (mChildList != null && mChildList.Count > 0)
                {
                    //foreach (ChildListenerInfo item in list)
                    LinkedListNode<ChildListenerInfo> nodeNow = mChildList.First;
                    while (nodeNow != null)
                    {
                        try
                        {
                            nodeNow.Value.notifyListener(sender, args);
                        }
                        catch (ObjectDisposedException)
                        {
                            LinkedListNode<ChildListenerInfo> nodeDel = nodeNow;
                            nodeNow = nodeNow.Next;
                            mChildList.Remove(nodeDel);
                            continue;
                        }
                        catch (Exception)
                        {
                        }

                        nodeNow = nodeNow.Next;
                    }
                    
                }
                
                if (mChildList != null && mChildList.Count > 0)
                {
                    NotifyDelayMode checkFastMode = NotifyDelayMode.Slow;
                    //foreach (ChildListenerInfo item in this.mChildList)
                    LinkedListNode<ChildListenerInfo> nodeNow = this.mChildList.First;
                    while (nodeNow != null)
                    {
                        if (nodeNow.Value.DelayMode < checkFastMode)
                            checkFastMode = nodeNow.Value.DelayMode;

                        nodeNow = nodeNow.Next;
                    }

                    mShortDelayTime = NotifyDelayTime.GetDelayTime(checkFastMode);
                }
            }

            

            //lock (mChildList)
            //{
            //    if (mChildList.Count == 0)
            //    {
            //        throw new ObjectDisposedException("No have child listener...");
            //    }
                
            //    NotifyDelayMode checkFastMode = NotifyDelayMode.Slow;
            //    //foreach (ChildListenerInfo item in this.mChildList)
            //    LinkedListNode<ChildListenerInfo> nodeNow = this.mChildList.First;
            //    while (nodeNow != null)
            //    {
            //        if (nodeNow.Value.DelayMode < checkFastMode)
            //            checkFastMode = nodeNow.Value.DelayMode;

            //        nodeNow = nodeNow.Next;
            //    }

            //    mShortDelayTime = NotifyDelayTime.GetDelayTime(checkFastMode);
            //}
        }

        public int GetListenerCount()
        {
            return mChildList.Count;
        }

        public ushort GetDelayTime()
        {
            return mShortDelayTime;
        }


        
        private class ChildListenerInfo
        {
            public readonly IListenerCallback<TListenerParam> Listener;
            public readonly NotifyDelayMode DelayMode;
            private long mLastNotifyTime;

            public ChildListenerInfo(IListenerCallback<TListenerParam> listener, NotifyDelayMode mode)
            {
                this.Listener = listener;
                this.DelayMode = mode;
                this.mLastNotifyTime = 0;
            }

            public void notifyListener(object sender, TListenerParam args)
            {
                if (DelayMode == NotifyDelayMode.RealTime)
                {
                    long curTime = DateTime.Now.Ticks;

                    if (((curTime - mLastNotifyTime) / TimeSpan.TicksPerMillisecond) < NotifyDelayTime.GetDelayTime(DelayMode))
                    {
                        return;
                    }

                    mLastNotifyTime = curTime;
                }

                if (Listener.GetType().IsSubclassOf(typeof(ISynchronizeInvoke))
                                && ((ISynchronizeInvoke)Listener).InvokeRequired)
                {
                    ((ISynchronizeInvoke)Listener).BeginInvoke(new EventHandler(delegate
                    {
                        Listener.onCallback(sender, args);
                    }), null);
                }
                else
                {
                    Listener.onCallback(sender, args);
                }

                //try
                //{
                    
                //}
                //catch (Exception)
                //{
                //}
            }
        }
    }
}
