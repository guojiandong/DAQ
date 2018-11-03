using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Listener
{
    [Guid("1C2363B1-0F98-4586-9C9C-34EF9EA2BCBF")]
    public class RegisterManagerWithFilter<TFilterKey, TListenerParam> 
        : AbstractRegisterManager<TListenerParam, CacheListenerItem<TFilterKey, TListenerParam>>
    {
        #region Properties
        public TFilterKey AllFilterKey { get; private set; }
        #endregion

        public RegisterManagerWithFilter(TFilterKey all_key) : base()
        {
            AllFilterKey = all_key;
        }
        
        public void RegisterListener(IListenerCallback<TListenerParam> listener)
        {
            RegisterListener(listener, AllFilterKey);
        }

        public void RegisterListener(IListenerCallback<TListenerParam> listener, TFilterKey key)
        {
            RegisterListener(listener, key, NotifyDelayMode.RealTime);
        }

        public void RegisterListener(IListenerCallback<TListenerParam> listener, TFilterKey key, NotifyDelayMode mode)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("RegisterListener(), listener can't be null...");
            }

            lock (mListenerList)
            {
                LinkedListNode<CacheListenerItem<TFilterKey, TListenerParam>> nodeNow = mListenerList.First;
                //foreach (CacheListenerItem<TFilterKey, TListenerParam> item in mListenerList)
                while(nodeNow != null)
                {
                    CacheListenerItem<TFilterKey, TListenerParam> item = nodeNow.Value;
                    if (item.Key.Equals(key))
                    {
                        item.register(listener, mode);
                        return;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            doAddListener(new CacheListenerItem<TFilterKey, TListenerParam>(key, listener, mode));
        }

        public void UnregisterListener(IListenerCallback<TListenerParam> listener)
        {
            UnregisterListener(listener, AllFilterKey);
        }

        public void UnregisterListener(IListenerCallback<TListenerParam> listener, TFilterKey key)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("UnregisterListener(), listener can't be null...");
            }

            lock (mListenerList)
            {
                LinkedList<CacheListenerItem<TFilterKey, TListenerParam>> removeList = new LinkedList<CacheListenerItem<TFilterKey, TListenerParam>>();

                LinkedListNode<CacheListenerItem<TFilterKey, TListenerParam>> nodeNow = mListenerList.First;
                //foreach (CacheListenerItem<TFilterKey, TListenerParam> item in mListenerList)
                while (nodeNow != null)
                {
                    CacheListenerItem<TFilterKey, TListenerParam> item = nodeNow.Value;
                    if (item.Key.Equals(key))
                    {
                        if (item.unregister(listener) && item.GetListenerCount() == 0)
                        {
                            removeList.AddLast(item);
                        }
                        break;
                    }

                    nodeNow = nodeNow.Next;
                }

                if (AllFilterKey.Equals(key))
                {
                    nodeNow = mListenerList.First;
                    //foreach (CacheListenerItem<TFilterKey, TListenerParam> item in mListenerList)
                    while (nodeNow != null)
                    {
                        CacheListenerItem<TFilterKey, TListenerParam> item = nodeNow.Value;
                        if (item.unregister(listener) && item.GetListenerCount() == 0)
                        {
                            removeList.AddLast(item);
                        }
                        nodeNow = nodeNow.Next;
                    }
                }

                nodeNow = removeList.First;
                while (nodeNow != null) //foreach (CacheListenerItem<TFilterKey, TListenerParam> item in removeList)
                {
                    mListenerList.Remove(nodeNow.Value);
					
					nodeNow = nodeNow.Next;
                }
            }
        }

        public ushort GetShortDelayTime()
        {
            ushort delay = NotifyDelayTime.GetDelayTime(NotifyDelayMode.Slow);

            lock (mListenerList)
            {
                LinkedListNode<CacheListenerItem<TFilterKey, TListenerParam>> nodeNow = mListenerList.First;
                //foreach (CacheListenerItem<TFilterKey, TListenerParam> item in mListenerList)
                while(nodeNow != null)
                {
                    CacheListenerItem<TFilterKey, TListenerParam> item = nodeNow.Value;
                    if (delay > item.GetDelayTime())
                    {
                        delay = item.GetDelayTime();

                        if (delay == 0)
                            break;
                    }

                    nodeNow = nodeNow.Next;
                }
            }

            return delay;
        }

        public void NotifyListener(object sender, TFilterKey tag, TListenerParam args)
        {
            LinkedList<CacheListenerItem<TFilterKey, TListenerParam>> list = null;
            lock (mListenerList)
            {
                list = new LinkedList<CacheListenerItem<TFilterKey, TListenerParam>>(mListenerList);
            }

            if (list != null && list.Count > 0)
            {
                LinkedListNode<CacheListenerItem<TFilterKey, TListenerParam>> nodeNow = list.First;
                //foreach (CacheListenerItem<TFilterKey, TListenerParam> item in list)
                while(nodeNow != null)
                {
                    CacheListenerItem<TFilterKey, TListenerParam> item = nodeNow.Value;

                    if (!item.Key.Equals(tag) && !item.Key.Equals(AllFilterKey))
                    {
                        nodeNow = nodeNow.Next;

                        continue;
                    }

                    try
                    {
                        NotifyListener(item, sender, args);
                    }
                    catch (ObjectDisposedException)
                    {
                        LinkedListNode<CacheListenerItem<TFilterKey, TListenerParam>> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        lock (mListenerList)
                        {
                            mListenerList.Remove(nodeDel);
                        }
                        continue;
                    }
                    catch (Exception)
                    {
                    }

                    nodeNow = nodeNow.Next;
                }

                list.Clear();
            }
        }

        public override void NotifyListener(CacheListenerItem<TFilterKey, TListenerParam> item, object sender, TListenerParam args)
        {
            if (item != null)
            {
                item.notifyListener(sender, args);
            }
        }



        //private class CacheListenerItem
        //{
        //    public readonly TFilterKey Key;
        //    private readonly List<ChildListenerInfo> mChildList;
        //    private ushort mShortDelayTime;

        //    public CacheListenerItem(TFilterKey Key, IListenerCallback<TListenerParam> listener, NotifyDelayMode mode)
        //    {
        //        mChildList = new List<ChildListenerInfo>();
        //    }

        //    public void register(IListenerCallback<TListenerParam> listener, NotifyDelayMode mode)
        //    {
        //        foreach (ChildListenerInfo item in this.mChildList)
        //        {
        //            if (item.Listener == listener)
        //            {
        //                this.mChildList.Remove(item);
        //                break;
        //            }
        //        }

        //        ushort time = NotifyDelayTime.GetDelayTime(mode);
        //        if (mShortDelayTime > time)
        //        {
        //            mShortDelayTime = time;
        //        }

        //        mChildList.Add(new ChildListenerInfo(listener, mode));
        //    }

        //    public bool unregister(IListenerCallback<TListenerParam> listener)
        //    {
        //        bool flag = false;
        //        foreach (ChildListenerInfo item in this.mChildList)
        //        {
        //            if (item.Listener == listener)
        //            {
        //                this.mChildList.Remove(item);
        //                flag = true;
        //                break;
        //            }
        //        }

        //        if (flag && this.mChildList.Count > 0)
        //        {
        //            NotifyDelayMode checkFastMode = NotifyDelayMode.Slow;
        //            foreach (ChildListenerInfo item in this.mChildList)
        //            {
        //                if (item.DelayMode < checkFastMode)
        //                    checkFastMode = item.DelayMode;
        //            }

        //            mShortDelayTime = NotifyDelayTime.GetDelayTime(checkFastMode);
        //        }

        //        return flag;
        //    }

        //    public void notifyListener(object sender, TListenerParam args)
        //    {
        //        foreach (ChildListenerInfo item in mChildList)
        //        {
        //            item.notifyListener(sender, args);
        //        }
        //    }

        //    public int GetListenerCount()
        //    {
        //        return mChildList.Count;
        //    }

        //    public ushort GetDelayTime()
        //    {
        //        return mShortDelayTime;
        //    }
        //}

        //private class ChildListenerInfo
        //{
        //    public readonly IListenerCallback<TListenerParam> Listener;
        //    public readonly NotifyDelayMode DelayMode;
        //    private long mLastNotifyTime;

        //    public ChildListenerInfo(IListenerCallback<TListenerParam> listener, NotifyDelayMode mode)
        //    {
        //        this.Listener = listener;
        //        this.DelayMode = mode;
        //        this.mLastNotifyTime = 0;
        //    }

        //    public void notifyListener(object sender, TListenerParam args)
        //    {
        //        if (DelayMode == NotifyDelayMode.RealTime)
        //        {
        //            long curTime = DateTime.Now.Ticks;

        //            if (((curTime - mLastNotifyTime) / TimeSpan.TicksPerMillisecond) < NotifyDelayTime.GetDelayTime(DelayMode))
        //            {
        //                return;
        //            }

        //            mLastNotifyTime = curTime;
        //        }

        //        try
        //        {
        //            if (Listener.GetType().IsSubclassOf(typeof(ISynchronizeInvoke))
        //                        && ((ISynchronizeInvoke)Listener).InvokeRequired)
        //            {
        //                ((ISynchronizeInvoke)Listener).BeginInvoke(new EventHandler(delegate
        //                {
        //                    Listener.onCallback(sender, args);
        //                }), null);
        //            }
        //            else
        //            {
        //                Listener.onCallback(sender, args);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //}
    }
}
