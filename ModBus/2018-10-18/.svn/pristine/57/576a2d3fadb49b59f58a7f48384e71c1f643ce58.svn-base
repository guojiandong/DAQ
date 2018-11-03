using Ksat.AppPlugIn.Model.Args;
using Ksat.AppPlugIn.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.LogMySql.Model
{
    public class LogMessageRecord
    {
        public LogMessageRecord()
        {
            this.TimeTicks = string.Empty;
            this.Tag = string.Empty;
            this.LogType = string.Empty;
            this.Message = string.Empty;
        }

        public LogMessageRecord(LogMessageEventArgs args)
        {
            this.TimeTicks = args.TimeTicks;
            this.Tag = args.Tag;
            this.LogType = args.LogType.ToString();
            this.Message = args.Value;
        }
        public string TimeTicks { get; set; }
        public string Tag { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("('{0}','{1}','{2}','{3}')", TimeTicks, Tag, LogType, Message);
        }
    }


}
