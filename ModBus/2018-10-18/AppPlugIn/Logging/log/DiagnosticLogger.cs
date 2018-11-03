using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging.log
{
    internal class DiagnosticLogger : ILogger
    {
        public LoggerType GetLoggerType()
        {
            return LoggerType.Trace;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Write(Model.Args.LogMessageEventArgs args)
        {
            string msg = string.Concat(args.TimeTicks, " ", args.Tag, " ", args.Value);
            switch (args.LogType)
            {
                case LogMessageType.Error:
                    System.Diagnostics.Trace.TraceError(msg);
                    break;
                case LogMessageType.Warning:
                    System.Diagnostics.Trace.TraceWarning(msg);
                    break;
                case LogMessageType.Information:
                    System.Diagnostics.Trace.TraceInformation(msg);
                    break;
                default:
                    System.Diagnostics.Trace.WriteLine(msg);
                    break;
            }
        }

        public void Write(string msg, string tag, LogMessageType msgtype)
        {
            switch (msgtype)
            {
                case LogMessageType.Error:
                    System.Diagnostics.Trace.TraceError(msg);
                    break;
                case LogMessageType.Warning:
                    System.Diagnostics.Trace.TraceWarning(msg);
                    break;
                case LogMessageType.Information:
                    System.Diagnostics.Trace.TraceInformation(msg);
                    break;
                default:
                    System.Diagnostics.Trace.WriteLine(msg);
                    break;
            }
        }
    }
}
