using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging.log
{
    [Flags]
    public enum LogMessageType
    {
        
        /// <summary>  
        /// information type  
        /// </summary>  
        Information = (1<<0),

        /// <summary>  
        /// Debug
        /// </summary>  
        Debug = (1 << 2),

        /// <summary>  
        /// warning type  
        /// </summary>  
        Warning = (1 << 3),

        /// <summary>  
        /// error type  
        /// </summary>  
        Error = (1 << 4)
    }

    public sealed class LogMessageTypeHelper
    {
        public static string GetSimpleName(LogMessageType t)
        {
            switch (t)
            {
                case LogMessageType.Information:
                    return "I";
                case LogMessageType.Debug:
                    return "D";
                case LogMessageType.Warning:
                    return "W";
                case LogMessageType.Error:
                    return "E";
            }

            return "I";
        }
    }
}
