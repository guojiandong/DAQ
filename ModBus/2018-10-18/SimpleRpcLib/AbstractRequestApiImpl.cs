using System;
using System.Runtime.InteropServices;

namespace Ksat.SimpleRpcLib
{
    [Guid("6EA24000-AFE7-4DAB-A3C0-EE43E665A501")]
    public abstract class AbstractRequestApiImpl : Ipc.AbstractRemoteApi, IpcRequestApi
    {
        public abstract ResponseEventArgs SendMessage(RequestEventArgs message);
    }
}
