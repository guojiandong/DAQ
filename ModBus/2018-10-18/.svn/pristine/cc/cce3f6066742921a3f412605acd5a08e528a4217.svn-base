using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.DataStore
{
    [Guid("4A36E3B9-B3FA-4104-B4C2-BBB77E0F9690")]
    [Serializable]
    public class LogMessageDataRecord : DataStoreBase
    {
        public LogMessageDataRecord()
        {
            this.LogType = Logging.log.LogMessageType.Information;
            this.Tag = String.Empty;
            this.Message = String.Empty;
            this.TimeTicks = this.CreateDate;
        }

        public Logging.log.LogMessageType LogType { get; set; }

        public string Tag { get; set; }

        public string Message { get; set; }

        public string TimeTicks { get; set; }

        public virtual string ToSqlString()
        {
            string tag = String.Empty;
            if (this.Tag != null)
            {
                if (this.Tag.Length > 50)
                    tag = this.Tag.Substring(0, 50);
                else
                    tag = this.Tag;
            }

            return String.Format("('{0}','{1}','{2}','{3}')", TimeTicks, tag, 
                Logging.log.LogMessageTypeHelper.GetSimpleName(this.LogType), Message);
        }
    }
}
