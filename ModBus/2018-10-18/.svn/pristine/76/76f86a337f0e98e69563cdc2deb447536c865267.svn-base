using System;
using System.Runtime.InteropServices;

namespace KsatRpcLibrary.Ipc
{
    [Guid("9E48CAED-8155-48CE-9D74-1D11B6D169CB")]
    [ComVisible(true)]
    public class DefaultGlobalApi : AbstractRemoteApi
    {
        public DefaultGlobalApi()
        {
        }

        public long GetCurrentTimeTick()
        {
            return DateTime.Now.Ticks;
        }

        public int CheckApi(int value)
        {
            return value;
        }
    }
}
