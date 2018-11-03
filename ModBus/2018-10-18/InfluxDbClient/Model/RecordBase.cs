using Ksat.InfluxDbClient.Model.Attri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ksat.InfluxDbClient.Model
{
    [Serializable]
    public class RecordBase
    {
        [Ignore]
        public long SystemTime { get; set; }
        [Field]
        public long CreateTime { get; set; }
        [Field]
        public string TimeString { get; set; }

        public RecordBase()
        {
            this.SystemTime = 0;
            this.CreateTime = DateTime.Now.Ticks;
            this.TimeString = CreateTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }
        public virtual string GetInsertSqlString()
        {
            StringBuilder strbtag = new StringBuilder();
            StringBuilder strbfield = new StringBuilder();
            strbtag.AppendFormat("type={0}", this.GetType().Name);

            PropertyInfo[] propertyInfos = this.GetType().GetProperties();
            foreach (PropertyInfo info in propertyInfos)
            {
                IEnumerable<Attribute> attributes = info.GetCustomAttributes();
                bool tagFlag = false;
                bool feildFlag = false;
                bool ignoreFlag = false;
                foreach (Attribute attri in attributes)
                {
                    if (attri is TagAttribute)
                    {
                        tagFlag = true;
                        break;
                    }
                    else if (attri is FieldAttribute)
                    {
                        feildFlag = true;
                        break;
                    }
                    else if (attri is IgnoreAttribute)
                    {
                        ignoreFlag = true;
                        break;
                    }
                }
                if (!ignoreFlag)
                {
                    if (tagFlag)
                        strbtag.AppendFormat(",{0}={1}", info.Name, info.GetValue(this, null).ToString());
                    else if (feildFlag)
                    {
                        if (IsDigitalType(info.GetValue(this, null).GetType()))
                            strbfield.AppendFormat(",{0}={1}", info.Name, info.GetValue(this, null).ToString());
                        else
                            strbfield.AppendFormat(",{0}=\"{1}\"", info.Name, info.GetValue(this, null).ToString());
                    }
                }

            }
            if (strbfield.Length > 0)
                strbfield.Remove(0, 1);
            return strbtag.ToString() + " " + strbfield.ToString();
        }

        public bool IsDigitalType(Type type)
        {
            if (type.Equals(typeof(int)) || type.Equals(typeof(Int32)) || type.Equals(typeof(UInt32))
                || type.Equals(typeof(Int16)) || type.Equals(typeof(UInt16))
                || type.Equals(typeof(short)) || type.Equals(typeof(ushort))
                || type.Equals(typeof(Int64)) || type.Equals(typeof(UInt64))
                || type.Equals(typeof(long)) || type.Equals(typeof(ulong))
                || type.Equals(typeof(char)) || type.Equals(typeof(Char))
                || type.Equals(typeof(byte)) || type.Equals(typeof(Byte))
                || type.Equals(typeof(float)) || type.Equals(typeof(double)) || type.Equals(typeof(Double))
                || type.Equals(typeof(decimal)) || type.Equals(typeof(Decimal)) || type.Equals(typeof(Single))
                )
            {
                return true;
            }

            return false;
        }
    }
}
