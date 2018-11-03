using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.SqliteHelper
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class SqlDatabase : MarshalByRefObject
    {
        private string mDbName;

        private List<SqlTable> mCacheTables;

        public SqlDatabase()
            : this(null, SQLiteOpenFlags.ReadWrite)
        {
        }

        public SqlDatabase(string dbname, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = false)
        {
            mCacheTables = new List<SqlTable>();

            if (String.IsNullOrEmpty(dbname))
            {
                string appname = System.IO.Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName);
                string dir = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), appname);
                mDbName = Path.Combine(dir, "local_data_sqlite.db");

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            else if (Path.IsPathRooted(dbname))
            {
                string dir = System.IO.Path.GetDirectoryName(dbname);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            else
            {
                string appname = System.IO.Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName);
                string dir = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), appname);
                mDbName = Path.Combine(dir, dbname);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }

            Console.WriteLine(String.Format("SqlDatabase(), dbname:{0}, full:{1}", dbname, mDbName));
            //Logger.Info(String.Format("SqlDatabase(), dbname:{0}, full:{1}", dbname, mDbName));
        }

        public string GetDatabaseName()
        {
            return mDbName;
        }

        public void CreateDatabase()
        {
            if (File.Exists(mDbName))
            {
                //throw new FileNotFoundException(mDbName + " not existing.");

                return;
            }

            SQLiteConnection.CreateFile(mDbName);
        }

        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString(true)))
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();

                return data;
            }
        }

        public List<string> ShowTables()
        {
            if (!File.Exists(mDbName))
            {
                throw new FileNotFoundException("Database not existing, " + mDbName);
            }

            List<string> list = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString(true)))
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();
                
                string name;
                foreach (DataRow row in data.Rows)
                {
                    name = row["TABLE_NAME"].ToString();
                    if (name == "sqlite_sequence")
                    {
                        continue;
                    }
                    list.Add(name);
                }
            }
            
            return list;
        }

        public bool IsTableExisting(string table_name)
        {
#if true
            SqlCommandHelper helper = new SqlCommandHelper(GetConnectionString(true));
            int count = Convert.ToInt32(helper.ExecuteScalar("SELECT COUNT(*) FROM SQLITE_MASTER WHERE name='" + table_name + "'"));
            helper.Dispose();
            if (count > 0)
                return true;
#else
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString(true)))
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();

                foreach (DataRow row in data.Rows)
                {
                    string name = row["TABLE_NAME"].ToString();
                    if (name == table_name)
                    {
                        return true;
                    }
                }
            }
#endif
            return false;
        }
        
        public SqlTable SelectTable(Type tp)
        {
            string name = tp.Name;
            object[] attrs = tp.GetCustomAttributes(typeof(AppPlugIn.Common.Attr.TableNameAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                name = (attrs[0] as AppPlugIn.Common.Attr.TableNameAttribute).Name;

                if (String.IsNullOrEmpty(name))
                    name = tp.Name;
            }
            
            return SelectTable(name, false);
        }

        public SqlTable SelectTable<T>()
        {
            return SelectTable(typeof(T));
        }

        public SqlTable SelectTable(string table_name)
        {
            return SelectTable(table_name, false);
        }

        public SqlTable SelectTable(string table_name, bool onlyread)
        {
            if (!File.Exists(mDbName))
            {
                throw new FileNotFoundException("Database not existing, " + mDbName);
            }

            if (String.IsNullOrEmpty(table_name))
            {
                throw new ArgumentNullException("Table name can't empty...");
            }

            lock (mCacheTables)
            {
                foreach (SqlTable item in mCacheTables)
                {
                    if (item.GetTableName() == table_name)
                    {
                        return item;
                    }
                }

                SqlTable tb = new SqlTable(table_name, GetConnectionString(onlyread));

                mCacheTables.Add(tb);
                return tb;
            }
        }

        public string GetConnectionString()
        {
            return GetConnectionString(false);
        }

        public string GetConnectionString(bool readOnly)
        {
            SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            connstr.DataSource = mDbName;
            connstr.Version = 3;
            connstr.ReadOnly = readOnly;

            return connstr.ToString();
        }
    }
}
