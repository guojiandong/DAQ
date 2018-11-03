using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging
{
    [Guid("1BFDC823-31B2-486E-88FF-F0C4D94C94D2")]
    public sealed class Logger
    {
        public static readonly string Tag = "NoTag";
        public static ThreadPriority LogPriority = ThreadPriority.BelowNormal;
        internal static string mLogPath = Directory.GetDirectoryRoot(System.Environment.CurrentDirectory);

        public static void SetLoggerPath(string path)
        {
            if(!String.IsNullOrEmpty(path))
                mLogPath = path;
        }

        public static void SetHolding_Log_Day(int day)
        {
            log.LoggerImpl.Instance().SetHolding_Log_Day(day);
        }

        /// <summary>
        /// enable logs
        /// </summary>
        /// <param name="logs"></param>
        public static void EnableLogger(params LoggerType[] logs)
        {
            foreach (LoggerType t in logs)
            {
                log.LoggerImpl.Instance().SetLoggerType(t, true);
            }

        }

        /// <summary>
        /// disable logs
        /// </summary>
        /// <param name="logs"></param>
        public static void DisableLogger(params LoggerType[] logs)
        {
            foreach (LoggerType t in logs)
            {
                log.LoggerImpl.Instance().SetLoggerType(t, false);
            }
        }


        public static void RegisterLogger(log.ILogger logger)
        {
            if (logger == null) return;

            log.LoggerImpl.Instance().RegisterLogger(logger);
        }

        public static void UnRegisterLogger(log.ILogger logger)
        {
            if (logger == null) return;

            log.LoggerImpl.Instance().UnRegisterLogger(logger);
        }

        public static void SetLoggerFilter(log.LogMessageType t)
        {
            log.LoggerImpl.Instance().SetLoggerFilter(t);
        }

        public static void Dispose()
        {
            log.LoggerImpl.Instance().Dispose();
        }

        public static void Debugf(string tag, String format, params object[] args)
        {
            Debug(tag, String.Format(format, args));
        }

        public static void Debug(string message)
        {
            Debug(Tag, message);
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string tag, string message)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Debug, tag, message);
        }

        public static void Error(string message, Exception ex)
        {
            Error(Tag, message, ex);
        }
        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string tag, string message, Exception ex)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Error, tag, message, ex);
        }

        public static void Infof(string tag, String format, params object[] args)
        {
            Info(tag, String.Format(format, args));
        }

        public static void Info(string message)
        {
            Info(Tag, message);
        }

        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string tag, string message)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Information, tag, message);
        }

        public static void Warn(string message)
        {
            Warn(Tag, message);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string tag, string message)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Warning, tag, message);
        }

        public static void Warn(string message, Exception ex)
        {
            Warn(Tag, message, ex);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string tag, string message, Exception ex)
        {
            log.LoggerImpl.Instance().Write(log.LogMessageType.Warning, tag, message, ex);
        }        
    }
}
