
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Task.Sock
{
    /// <summary>
    /// Abstract socket binary task.
    /// </summary>
    public abstract class AbstractSocketBinaryTask : AbstractSocketTask<Communicate.FastIocp.Base.Messaging.BinaryMessage>
    {
        //private readonly LinkedList<Communicate.FastIocp.Base.ISocketStatusChangedListener> mStatusListenerList;
        private readonly LinkedList<Communicate.Base.IDataReceivedListener> mBaseDataListenerList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maintask">proposal set to maintask</param>
        public AbstractSocketBinaryTask(object maintask = null) : base(maintask)
        {
            //sLog = new Logging.Log(this.GetType());

            //mStatusListenerList = new LinkedList<Communicate.FastIocp.Base.ISocketStatusChangedListener>();
            mBaseDataListenerList = new LinkedList<Communicate.Base.IDataReceivedListener>();
            //mCacheList = new Threading.Queue.QueueManager<SocketItemEventArgs>();
        }


        #region notify status
        public ThreadTask RegisterDataListener(Communicate.Base.IDataReceivedListener listener)
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

            lock (mBaseDataListenerList)
            {
                LinkedListNode<Communicate.Base.IDataReceivedListener> nodeNow = mBaseDataListenerList.First;
                while (nodeNow != null)
                {
                    if (nodeNow.Value.Equals(listener))
                    {
                        return this;
                    }
                    nodeNow = nodeNow.Next;
                }

                mBaseDataListenerList.AddLast(listener);
            }

            return this;
        }

        public bool UnregisterDataListener(Communicate.Base.IDataReceivedListener listener)
        {
            if (listener == null) return false;

            bool flag = false;
            lock (mBaseDataListenerList)
            {
                flag = mBaseDataListenerList.Remove(listener);
            }
            return flag;
        }

        #region notify
        public override void NotifyDataListener(Communicate.FastIocp.Base.IConnection connection, Communicate.FastIocp.Base.Messaging.BinaryMessage message)
        {
            LinkedList<Communicate.Base.IDataReceivedListener> list;

            lock (mBaseDataListenerList)
                list = new LinkedList<Communicate.Base.IDataReceivedListener>(mBaseDataListenerList);
            
            {
                LinkedListNode<Communicate.Base.IDataReceivedListener> nodeNow = list.First;
                while (nodeNow != null)
                {
                    try
                    {

                        Communicate.Base.IDataReceivedListener listener = nodeNow.Value;

                        if (typeof(System.ComponentModel.ISynchronizeInvoke).IsAssignableFrom(listener.GetType())
                                                    && ((System.ComponentModel.ISynchronizeInvoke)listener).InvokeRequired)
                        {
                            ((System.ComponentModel.ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                            {
                                listener.OnDataReceivedCallback(connection, message.Payload, message.Payload.Length);
                            }), null);
                        }
                        else
                        {
                            listener.OnDataReceivedCallback(connection, message.Payload, message.Payload.Length);
                        }
                    }
                    catch (ObjectDisposedException ex)
                    {
                        LinkedListNode<Communicate.Base.IDataReceivedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;

                        lock (mBaseDataListenerList)
                            mBaseDataListenerList.Remove(nodeDel);

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

            base.NotifyDataListener(connection, message);
        }

        #endregion
        #endregion
    }
}
