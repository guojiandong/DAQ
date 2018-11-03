using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Logging.log;

namespace Ksat.AppPlugIn.Model.Args
{
    public class LogMessageEventArgs : GenericEventArgs<string>
    {
        public readonly Logging.log.LogMessageType LogType;

        public readonly string Tag;

        public readonly string TimeTicks;

        public LogMessageEventArgs(LogMessageType logType, string tag, string ticks, string txt) : base(txt)
        {
            this.LogType = logType;
            this.Tag = tag;
            this.TimeTicks = ticks;
        }

        public override string ToString()
        {
            return String.Format("{0}  {1, -3} {2} {3}", this.TimeTicks,
                this.LogType.ToString().Substring(0, 1), this.Tag, this.Value);
        }
    }
}
