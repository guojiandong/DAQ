using Ksat.AppPlugIn.Common.Attr;
using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;


namespace Ksat.SqliteHelper
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class SqlTable : SqlCommandHelper
    {
        private string mTableName;

        private string mCreateSql;

        internal SqlTable(string name, string connstr)
            : base(connstr)
        {
            mTableName = name;
            mCreateSql = "";
        }

        public string GetTableName()
        {
            return mTableName;
        }

        public bool IsExisting()
        {
            int count = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM SQLITE_MASTER WHERE name='" + GetTableName() + "'"));
            if (count > 0)
                return true;

            return false;
        }

        public string GetTableSql()
        {
            return (string)ExecuteScalar("SELECT sql FROM SQLITE_MASTER WHERE name='" + GetTableName() + "'");
        }

        public int GetRecordCount()
        {
            return Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM " + GetTableName()));
        }

        #region create table
        public int CheckAndAlertTable<T>()
        {
            return CheckAndAlertTable(typeof(T));
        }

        public int CheckAndAlertTable(Type t)
        {
            String sql = this.GetTableSql();

            List<ColumnInfo> list = getTableColumns(t);


            if (!String.IsNullOrEmpty(sql))
            {
                sql = sql.Substring(sql.IndexOf("(") + 1);

                sql = sql.Substring(0, sql.IndexOf(")"));


                if (!String.IsNullOrEmpty(sql))
                {
                    string[] columns = sql.ToLower().Split(',');
                    if (columns != null && columns.Length > 0)
                    {
                        foreach (string col in columns)
                        {
                            foreach (ColumnInfo item in list)
                            {
                                if (col.TrimStart().StartsWith(item.Name.ToLower()))
                                {
                                    list.Remove(item);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if(list.Count > 0)
            {
                foreach (ColumnInfo item in list)
                {
                    this.AddColumn(item.Name, item.ColumnType);
                }

                return list.Count;
            }

            return 0;
        }

        public int CreateTable<T>()
        {
            return CreateTable(typeof(T));
        }

        public int CreateTable(Type t)
        {
            if (IsExisting())
            {
                CheckAndAlertTable(t);

                return 0;
            }

            try
            {
                return CreateTable(getTableColumns(t).ToArray());
            }
            catch (Exception ex)
            {
                
            }

            return -1;
        }

        public static string CreateFullTextCreateTableStatement(Type objectWithProperties)
        {
            var sbColumns = StringBuilderCache.Allocate();
            sbColumns.AppendLine("ID INTEGER PRIMARY KEY AUTOINCREMENT");
            foreach (var propertyInfo in objectWithProperties.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
#if true
                var columnDefinition = $", {propertyInfo.Name} TEXT";

                sbColumns.AppendLine(columnDefinition);
#else
                var columnDefinition = (sbColumns.Length == 0)
                    ? $"{propertyInfo.Name} TEXT PRIMARY KEY"
                    : $", {propertyInfo.Name} TEXT";

                sbColumns.AppendLine(columnDefinition);
#endif
            }

            var tableName = objectWithProperties.GetType().Name;
            var sql = $"CREATE VIRTUAL TABLE \"{tableName}\" USING FTS3 ({StringBuilderCache.ReturnAndFree(sbColumns)});";

            return sql;
        }

        private List<ColumnInfo> getTableColumns(Type t)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();

            PropertyInfo[] properties = t.GetProperties();// (BindingFlags.Public | BindingFlags.CreateInstance);
            foreach (PropertyInfo property in properties)
            {
                if (!property.CanRead)
                {
                    //Logger.Warn("getTableColumns(), name:" + property.Name
                    //    + ", property is private, will ignore, CanRead:" + property.CanRead
                    //    + ", CanWrite:" + property.CanWrite);

                    continue;
                }

                ColumnInfo col = new ColumnInfo(property.Name);
                //col.Name = property.Name;
                col.ColumnType = SqlHelper.GetSqliteDataType(property.PropertyType);
                object[] attrs = property.GetCustomAttributes(true);
                if (attrs != null && attrs.Length > 0)
                {
                    foreach (object att in attrs)
                    {
                        if (att is ColumnIgnoreAttribute)
                        {
                            col.Name = "";
                            break;
                        }
                        else if (att is ColumnAttribute)
                        {
                            col.Name = ((ColumnAttribute)att).Name;
                            if (String.IsNullOrEmpty(col.Name))
                                col.Name = property.Name;
                            //else
                            //    data_type = ((ColumnAttribute)att).GetDataType();
                            break;
                        }
                        else if(att is PrimaryKeyAttribute)
                        {
                            col.PrimaryKey = true;
                        }
                    }
                }

                if (String.IsNullOrEmpty(col.Name))
                    continue;

                list.Add(col);
            }

            return list;
        }

        //public int CreateTable(params SQLiteParameter[] columns)
        //{
        //    StringBuilder sql = new StringBuilder();
        //    sql.Append("CREATE TABLE ");
        //    sql.Append(GetTableName()).Append(" ");

        //    sql.Append("( ");

        //    sql.Append("ID INTEGER PRIMARY KEY AUTOINCREMENT ");

        //    List<string> columnList = new List<string>();
        //    columnList.Add("ID");

        //    foreach (SQLiteParameter item in columns)
        //    {
        //        sql.Append(" ,").Append(item.ParameterName).Append(" ").Append(item.DbType);

        //        columnList.Add(item.ParameterName);
        //    }

        //    sql.Append(" )");

        //    Logger.Info("CreateTable() sql:" + sql.ToString());
        //    try
        //    {
        //        int result = ExecuteNonQuery(sql.ToString());
        //        //if(result >= 0)
        //        //{
        //        //    mColumnName.Clear();
        //        //    mColumnName.AddRange(columnList);
        //        //}
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("CreateTable() create table exception:", ex);
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="columns">KeyValuePair<string, string> Key : Column name, Value : Column Type</param>
        /// <returns></returns>
        public int CreateTable(params ColumnInfo[] columns)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("CREATE TABLE ");
            sql.Append(GetTableName()).Append(" ");

            sql.Append("( ");

            sql.Append("ID INTEGER PRIMARY KEY AUTOINCREMENT ");

            List<string> columnList = new List<string>();
            columnList.Add("ID");

            foreach (ColumnInfo v in columns)
            {
                sql.Append(" ,").Append(v.Name).Append(" ").Append(v.ColumnType);

                columnList.Add(v.Name);
            }

            sql.Append(" )");

            Logger.Info("CreateTable() sql:" + sql.ToString());
            try
            {
                int result = ExecuteNonQuery(sql.ToString());

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("CreateTable() create table exception:", ex);
                throw ex;
            }
        }

        //public int CreateTable(params ColumnInfo[] columns)
        //{
        //    StringBuilder sql = new StringBuilder();
        //    sql.Append("CREATE TABLE ");
        //    sql.Append(GetTableName()).Append(" ");

        //    sql.Append("( ");

        //    sql.Append("ID INTEGER PRIMARY KEY AUTOINCREMENT ");

        //    List<string> columnList = new List<string>();
        //    columnList.Add("ID");

        //    foreach (KeyValuePair<string, SqliteDataType> v in columns)
        //    {
        //        sql.Append(" ,").Append(v.Key).Append(" ").Append(SqlHelper.GetSqliteDataType(v.Value));

        //        columnList.Add(v.Key);
        //    }

        //    sql.Append(" )");

        //    try
        //    {
        //        int result = ExecuteNonQuery(sql.ToString());
        //        //if (result >= 0)
        //        //{
        //        //    mColumnName.Clear();
        //        //    mColumnName.AddRange(columnList);
        //        //}
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("CreateTable() create table exception:", ex);
        //        throw ex;
        //    }
        //}
#endregion

        ///// <summary>
        ///// Query the built-in sqlite table_info table for a specific tables columns.
        ///// </summary>
        ///// <returns>The columns contains in the table.</returns>
        ///// <param name="tableName">Table name.</param>
        //public List<ColumnInfo> GetTableInfo(string tableName)
        //{
        //    var query = "pragma table_info(\"" + tableName + "\")";

        //    SQLiteDataReader reader = base.ExecuteReader(query, null);
        //    if(reader != null && reader.HasRows){
                
        //    }

        //    return null; //Get<ColumnInfo>(query);
        //}

        //void MigrateTable(TableMapping map, List<ColumnInfo> existingCols)
        //{
        //    var toBeAdded = new List<TableMapping.Column>();

        //    foreach (var p in map.Columns)
        //    {
        //        var found = false;
        //        foreach (var c in existingCols)
        //        {
        //            found = (string.Compare(p.Name, c.Name, StringComparison.OrdinalIgnoreCase) == 0);
        //            if (found)
        //                break;
        //        }
        //        if (!found)
        //        {
        //            toBeAdded.Add(p);
        //        }
        //    }

        //    foreach (var p in toBeAdded)
        //    {
        //        var addCol = "alter table \"" + map.TableName + "\" add column " + Orm.SqlDecl(p, StoreDateTimeAsTicks);
        //        Execute(addCol);
        //    }
        //}
        public int RenameTableName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return -1;
            }

            if (name.Equals(GetTableName()))
            {
                return 0;
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("ALTER TABLE ");
            sql.Append(GetTableName()).Append(" ");

            sql.Append(" RENAME TO ");

            sql.Append(name);

            Logger.Info("RenameTableName() sql:" + sql.ToString());
            try
            {
                int ret = ExecuteNonQuery(sql.ToString());
                if (ret == 0)
                {
                    mTableName = name;
                }

                return ret;
            }
            catch (Exception ex)
            {
                Logger.Error("CreateTable() create table exception:", ex);
                throw ex;
            }
        }

        public int AddColumn(string column_name, SqliteDataType data_type)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("ALTER TABLE ");
            sql.Append(GetTableName()).Append(" ");

            sql.Append(" ADD COLUMN ");

            sql.Append(" ").Append(column_name);

            switch (data_type)
            {
                case SqliteDataType.INTEGER:
                case SqliteDataType.TINYINT:
                case SqliteDataType.MEDIUMINT:
                case SqliteDataType.BIGINT:
                case SqliteDataType.REAL:
                case SqliteDataType.TEXT:
                case SqliteDataType.BLOB:
                case SqliteDataType.NUMERIC:
                    sql.Append(" ").Append(data_type.ToString());
                    break;
                default:
                    sql.Append(" ").Append("TEXT");
                    break;
            }

            try
            {
                int result = ExecuteNonQuery(sql.ToString());
                //if (result >= 0)
                //{
                //    mColumnName.Add(column_name);
                //}
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("CreateTable() create table exception:", ex);
                throw ex;
            }
        }

        public int Delete(params int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return -1;
            }

            List<SQLiteParameter[]> parameters = new List<SQLiteParameter[]>();
            foreach (int id in ids)
            {
                parameters.Add(new SQLiteParameter[] { new SQLiteParameter("@ID", id) });
            }

            string sql = "DELETE FROM " + GetTableName() + " WHERE  ID=@ID";

            return ExecuteBatchQuery(sql, parameters);
        }


//        public int Insert<T>(T data)
//        {
//            return Insert(typeof(T), data);
//        }

//        public int Insert(Type t, object data)
//        {
//            if (!IsExisting())
//            {
//                return -1;
//            }

//            PropertyInfo[] properties = t.GetProperties();

//            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

//            StringBuilder strSqlKey = new StringBuilder();
//            StringBuilder strSqlValue = new StringBuilder();


//            foreach (PropertyInfo property in properties)
//            {
//                if (!property.CanRead)
//                {
//                    Logger.Warn("getTableColumns(), name:" + property.Name
//                        + ", property is private, will ignore, CanRead:" + property.CanRead
//                        + ", CanWrite:" + property.CanWrite);

//                    continue;
//                }

//                string name = property.Name;
//                object[] attrs = property.GetCustomAttributes(true);
//                if (attrs != null && attrs.Length > 0)
//                {
//                    foreach (object att in attrs)
//                    {
//                        if (att is ColumnIgnore)
//                        {
//                            name = "";
//                            break;
//                        }
//                        else if (att is ColumnName)
//                        {
//                            name = ((ColumnName)att).GetName();
//                            if (String.IsNullOrEmpty(name))
//                                name = property.Name;
//                            break;
//                        }
//                    }
//                }

//                if (String.IsNullOrEmpty(name)) continue;

//                parameters.Add(new SQLiteParameter("@" + name, property.GetValue(data, null)));

//                //Logger.Info("Insert(5), Name:"+ property.Name+", Value:"+ property.GetValue(data, null));
//                if (strSqlKey.Length > 0)
//                    strSqlKey.Append(" ,");
//                strSqlKey.Append(name);

//                if (strSqlValue.Length > 0)
//                    strSqlValue.Append(" ,");
//                strSqlValue.Append("@").Append(name);

//                //if (property.Name.Equals("CreateTime"))
//                //{
//                //    isContentCreateTime = true;
//                //}
//            }

//            //if (!isContentCreateTime)
//            //{
//            //    parameters[properties.Length] = new SQLiteParameter("@CreateTime", DateTime.Now.Ticks);

//            //    if (strSqlKey.Length > 0)
//            //        strSqlKey.Append(" ,").Append("CreateTime");

//            //    if (strSqlValue.Length > 0)
//            //        strSqlValue.Append(" ,").Append("@").Append("CreateTime");
//            //}

//            StringBuilder sql = new StringBuilder();
//            sql.Append("INSERT INTO ").Append(GetTableName());

//            sql.Append(" (");
//            sql.Append(strSqlKey);
//            sql.Append(" )");

//            sql.Append(" VALUES ");

//            sql.Append(" (");
//            sql.Append(strSqlValue);
//            sql.Append(" )");
//#if DEBUG
//            Logger.Info("Insert(), sql:" + sql.ToString());
//#endif
//            return ExecuteNonQuery(sql.ToString(), parameters.ToArray());
//        }

//        public int InsertBatch(Type t, IList<object> data)
//        {
//            if (!IsExisting())
//            {
//                return -1;
//            }

//            if (data == null || data.Count == 0)
//            {
//                return -2;
//            }

//            //List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, SqliteDataType>>();

//            PropertyInfo[] properties = t.GetProperties();

//            StringBuilder strSqlKey = new StringBuilder();
//            StringBuilder strSqlValue = new StringBuilder();

//            LinkedList<KeyValuePair<PropertyInfo, string>> prop_name_list = new LinkedList<KeyValuePair<PropertyInfo, string>>();

//            foreach (PropertyInfo property in properties)
//            {
//                string name = property.Name;
//                object[] attrs = property.GetCustomAttributes(true);
//                if (attrs != null && attrs.Length > 0)
//                {
//                    foreach (object att in attrs)
//                    {
//                        if (att is ColumnIgnore)
//                        {
//                            name = "";
//                            break;
//                        }
//                        else if (att is ColumnName)
//                        {
//                            name = ((ColumnName)att).GetName();
//                            if (String.IsNullOrEmpty(name))
//                                name = property.Name;
//                            break;
//                        }
//                    }
//                }

//                if (String.IsNullOrEmpty(name)) continue;

//                prop_name_list.AddLast(new KeyValuePair<PropertyInfo, string>(property, name));

//                //parameters.Add(new SQLiteParameter("@" + name, property.GetValue(data, null)));


//                //Logger.Info("Insert(5), Name:"+ property.Name+", Value:"+ property.GetValue(data, null));
//                if (strSqlKey.Length > 0)
//                    strSqlKey.Append(" ,");
//                strSqlKey.Append(name);

//                if (strSqlValue.Length > 0)
//                    strSqlValue.Append(" ,");
//                strSqlValue.Append("@").Append(name);
//            }

//            List<SQLiteParameter[]> parameters = new List<SQLiteParameter[]>();
//            foreach (object obj in data)
//            {
//                if (obj == null)
//                    continue;

//                SQLiteParameter[] prop_params = new SQLiteParameter[prop_name_list.Count];
//                int i = 0;

//                LinkedListNode<KeyValuePair<PropertyInfo, string>> nodeNow = prop_name_list.First;
//                while (nodeNow != null)
//                {
//                    //parameters.Add(new SQLiteParameter("@" + name, property.GetValue(data, null)));
//                    KeyValuePair<PropertyInfo, string> item = nodeNow.Value;
//                    prop_params[i] = new SQLiteParameter("@" + item.Value, item.Key.GetValue(obj, null));
//                    i++;

//                    nodeNow = nodeNow.Next;
//                }

//                parameters.Add(prop_params);
//            }

//            StringBuilder sql = new StringBuilder();
//            sql.Append("INSERT INTO ").Append(GetTableName());

//            sql.Append(" (");
//            sql.Append(strSqlKey);
//            sql.Append(" )");

//            sql.Append(" VALUES ");

//            sql.Append(" (");
//            sql.Append(strSqlValue);
//            sql.Append(" )");
//#if DEBUG
//            Logger.Info("Insert(), sql:" + sql.ToString());
//#endif
//            return ExecuteBatchQuery(sql.ToString(), parameters);
//        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <returns></returns>
        public int DropTable()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DROP TABLE ");
            sql.Append(GetTableName());

            try
            {
                return ExecuteNonQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error("DropTable() create table exception:", ex);
                throw ex;
            }
        }
    }
}
