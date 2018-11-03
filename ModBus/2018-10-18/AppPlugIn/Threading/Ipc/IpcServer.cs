using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Ipc
{
    [Guid("6D2712E3-67D5-4CAF-9C81-BA748000C9BD")]
    [ComVisible(true)]
    public class IpcServer : AbstractIpcObject
    {
        public static readonly int CONST_DEFAULT_PORT = 9951;
        private IChannel mChannel;
        private List<Type> mRegApiList;

        public IpcServer() : this(CONST_DEFAULT_PORT)
        {

        }

        public IpcServer(int port) : base((port > 0 ? port : CONST_DEFAULT_PORT))
        {
            mChannel = null;
            mRegApiList = new List<Type>();
        }

        public override IpcMode GetIpcMode()
        {
            return IpcMode.Local;
        }

        public override IpcType GetIpcType()
        {
            return IpcType.Server;
        }

        protected virtual IChannel createIpcChannel()
        {
            return new IpcServerChannel(GetChannelName(), GetIpcName());
        }

        public override void Start()
        {
            lock (typeof(IpcServer))
            {
                if (mChannel != null)
                {
#if DEBUG
                    Logger.Info(this.GetType().Name, "IpcServer::start() already started...");
#endif
                    return;
                }
                
                mChannel = createIpcChannel();
            }

            ChannelServices.RegisterChannel(mChannel, false);

            RegisterApi<DefaultGlobalApi>(true);

            OnStart();
#if DEBUG
            Logger.Info(this.GetType().Name, "IpcServer::start() start success...");
#endif
        }

        public bool IsStarted()
        {
            lock (typeof(IpcServer))
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

        public override void Stop()
        {
            OnStop();

            lock (typeof(IpcServer))
            {
                if (mChannel == null)
                {
#if DEBUG
                    Logger.Info(this.GetType().Name, "IpcServer::Stop() already stopped...");
#endif
                    return;
                }                

                ChannelServices.UnregisterChannel(mChannel);

                mChannel = null;
            }
        }


        //public void RegisterApi<T>() where T : MarshalByRefObject
        //{
        //    registerApi<T>(false);
        //}

        //public void RegisterApi<T>(bool isStaticMode) where T : MarshalByRefObject
        //{
        //    registerApi<T>(isStaticMode, typeof(T).FullName);
        //}

        public void RegisterApi<T>(Type obj) where T : MarshalByRefObject
        {
            RegisterApi<T>(false, obj.FullName);
        }

        public void RegisterApi<T>(string ipc_name) where T : MarshalByRefObject
        {
            RegisterApi<T>(false, ipc_name);
        }

        public void RegisterApi<T>(bool isStaticMode = false, string ipc_name = "") where T : MarshalByRefObject
        {
            //RemotingConfiguration.RegisterWellKnownServiceType(typeof(T), ipc_name,
            //    isStaticMode ? WellKnownObjectMode.Singleton : WellKnownObjectMode.SingleCall);

            RegisterApi(typeof(T), isStaticMode, ipc_name);
        }

        public void RegisterApi(Type obj, bool isStaticMode = false, string ipc_name = "")
        {
            if (String.IsNullOrEmpty(ipc_name))
                ipc_name = obj.FullName;

            Logger.Info("IpcServer", "RegisterApi(), isStaticMode:"+ isStaticMode+ ", ipc_name:"+ ipc_name+", type:"+obj.ToString());

            RemotingConfiguration.RegisterWellKnownServiceType(obj, ipc_name,
                isStaticMode ? WellKnownObjectMode.Singleton : WellKnownObjectMode.SingleCall);
        }

        //public void unregisterApi<T>() where T : MarshalByRefObject
        //{
        //    RemotingConfiguration..GetRegisteredWellKnownServiceTypes
        //}
    }
}
