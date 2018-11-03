using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging.log
{
    public interface ILogger : IDisposable
    {
        //void Write(string msg, string tag, LogMessageType msgtype);
        void Write(Model.Args.LogMessageEventArgs args);

        LoggerType GetLoggerType();
    }
}
