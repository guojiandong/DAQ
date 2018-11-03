using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Tests
{
    public class Demo
    {

        public static void TestSqlite()
        {
            //1.基础连接，FailIfMissing 参数 true=没有数据文件将异常;false=没有数据库文件则创建一个  
            //Data Source=test.db;Pooling=true;FailIfMissing=false  
            //2。使用utf-8 格式  
            //Data Source={0};Version=3;UTF8Encoding=True;  
            //3.禁用日志  
            //Data Source={0};Version=3;UTF8Encoding=True;Journal Mode=Off;  
            //4.连接池  
            //Data Source=c:\mydb.db;Version=3;Pooling=True;Max Pool Size=100;  

            
            string conn = string.Format(@"Data Source={0};Pooling=true;FailIfMissing=false;Version=3;UTF8Encoding=True;Journal Mode=Off;",
                System.IO.Path.Combine(System.Reflection.Assembly.GetEntryAssembly().Location, "local_data_sqlite.db"));
        }
        

    }

    #region sqlite
#if false
            项目结构：
                ┌TestSqllite
                ├-x64
                │ └-SQLite.Interop.dll （右键属性，复制到输出目录）
                ├-x86
                │ └-SQLite.Interop.dll （右键属性，复制到输出目录）
                ├-dapper
                │ └-SqlMapper1.4.2.cs
                └-TestSqllite.cs


                发布后的结构：
                ┌bin\debug\
                ├-x64
                │ └-SQLite.Interop.dll
                ├-x86
                │ └-SQLite.Interop.dll
                └-System.Data.SQLite.dll
#endif
#if false
    public class SqliteDemo
    {
        private string _conn
        {
            get
            {

                //1.基础连接，FailIfMissing 参数 true=没有数据文件将异常;false=没有数据库文件则创建一个  
                //Data Source=test.db;Pooling=true;FailIfMissing=false  
                //2。使用utf-8 格式  
                //Data Source={0};Version=3;UTF8Encoding=True;  
                //3.禁用日志  
                //Data Source={0};Version=3;UTF8Encoding=True;Journal Mode=Off;  
                //4.连接池  
                //Data Source=c:\mydb.db;Version=3;Pooling=True;Max Pool Size=100;  

                return string.Format(@"Data Source={0};Pooling=true;FailIfMissing=false;Version=3;UTF8Encoding=True;Journal Mode=Off;", 
                                            System.IO.Path.Combine(System.Reflection.Assembly.GetEntryAssembly().Location, "local_data_sqlite.db"));
            }
        }

        /// <summary>  
        /// TestInsert  
        /// </summary>  
        public void TestInsert()
        {
            var sql = @"  insert into person(  
                         name,  
                         age,  
                         sex,  
                         create_time  
                        ) values(  
                         @name,  
                         @age,  
                         @sex,  
                         @create_time  
                        );";

            int n = 0;
            object obj;
            using (var conn = new SQLiteConnection(_conn))
            {
                n = conn.Execute(sql, new
                {
                    name = "admin",
                    age = "111",
                    sex = "男",
                    create_time = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")
                });
                Assert.IsTrue(n == 1);
                //  
                n = conn.Execute(sql, new
                {
                    name = "guest",
                    age = "222",
                    sex = "女",
                    create_time = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")
                });
                Assert.IsTrue(n == 1);

                //2 查表  
                sql = "select count(*) from person;";
                n = conn.ExecuteScalar<int>(sql);

                conn.Close();
            }

            Assert.IsTrue(n > 0);
        }

        /// <summary>  
        /// TestDelete  
        /// </summary>  
        public void TestDelete()
        {
            var sql = @"delete from person where name='admin';";

            int n = 0;
            object obj;
            using (var conn = new SQLiteConnection(_conn))
            {
                n = conn.Execute(sql);
            }

            Assert.IsTrue(n > 0);
        }
        /// <summary>  
        /// TestUpdate  
        /// </summary>  
        public void TestUpdate()
        {
            var sql = @"update person set age=99 where name=@name;";

            int n = 0;
            object obj;
            using (var conn = new SQLiteConnection(_conn))
            {
                n = conn.Execute(sql, new
                {
                    name = "guest"
                });
            }

            Assert.IsTrue(n > 0);
        }
        /// <summary>  
        /// TestSelect  
        /// </summary>  
        public void TestSelect()
        {
            var sql = @"select * from person where name=@name;";
            //sql = @"select * from person;";  
            var list = new List<PsersonModel>();
            using (var conn = new SQLiteConnection(_conn))
            {
                list = conn.Query<PsersonModel>(sql, new
                {
                    name = "admin"
                }) as List<PsersonModel>;
            }

            Assert.IsTrue(list.Count > 0);
        }

        public class PsersonModel
        {
            public string name { set; get; }
            public int age { set; get; }
            public string sex { set; get; }
            public DateTime create_time { set; get; }
        }
    }
#endif
#endregion
}
