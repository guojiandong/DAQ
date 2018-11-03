using System;
using System.Runtime.InteropServices;

namespace KsatRpcLibrary.Ipc
{
    [Guid("7AF4C769-2288-4BCD-9743-437F504BE60A")]
    public enum IpcMode : int
    {
        /// <summary>
        /// localhost
        /// </summary>
        Local,

        /// <summary>
        /// tcp, support remote compute
        /// </summary>
        Tcp,

        /// <summary>
        /// Http
        /// </summary>
        Http,
    }

    [Guid("143B6E10-1939-41C5-83D4-C27D25DC2EDF")]
    public enum IpcType : int
    {
        /// <summary>
        /// localhost
        /// </summary>
        Client,

        /// <summary>
        /// tcp, support remote compute
        /// </summary>
        Server,
    }

    [Guid("79FF2835-0171-469E-88B1-0E520E6343FF")]
    [ComVisible(true)]
    public abstract class AbstractIpcObject
    {
        
        public int Port { get; set; }

        protected AbstractIpcObject(int port)
        {
            this.Port = port;

            //mIpcServerChannel = new IpcServerChannel(String.Format("localhost:{0}", mPort));
        }

        public abstract IpcMode GetIpcMode();

        public abstract IpcType GetIpcType();

        protected virtual string GetIpcUri()
        {
            //return "localhost";
            return "127.0.0.1";
        }

        public string GetIpcName()
        {
            return String.Format("{0}:{1}", GetIpcUri(), this.Port);
        }

        public string GetChannelName()
        {
            return String.Format("{0}-{1}-{2}:{3}", GetIpcMode(), GetIpcType(), GetIpcUri(), this.Port);
        }

        public T FindApiObject<T>()// where T : MarshalByRefObject
        {
            return FindApiObject<T>(typeof(T).FullName);
        }

        public T FindApiObject<T>(string ipc_name)// where T : MarshalByRefObject
        {
            string protocal = "ipc";
            if (GetIpcMode() == IpcMode.Tcp)
                protocal = "tcp";
            else if (GetIpcMode() == IpcMode.Http)
                protocal = "http";

            return (T)Activator.GetObject(typeof(T),
                String.Format("{0}://{1}/{2}", protocal, GetIpcName(), ipc_name));
        }

        public object FindApiObject(Type obj, string ipc_name = "")// where T : MarshalByRefObject
        {
            string protocal = "ipc";
            if (GetIpcMode() == IpcMode.Tcp)
                protocal = "tcp";
            else if (GetIpcMode() == IpcMode.Http)
                protocal = "http";

            if (String.IsNullOrEmpty(ipc_name))
                ipc_name = obj.FullName;

            return Activator.GetObject(obj,
                String.Format("{0}://{1}/{2}", protocal, GetIpcName(), ipc_name));
        }


        public abstract void Start();

        public abstract void Stop();

    }
}
