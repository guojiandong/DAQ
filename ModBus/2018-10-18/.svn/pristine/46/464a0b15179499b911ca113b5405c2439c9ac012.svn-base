using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;

namespace Ksat.SqliteHelper
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class SqlCommandHelper : MarshalByRefObject, IDisposable
    {
        private string mConnString;

        public string ConnectionString { get { return mConnString; } set { mConnString = value; } }

        public SqlCommandHelper(string connstr)
        {
            mConnString = connstr;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public int ExecuteNonQuery(string sql)
        {
            int result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;
                    result = command.ExecuteNonQuery();
                }
                connection.Close();
            }

            return result;
        }

        // <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public int ExecuteNonQuery(string sql, CommandBehavior behavior)
        {
            int result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;
                    result = command.ExecuteNonQuery(behavior);
                }
                connection.Close();
            }
            return result;
        }
        /// <summary> 
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。 
        /// </summary> 
        /// <param name="sql">要执行的增删改的SQL语句</param> 
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sql;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        affectedRows = command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
            return affectedRows;
        }

        public int ExecuteBatchQuery(string sql, List<SQLiteParameter[]> parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sql;
                        if (parameters != null)
                        {
                            foreach (SQLiteParameter[] item in parameters)
                            {
                                command.Parameters.Clear();
                                command.Parameters.AddRange(item);
                                affectedRows += command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            affectedRows = command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
            return affectedRows;
        }


        /// <summary> 
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(ConnectionString);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }


        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }

        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }

        /// <summary> 
        /// 执行一个查询语句，返回查询结果的第一行第一列 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns>返回第一行第一列值</returns> 
        public object ExecuteScalar(string sql, SQLiteParameter[] parameters)
        {
            object result = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    result = command.ExecuteScalar();
                }
                connection.Close();
            }
            return result;
        }

        /// <summary> 
        /// 查询数据库中的所有数据表信息 
        /// </summary> 
        /// <returns></returns> 
        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();
                //foreach (DataColumn column in data.Columns) 
                //{ 
                //  Console.WriteLine(column.ColumnName); 
                //} 
                return data;
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        #region 

        /// <summary>
        /// 常量；
        /// </summary>
        const string INSERT_TABLE_ITEM_VALUE = "INSERT INTO {0} ({1}) VALUES ({2})";
        const string DELETE_TABLE_WHERE = "DELETE FROM {0} WHERE {1}";
        const string UPDATE_TABLE_EDITITEM = "UPDATE {0} SET {1}";
        const string UPDATE_TABLE_EDITITEM_WHERE = "UPDATE {0} SET {1} WHERE {2}";
        const string Query_ITEM_TABLE_WHERE = "SELECT {0} FROM {1} WHERE {2}";

        /// <summary>
        /// 1.1 新增实体；
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="model">实体</param>
        /// <param name="autoPrimaryKey">自增主键名称</param>
        /// <returns></returns>
        public long Insert<T>(T model, string autoPrimaryKey = "id") where T : class
        {
            long result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                result = connection.Insert<T>(model);

                connection.Close();
            }

            return result;
            //var insertSql = GetInsertSql<T>(model, autoPrimaryKey);
            //return (int)conn.Insert<T>(model);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T">实休类</typeparam>
        /// <param name="addData">实体数据列表</param>
        /// <param name="autoPrimaryKey">自增主键名称</param>
        /// <returns></returns>
        public long Insert<T>(IList<T> models, string autoPrimaryKey = "id") where T : class
        {
#if true
            long result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                result = (int)connection.Insert<T>(models);

                connection.Close();
            }

            return result;
#else
            var type = typeof(T);
            int resultN = 0;
            var transaction = conn.BeginTransaction();
            try
            {
                models.ForEach(d =>
                {
                    var insertSql = GetInsertSql<T>(d);
                    resultN += conn.Execute(insertSql);
                });
                transaction.Commit();
            }
            catch (Exception)
            {
                resultN = 0;
                transaction.Rollback();
            }
            return resultN;
#endif
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="where">删除条件</param>
        /// <returns></returns>
        public bool Delete<T>(T value) where T : class
        {
#if true
            bool result = false;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                result = connection.Delete<T>(value);

                connection.Close();
            }

            return result;
#else
            var type = typeof(T);
            string sqlStr = string.Format(DELETE_TABLE_WHERE, type.Name, where);
            return conn.Execute(sqlStr);
#endif
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="where">删除条件</param>
        /// <returns></returns>
        public int Delete<T>(string where) where T : class
        {
            return Delete(typeof(T), where);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="where">删除条件</param>
        /// <returns></returns>
        public int Delete(Type t, string where) 
        {
            return Delete(t.Name, where);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete(string tableName, string where)
        {
#if true
            int result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                result = connection.Execute(string.Format(DELETE_TABLE_WHERE, tableName, where));

                connection.Close();
            }

            return result;
#else
            string sqlStr = string.Format(DELETE_TABLE_WHERE, tableName, where);
            return conn.Execute(sqlStr);
#endif
        }
        /// <summary>
        /// 修改; 
        /// </summary>
        /// <typeparam name="T">实体 Type </typeparam>
        /// <param name="model">实体</param>
        /// <param name="where">修改条件</param>
        /// <param name="attrs">要修改的实休属性数组</param>
        /// <returns></returns>
        public bool Update<T>(T model/*, string where, params string[] attrs*/) where T : class
        {
#if true
            bool result = false;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                result = connection.Update<T>(model);

                connection.Close();
            }

            return result;
#else
            var sqlStr = GetUpdateSql<T>(model, where, attrs);
            return conn.Execute(sqlStr);
#endif
        }
        #endregion
    }
}
