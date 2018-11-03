using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging.log
{
    public sealed class LogMessage
    {
        /// <summary>  
        /// Create Log message instance  
        /// </summary>  
        public LogMessage() : this("")
        {
        }

        public LogMessage(string text) : this(Logger.Tag, text)
        {
        }

        public LogMessage(string tag, string text) : this(LogMessageType.Information, tag, text)
        {
        }

        /// <summary>  
        /// Crete log message by message content and message type  
        /// </summary>  
        /// <param name="text">message content</param>  
        /// <param name="messageType">message type</param>  
        public LogMessage(LogMessageType messageType, string tag, string text)
            : this(messageType, tag, text, null)
        {
        }

        /// <summary>  
        /// Create log message by datetime and message content and message type  
        /// </summary>  
        /// <param name="dateTime">date time </param>  
        /// <param name="text">message content</param>  
        /// <param name="messageType">message type</param>  
        public LogMessage(LogMessageType messageType, string tag, string text, Exception ex)
        {
            Datetime = DateTime.Now;
            LogType = messageType;

            if(String.IsNullOrEmpty(tag))
                Tag = Logger.Tag;
            else
                Tag = tag;

            Text = text;
            this.Ex = ex;
        }

        /// <summary>  
        /// Gets or sets datetime  
        /// </summary>  
        public DateTime Datetime { get; private set; }

        /// <summary>  
        /// Gets or sets message content  
        /// </summary>  
        public string Tag { get; private set; }

        /// <summary>  
        /// Gets or sets message content  
        /// </summary>  
        public string Text { get; private set; }

        /// <summary>  
        /// Gets or sets message type  
        /// </summary>  
        public LogMessageType LogType { get; private set; }

        public Exception Ex { get; private set; }

        public Model.Args.LogMessageEventArgs ToMessage()
        {
            string str = Text;
            if (Ex != null)
            {
                str = string.Concat(str, Environment.NewLine, Ex.ToString());
            }

            return new Model.Args.LogMessageEventArgs(this.LogType, this.Tag, Datetime.ToString("yy-MM-dd HH:mm:ss.fff"), str);
        }

        /// <summary>  
        /// Get Message to string  
        /// </summary>  
        /// <returns></returns>  
        public override string ToString()
        {
            //string str = String.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3}  {7, -3} {8} {9}",
            //    Datetime.Year, Datetime.Month, Datetime.Day, Datetime.Hour, Datetime.Minute, Datetime.Second, Datetime.Millisecond,
            //    Type.ToString().Substring(0, 1), Tag, Text);

            string str = String.Format("{0}  {1, -3} {2} {3}", Datetime.ToString("yy-MM-dd HH:mm:ss.fff"),
                LogType.ToString().Substring(0, 1), Tag, Text);

            if (Ex != null)
            {
                return string.Concat(str, Environment.NewLine, Ex.ToString());
            }

            return str;
            //return Datetime.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\t" + Type + "\t" + Text;
        }
    }
}
