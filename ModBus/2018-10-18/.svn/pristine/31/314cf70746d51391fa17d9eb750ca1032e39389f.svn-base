using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Threading.Listener
{
    [Guid("A41A9513-5BAF-46BA-A299-3824A22BDAB3")]
    public enum NotifyDelayMode : ushort
    {
        RealTime = 0,
        Fastest,
        Medium,
        Normal,
        Slow
    }

    [Guid("A9FC5520-2571-4208-9F61-ADE5B7E1AA2D")]
    public class NotifyDelayTime
    {
        /// <summary>
        /// Millisecond
        /// </summary>
        /// <param name="rate"></param>
        /// <returns>Millisecond</returns>
        public static ushort GetDelayTime(NotifyDelayMode rate)
        {
            ushort delay = 0;
            switch (rate)
            {
                case NotifyDelayMode.Slow:
                    delay = 300;
                    break;
                case NotifyDelayMode.Normal:
                    delay = 200;
                    break;
                case NotifyDelayMode.Medium:
                    delay = 100;
                    break;
                case NotifyDelayMode.Fastest:
                    delay = 50;
                    break;
                case NotifyDelayMode.RealTime:
                    delay = 0;
                    break;
                default:
                    delay = (ushort)rate;
                    break;
            }

            return delay;
        }
    }
}
