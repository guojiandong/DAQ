using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging.log
{
    internal class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {

        }

        public LoggerType GetLoggerType()
        {
            return LoggerType.Console;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Write(Model.Args.LogMessageEventArgs args)
        {
            Console.WriteLine(string.Concat(args.TimeTicks, " ", 
                args.Tag, " ",
                args.LogType.ToString().Substring(0, 1), " ",
                args.Value));
        }

        public void Write(string msg, string tag, LogMessageType msgtype)
        {
            //Console.WriteLine(msg);
            Console.WriteLine(string.Concat(tag, msg));
        }
    }
}
