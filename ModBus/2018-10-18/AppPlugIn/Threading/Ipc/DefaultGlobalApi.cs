using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Ipc
{
    [Guid("C72FB727-EB83-430B-8406-9AFC58642D6F")]
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
