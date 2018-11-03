using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace KsatRpcLibrary.Ipc
{
    [Guid("E8C37C5F-30DA-48D6-BE75-E712C5BC4ECC")]
    [ComVisible(true)]
    public class IpcClient : AbstractIpcObject
    {
        private IChannel mChannel;

        public IpcClient() : this(IpcServer.CONST_DEFAULT_PORT)
        {

        }

        public IpcClient(int port) : base((port > 0 ? port : IpcServer.CONST_DEFAULT_PORT))
        {
            mChannel = null;
        }

        public override IpcMode GetIpcMode()
        {
            return IpcMode.Local;
        }

        public override IpcType GetIpcType()
        {
            return IpcType.Client;
        }

        protected virtual IChannel createIpcChannel()
        {
            return new IpcClientChannel(GetChannelName(), null);
        }

        public override void Start()
        {
            lock (typeof(IpcClient))
            {
                if(mChannel != null)
                {
#if false //DEBUG
                    Logger.Info(this.GetType().Name, "IpcClient::start() already started...");
#endif
                    return;
                }
                mChannel = createIpcChannel();
            }

            ChannelServices.RegisterChannel(mChannel, false);

            OnStart();
        }

        public override void Stop()
        {
            OnStop();

            lock (typeof(IpcClient))
            {
                if (mChannel == null)
                {
#if false //DEBUG
                    Logger.Info(this.GetType().Name, "IpcClient::Stop() already stopped...");
#endif
                    return;
                }

                ChannelServices.UnregisterChannel(mChannel);
                mChannel = null;
            }
        }

        public bool IsStarted()
        {
            lock (typeof(IpcClient))
            {
                if (mChannel != null)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnStop()
        {

        }
                
        public int CheckIsOnline()
        {
            lock (typeof(IpcClient))
            {
                if (mChannel == null)
                {
                    throw new NullReferenceException("CheckIsOnline(), Please call Start() first...");
                }
            }

            try
            {
                long tick = DateTime.Now.Ticks;

                DefaultGlobalApi defapi = FindApiObject<DefaultGlobalApi>();

                if (defapi == null)
                {
#if false //DEBUG
                    Logger.Warn(this.GetType().Name, "CheckIsOnline(1) Failed, can't found the DefaultGlobalApi object.");
#endif
                    return -1;
                }
                
                Random ra = new Random();
                int value = ra.Next(int.MinValue, int.MaxValue);

                int resp_value = defapi.CheckApi(value);
                if (resp_value == value)
                {

                    int delaytime = Convert.ToInt32((DateTime.Now.Ticks - tick) / TimeSpan.TicksPerMillisecond);
#if false //DEBUG
                    Logger.Info(this.GetType().Name, "CheckIsOnline(2) successed value:" + value+", delay:"+ delaytime+" Ms");
#endif
                    return delaytime;
                }
#if false //DEBUG
                Logger.Warn(this.GetType().Name, "CheckIsOnline(3) Failed, req value:" + value + "!= resp value:"+ resp_value 
                    + ", delay:" + Convert.ToInt32((DateTime.Now.Ticks - tick) / TimeSpan.TicksPerMillisecond) + " Ms");
#endif
            }
            catch (Exception ex)
            {
#if false //DEBUG
                Logger.Error(this.GetType().Name, "CheckIsOnline() exception:", ex);
#endif
            }
            return -1;
        }
    }
}
