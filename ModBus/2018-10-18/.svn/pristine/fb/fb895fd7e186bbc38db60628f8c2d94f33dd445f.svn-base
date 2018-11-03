using Ksat.AppPlugIn.Communicate.SuperIoc.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Net
{
    public interface ISocketListener : ISession
    {
        void StartListen();

        ISocketSession StartAccept();


        /// <summary>
        /// Port
        /// </summary>
        int ListenPort { get; }

        /// <summary>
        /// Port
        /// </summary>
        int MaxListenCount { get; }
    }
}
