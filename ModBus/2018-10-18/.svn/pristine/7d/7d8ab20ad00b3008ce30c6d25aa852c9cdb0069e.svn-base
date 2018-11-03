using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Ipc
{
    [Guid("A2E1B211-F9CE-467B-B456-8CE9C8A264FE")]
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
    }

    [Guid("AC64F70E-28D9-4E7C-86C6-14E83C9A83CD")]
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

    [Guid("2C4D0B97-21BE-48C9-ADDC-2FC4DB41407B")]
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

            return (T)Activator.GetObject(typeof(T),
                String.Format("{0}://{1}/{2}", protocal, GetIpcName(), ipc_name));
        }

        public object FindApiObject(Type obj, string ipc_name = "")// where T : MarshalByRefObject
        {
            string protocal = "ipc";
            if (GetIpcMode() == IpcMode.Tcp)
                protocal = "tcp";

            if (String.IsNullOrEmpty(ipc_name))
                ipc_name = obj.FullName;

            return Activator.GetObject(obj,
                String.Format("{0}://{1}/{2}", protocal, GetIpcName(), ipc_name));
        }


        public abstract void Start();

        public abstract void Stop();

    }
}
