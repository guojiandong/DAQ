
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model
{
    [Guid("9180392C-B887-4C36-BF51-578CEE6A8AB5")]
    [Serializable]
    public abstract class AbstractDataStoreBase
    {
        public AbstractDataStoreBase()
        {
            this.CreateDate = DateTime.UtcNow.Ticks;// Utils.DateHelper.ToMillisecondsSinceEpoch(DateTime.UtcNow);

            this.UploadDate = 0;
        }

        public AbstractDataStoreBase(AbstractDataStoreBase other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(AbstractDataStoreBase other)
        {
            this.CreateDate = other.CreateDate;
            this.UploadDate = other.UploadDate;
        }

        public abstract AbstractDataStoreBase Clone();

        public virtual string GetTableName()
        {

            object[] attrs = this.GetType().GetCustomAttributes(typeof(Common.Attr.TableNameAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {

                string name = (attrs[0] as Common.Attr.TableNameAttribute).Name;

                if (String.IsNullOrEmpty(name))
                    name = this.GetType().Name;

                return name;
            }
            else
            {
                return this.GetType().Name;
            }
        }

        public virtual IList<string> GetColumnNames()
        {
            List<string> cols = new List<string>();

            System.Reflection.PropertyInfo[] properties = this.GetType().GetProperties();// (BindingFlags.Public | BindingFlags.CreateInstance);
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (!property.CanRead/* || !property.CanWrite*/)
                {
                    Logging.Logger.Warn("GetColumnNames(), name:" + property.Name
                        + ", property is private, will ignore, CanRead:" + property.CanRead
                        + ", CanWrite:" + property.CanWrite);

                    continue;
                }

                string name = property.Name;

                object[] attrs = property.GetCustomAttributes(true);
                if (attrs != null && attrs.Length > 0)
                {
                    foreach (object att in attrs)
                    {
                        if (att is Common.Attr.ColumnIgnoreAttribute)
                        {
                            name = "";
                            break;
                        }

                        if (att is Common.Attr.ColumnAttribute)
                        {
                            name = ((Common.Attr.ColumnAttribute)att).Name;
                            if (String.IsNullOrEmpty(name))
                                name = property.Name;
                            break;
                        }
                    }
                }
#if DEBUG
                Logging.Logger.Info("GetColumnNames(2), column name:" + name + ", property.Name:" + property.Name);
#endif
                if (String.IsNullOrEmpty(name))
                    continue;

                cols.Add(name);
            }

            return cols;
        }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public long CreateDate { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public long UploadDate { get; set; }
    }
}
