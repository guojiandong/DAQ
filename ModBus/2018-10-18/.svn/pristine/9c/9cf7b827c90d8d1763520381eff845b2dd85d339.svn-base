using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Args
{
    public class SyncInformationEventArgs : EventArgs
    {
        public readonly long StartTicks;

        /// <summary>
        /// Millisecond
        /// </summary>
        public readonly int Timeout;

        /// <summary>
        /// Millisecond
        /// </summary>
        public int TimeInterval;

        public object Value { get; set; }

        public SyncInformationEventArgs(int timeout)
        {
            this.StartTicks = DateTime.UtcNow.Ticks;
            this.Timeout = timeout;
            this.TimeInterval = 0;
        }

        public void SetResult(object value)
        {
            this.Value = value;
            this.TimeInterval = (int)((DateTime.UtcNow.Ticks - this.StartTicks) / TimeSpan.TicksPerMillisecond);
        }

        /// <summary>
        /// 获取时间差，Ms
        /// </summary>
        /// <returns></returns>
        public long GetTimeInterval()
        {
            if (this.TimeInterval == 0)
            {
                return (int)((DateTime.UtcNow.Ticks - this.StartTicks) / TimeSpan.TicksPerMillisecond);
            }

            return this.TimeInterval;
        }

        public bool IsTimeout()
        {
            if(this.TimeInterval == 0)
            {
                if ((int)((DateTime.UtcNow.Ticks - this.StartTicks) / TimeSpan.TicksPerMillisecond) > this.Timeout)
                    return true;
            }
            else if (this.TimeInterval > this.Timeout)
                return true;

            return false;
        }
    }
}
