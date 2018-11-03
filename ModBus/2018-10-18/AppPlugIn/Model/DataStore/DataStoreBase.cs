using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ksat.AppPlugIn.Model.DataStore
{
    [Guid("0EC341E7-255F-4C3B-9021-3244F054C84A")]
    [Serializable]
    public class DataStoreBase : ICloneable
    {
        public DataStoreBase()
        {
            this.CreateDateTicks = DateTime.Now.Ticks;

            this.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            this.UploadDate = String.Empty;
            this.UploadDateTicks = 0L;
        }

        public DataStoreBase(DataStoreBase other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(DataStoreBase other)
        {
            System.Reflection.FieldInfo[] fields = other.GetType().GetFields(System.Reflection.BindingFlags.Public 
                | System.Reflection.BindingFlags.NonPublic 
                | System.Reflection.BindingFlags.Instance 
                | System.Reflection.BindingFlags.Static);

            if(fields != null)
            {
                foreach (System.Reflection.FieldInfo field in fields)
                {
                    try
                    {
                        field.SetValue(this, DeepCopy(field.GetValue(other)));
                    }
                    catch { }
                }
            }

            //this.CreateDate = other.CreateDate;
            //this.UploadDate = other.UploadDate;

            //this.CreateDateTicks = other.CreateDateTicks;
            //this.UploadDateTicks = other.UploadDateTicks;
        }

        //public abstract AbstractDataStoreBase Clone();

        public object Clone()
        {
            return this.MemberwiseClone();
        }

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
        public long CreateDateTicks { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public long UploadDateTicks { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public string UploadDate { get; set; }




        public static T DeepCopy<T>(T obj) 
        {
            object retval;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }
}
