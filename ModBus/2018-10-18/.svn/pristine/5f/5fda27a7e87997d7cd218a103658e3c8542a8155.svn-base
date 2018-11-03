using Ksat.AppPlugIn.Communicate.SuperIoc.Protocol.Filter;
using Ksat.AppPlugIn.Model.Communication;
using Ksat.AppPlugIn.Threading.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Base
{
    public abstract class AbstraceSessionBase : ISession
    {
        private object _SyncLock = new object();
        public abstract CommunicateType CommunicationType { get; }

        public abstract string Tag { get; }

        public abstract string SessionID { get; protected set; }

        private bool _IsDisposed = false;

        private ChannelStatusListenerManager mChannelStatusListenerManager;

        public AbstraceSessionBase()
        {
            mChannelStatusListenerManager = new ChannelStatusListenerManager();
#if DEBUG
            this.EnableLog = true;
#else
            this.EnableLog = false;
#endif
        }


        public object SyncLock
        {
            get { return _SyncLock; }
        }

        public bool IsDisposed
        {
            get { return _IsDisposed; }
        }

        public bool EnableLog
        {
            get; set;
        }


        public abstract void Initialize(AbstractCommunicationProfile profile);

        public abstract object Session { get; }

        public abstract void Close();

        public void Dispose()
        {
            if (!this._IsDisposed)
            {
                _IsDisposed = true;

                onDisposed();

                mChannelStatusListenerManager.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        protected abstract void onDisposed();


        public int Read(byte[] data)
        {
            return Read(data, 0, data.Length);
        }

        public abstract int Read(byte[] data, int offset, int length);

        public int Write(byte[] data)
        {
            return Write(data, 0, data.Length);
        }

        public abstract int Write(byte[] data, int offset, int length);

        ///// <summary>
        ///// 过滤读取数据
        ///// </summary>
        ///// <param name="receiveFilter"></param>
        ///// <returns></returns>
        //public abstract IList<byte[]> Read(IReceiveFilter receiveFilter);


        ///// <summary>
        ///// 异步读取只写长度的数据
        ///// </summary>
        ///// <param name="dataLength"></param>
        ///// <param name="cts"></param>
        ///// <returns></returns>
        //protected abstract Task<byte[]> ReadAsync(int dataLength, CancellationTokenSource cts);


        #region listener manager
        public void RegisterSessionStatusListener(ISessionStatusListener listener)
        {
            mChannelStatusListenerManager.RegisterListener(listener);
        }

        public void UnregisterSessionStatusListener(ISessionStatusListener listener)
        {
            mChannelStatusListenerManager.UnregisterListener(listener);
        }

        public virtual void NotifySessionStatusChanged(SessionStatus status, object value = null)
        {
            mChannelStatusListenerManager.NotifyListener(this, new SessionStatusObjectEventArgs(status, value));
        }

        protected void doNotifySessionStatusChanged(object sender, SessionStatus status, object value = null)
        {
            mChannelStatusListenerManager.NotifyListener(sender, new SessionStatusObjectEventArgs(status, value));
        }


        private class ChannelStatusListenerManager : AbstractRegisterManager<SessionStatusObjectEventArgs, ISessionStatusListener>
        {
            public override void NotifyListener(ISessionStatusListener listener, object sender, SessionStatusObjectEventArgs args)
            {
                listener.OnSessionStatusChanged(sender, args.Status, args.Value);
            }
        }
        #endregion



        ///// <summary>
        ///// 读包长数据
        ///// </summary>
        ///// <param name="dataLength"></param>
        ///// <param name="readTimeout"></param>
        ///// <returns></returns>
        //internal byte[] ReceivePackageData(int dataLength, int readTimeout)
        //{
        //    byte[] bigData = null;
        //    CancellationTokenSource cts = new CancellationTokenSource();
        //    Task<byte[]> task = ReadAsync(dataLength, cts);
        //    task.Wait(readTimeout);
        //    if (task.IsCompleted)
        //    {
        //        bigData = task.Result;
        //    }
        //    else
        //    {
        //        cts.Cancel(true);
        //        if (!task.IsFaulted)
        //        {
        //            bigData = task.Result;
        //        }
        //    }
        //    return bigData;
        //}
    }
}
