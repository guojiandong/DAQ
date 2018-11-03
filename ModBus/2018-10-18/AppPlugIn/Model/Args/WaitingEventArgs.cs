using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Args
{
    public class WaitingEventArgs : ObjectEventArgs
    {
        public readonly long StartTicks;

        /// <summary>
        /// Millisecond
        /// </summary>
        public readonly int Timeout;

        private readonly System.Threading.AutoResetEvent WaitEvent;

        public WaitingEventArgs(int timeout, object value) : base(value)
        {
            this.StartTicks = DateTime.UtcNow.Ticks;

            this.Timeout = timeout;// ConstDefine.GetTimeTicks() + timeout * TimeSpan.TicksPerMillisecond;

            this.WaitEvent = new System.Threading.AutoResetEvent(false);
        }

        public void Wait()
        {
            try
            {
                if (this.Timeout > 0)
                    this.WaitEvent.WaitOne(this.Timeout);
                else
                    this.WaitEvent.WaitOne();
            }
            catch { }
        }

        public virtual void Release()
        {
            this.WaitEvent.Set();
        }

        public void Lock()
        {
            this.WaitEvent.Reset();
        }
    }
}
