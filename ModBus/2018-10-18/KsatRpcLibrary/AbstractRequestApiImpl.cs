using System;
using System.Runtime.InteropServices;

namespace KsatRpcLibrary
{
    [Guid("E7692D5E-10FC-4EBF-A0CD-89EBF683076E")]
    public interface IpcActionRequestApi
    {
        ResponseEventArgs SendAction(RequestActionEventArgs message);
    }


    [Guid("6EA24000-AFE7-4DAB-A3C0-EE43E665A501")]
    public abstract class AbstractRequestApiImpl : Ipc.AbstractRemoteApi, IpcActionRequestApi
    {
        public abstract ResponseEventArgs SendAction(RequestActionEventArgs message);
    }
}
