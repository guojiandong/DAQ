using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.V1
{
    public abstract class SocketTask : ThreadTask, IConnection
    {
        #region Properities
        public bool EnableDebug { get; set; }
        #endregion
        #region Members
        private LinkedList<Communicate.Base.IDataReceivedListener> mListenerList;
        private LinkedList<ISocketStatusChangedListener> mStatusListenerList;

        private int mMaxPacketSize = 0;
        #endregion
        public SocketTask(ISynchronizeInvoke control, Socket sock)
            : base(sock)
        {
            mListenerList = new LinkedList<Communicate.Base.IDataReceivedListener>();
            mStatusListenerList = new LinkedList<ISocketStatusChangedListener>();
#if DEBUG
            EnableDebug = true;
#else
            EnableDebug = false;
#endif
        }
        //private string mTag;

        //public void setTag(string tag)
        //{
        //    this.mTag = tag;
        //}

        //public virtual string getTag()
        //{
        //    return this.mTag;
        //}

        public void stop()
        {
            Cancel(true);
        }

        public virtual int send(string data)
        {
            byte[] buff = Encoding.UTF8.GetBytes(data);
            return send(buff, 0, buff.Length);
        }

        public int send(byte[] data)
        {
            return send(data, 0, data.Length);
        }

        protected virtual void onSocketCreated(Socket sock)
        {
        }

        public abstract int send(byte[] data, int offset, int size, SocketFlags socketFlags);

        public abstract int send(byte[] data, int offset, int count);

        public abstract string GetRemoteAddress();

        public abstract Socket GetSocket();

        /// <summary>
        /// if count = 0, then disable split packet...
        /// </summary>
        /// <param name="count"></param>
        public void SetMaxPacketSize(int count)
        {
            mMaxPacketSize = Math.Max(0, count);
        }

        protected int getMaxPacketSize()
        {
            return mMaxPacketSize;
        }

        //public string sendSync(string data, int timeout)
        //{
        //    throw new NotImplementedException();
        //}

        #region Implement for IConnection
        public void registerDataListener(Communicate.Base.IDataReceivedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("registerDataListener(), listener can't be null...");
            }

            lock (mListenerList)
            {
                if (mListenerList.Contains(listener))
                {
                    return;
                }

                mListenerList.AddLast(listener);
            }
        }

        public void unregisterDataListener(Communicate.Base.IDataReceivedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("unregisterDataListener(), listener can't be null...");
            }
            //throw new NotImplementedException();
            lock (mListenerList)
            {
                mListenerList.Remove(listener);
            }
        }

        public virtual void registerStatusListener(ISocketStatusChangedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("registerStatusListener(), listener can't be null...");
            }

            lock (mStatusListenerList)
            {
                if (mStatusListenerList.Contains(listener))
                {
                    return;
                }

                mStatusListenerList.AddLast(listener);
            }
        }

        public void unregisterStatusListener(ISocketStatusChangedListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("unregisterStatusListener(), listener can't be null...");
            }
            //throw new NotImplementedException();
            lock (mStatusListenerList)
            {
                mStatusListenerList.Remove(listener);
            }
        }
        #endregion

        protected void setSocketKeepAlive(Socket sock, bool enable)
        {
            uint dummy = 0;
            byte[] inOptionValues = new byte[System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 3];
            if (enable)
            {
                BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);//是否启用Keep-Alive
                BitConverter.GetBytes((uint)10000).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy));//多长时间开始第一次探测
                BitConverter.GetBytes((uint)3000).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 2);//探测时间间隔
            }
            else
            {
                BitConverter.GetBytes((uint)0).CopyTo(inOptionValues, 0);//是否启用Keep-Alive
                BitConverter.GetBytes((uint)10000).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy));//多长时间开始第一次探测
                BitConverter.GetBytes((uint)3000).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 2);//探测时间间隔
            }

            sock.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }

        private bool doNotifyDataReceivedListener(Communicate.Base.IDataReceivedListener listener, object sender, byte[] buffer, int count)
        {
#if true
            try
            {
                if (typeof(ISynchronizeInvoke).IsAssignableFrom(listener.GetType()) && ((ISynchronizeInvoke)listener).InvokeRequired)
                {
                    ((ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                    {
                        listener.OnDataReceivedCallback(sender, buffer, count);
                    }), null);
                }
                else
                {
                    listener.OnDataReceivedCallback(sender, buffer, count);
                }
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception)
            {
            }
#else
            lock (mListenerList)
            {
                foreach (IDataReceivedListener l in mListenerList)
                    l.onDataReceivedCallback(sender, buffer, count);
                //mListenerList.ForEach( (c) => c.onRecvDataCallback(sock, data)  );
            }
#endif

            return true;
        }

        protected virtual void notifyDataReceivedListener(byte[] buffer, int count)
        {
            notifyDataReceivedListener(this, buffer, count);
        }

        protected virtual void notifyDataReceivedListener(object sender, byte[] buffer, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("notifyDataReceivedListener() data can't be null...");
            }

            lock (mListenerList)
            {
                if (mListenerList.Count == 0) return;
            }
#if true
            byte[] buff = new byte[count];
            Array.Copy(buffer, buff, count);

            lock (mListenerList)
            {
                LinkedListNode<Communicate.Base.IDataReceivedListener> nodeNow = mListenerList.First;
                while (nodeNow != null)
                {
                    if (!doNotifyDataReceivedListener(nodeNow.Value, sender, buff, count))
                    {
                        LinkedListNode<Communicate.Base.IDataReceivedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mListenerList.Remove(nodeDel);
                        continue;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
#else
            ISynchronizeInvoke form = getMainThreadHandler();
            if (form != null && form.InvokeRequired)
            {
                byte[] buff = new byte[count];
                Array.Copy(buffer, buff, count);

                form.BeginInvoke(new EventHandler(delegate
                {
                    doNotifyDataReceivedListener(sender, buff, count);
                }), null);
            }
            else
            {
                doNotifyDataReceivedListener(sender, buffer, count);
            }
#endif
        }

        protected virtual void notifyDisconnectedListener()
        {
            notifyDisconnectedListener(this);
        }

        protected virtual void notifyDisconnectedListener(IConnection sock)
        {
            lock (mStatusListenerList)
            {
                if (mStatusListenerList.Count == 0) return;
            }

            lock (mStatusListenerList)
            {
                LinkedListNode<ISocketStatusChangedListener> nodeNow = mStatusListenerList.First;
                while (nodeNow != null)
                {
                    if (!doNotifyStatusListener(nodeNow.Value, sock, false))
                    {
                        LinkedListNode<ISocketStatusChangedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mStatusListenerList.Remove(nodeDel);
                        continue;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        //protected void doNotifyDisconnectedListener(IConnection sock)

        protected virtual void notifyConnectedListener()
        {
            notifyConnectedListener(this);
        }

        protected virtual void notifyConnectedListener(IConnection sock)
        {
            lock (mStatusListenerList)
            {
                if (mStatusListenerList.Count == 0) return;
            }

            lock (mStatusListenerList)
            {
                LinkedListNode<ISocketStatusChangedListener> nodeNow = mStatusListenerList.First;
                while (nodeNow != null)
                {
                    if (!doNotifyStatusListener(nodeNow.Value, sock, true))
                    {
                        LinkedListNode<ISocketStatusChangedListener> nodeDel = nodeNow;
                        nodeNow = nodeNow.Next;
                        mStatusListenerList.Remove(nodeDel);
                        continue;
                    }

                    nodeNow = nodeNow.Next;
                }
            }
        }

        protected bool doNotifyStatusListener(ISocketStatusChangedListener listener, IConnection sock, bool isConnected)
        {
            try
            {
                if ((listener is ISynchronizeInvoke)
                    && ((ISynchronizeInvoke)listener).InvokeRequired)
                {
                    ((ISynchronizeInvoke)listener).BeginInvoke(new EventHandler(delegate
                    {
                        if (isConnected)
                            listener.onSocketConnected(sock);
                        else
                            listener.onSocketDisconnected(sock);
                    }), null);
                }
                else
                {
                    if (isConnected)
                        listener.onSocketConnected(sock);
                    else
                        listener.onSocketDisconnected(sock);
                }
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception)
            {
            }

            return true;
        }

    }
}
