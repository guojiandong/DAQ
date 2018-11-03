using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging
{
    [Guid("E636E601-F7A1-49F3-A743-CE5FCC3C0A4E")]
    public class Log
    {
        public string Tag { get; set; }

        public Log(Type obj) : this(obj.Name)
        {
        }

        public Log(string tag)
        {
            this.Tag = tag;
        }

        public void Debugf(String format, params object[] args)
        {
            Debug(String.Format(format, args));
        }

        public void Debug(string message)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Debug, Tag, message);
        }

        

        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Error(string message, Exception ex)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Error, Tag, message, ex);
        }


        public void Infof(String format, params object[] args)
        {
            Info(String.Format(format, args));
        }

        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        public void Info( string message)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Information, Tag, message);
        }



        public void Warnf(String format, params object[] args)
        {
            Warn(String.Format(format, args));
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Warning, Tag, message);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message, Exception ex)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Warning, Tag, message, ex);
        }
    }
}
