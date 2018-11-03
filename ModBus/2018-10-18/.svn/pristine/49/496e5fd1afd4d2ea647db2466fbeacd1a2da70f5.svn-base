using Ksat.AppPlugIn.Logging;
using Ksat.AppPlugIn.Logging.log;
using Ksat.AppPlugIn.Model;
using Ksat.AppPlugIn.Model.Args;
using Ksat.AppPlugIn.Threading;
using Ksat.AppPlugIn.Threading.Queue;
using Ksat.AppPlugIn.Threading.Task;
using Ksat.LogMySql.Model;
using Ksat.LogMySql.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.LogMySql.Task
{
    internal class MySqlImpl : ILogger
    {
        private static MySqlImpl influxImpl;
        public static MySqlImpl Instance()
        {
            if (influxImpl == null)
                influxImpl = new MySqlImpl();
            return influxImpl;
        }

        private Thread mThread;
        public string connectionString;
        public SoftWareInfo AssemblyInfo { get; set; }
        private LinkedList<string> mdbNameCache { get; set; }

        public bool IsDbNameExist(string str)
        {
            lock (mdbNameCache)
            {
                foreach (string db in mdbNameCache)
                {
                    if (db == str) return true;
                }
            }
            return false;
        }

        private string lastDbName;
        public MySqlImpl()
        {
            influxImpl = this;

            mRecordCache = new Queue<LogMessageRecord>();
            mSemaphore = new SemaphoreSlim(0, int.MaxValue);
            mdbNameCache = new LinkedList<string>();
            AssemblyInfo = new SoftWareInfo();
        }

        private bool isCancel;
        public void Start()
        {
            isCancel = false;
            if (mThread == null)
            {
                mThread = new Thread(new ThreadStart(OnProcess));
                mThread.IsBackground = true;
                mThread.Priority = ThreadPriority.Lowest;
            }
            mThread.Start();
        }

        public void Stop()
        {
            isCancel = true;
            mThread.Interrupt();
            mThread = null;
        }

        private Queue<LogMessageRecord> mRecordCache;
        private SemaphoreSlim mSemaphore;

        public void Push(LogMessageRecord record)
        {
            lock (mRecordCache)
            {
                mRecordCache.Enqueue(record);

            }

            if (mRecordCache.Count > 10 && mSemaphore.CurrentCount == 0)
                mSemaphore.Release();
        }


        private int createTable(string dbStr)
        {
            string sql = "create table if not exists " + dbStr
                + "(ID BIGINT NOT NULL AUTO_INCREMENT, TimeTicks VARCHAR(40) NOT NULL,Tag VARCHAR(60),LogType VARCHAR(60),Message TEXT,PRIMARY KEY(ID))";
            if (string.IsNullOrEmpty(connectionString)) return -1;
            try
            {
                return MySqlHelper.ExecuteNonQuery(connectionString, System.Data.CommandType.Text, sql, null);

            }
            catch (Exception ex)
            {
                Console.WriteLine("createTable error! " + ex.ToString());
                return -1;
            }
        }

        private int saveToDataBase(string tbName, List<LogMessageRecord> items)
        {
            if (items == null || items.Count == 0) return 0;
            string sql = string.Format("insert into {0}(TimeTicks,Tag,LogType,Message)Values", tbName);
            StringBuilder stringBuilder = new StringBuilder(sql);
            stringBuilder.Append(string.Join<LogMessageRecord>(",", items));
            try
            {
                return MySqlHelper.ExecuteNonQuery(connectionString, System.Data.CommandType.Text, stringBuilder.ToString(), null);

            }
            catch (Exception ex)
            {
                Console.WriteLine("saveToDataBase error! " + ex.ToString());
                return -1;
            }
        }

        private void OnProcess()
        {
            Console.WriteLine(">>>>>>>>>>>>>InfluxImpl::ThreadProc(start)>>>>>>>>>>>>>");

            List<LogMessageRecord> cache = new List<LogMessageRecord>();
            while (true)
            {
                try
                {
                    LogMessageRecord record = null;
                    lock (mRecordCache)
                    {
                        if (mRecordCache.Count > 0)
                        {
                            record = mRecordCache.Dequeue();
                            cache.Add(record);
                        }
                    }

                    if (cache.Count > 0)
                    {

                        string dbStr = string.Format("{0}_{1}_{2}_{3}", AssemblyInfo.ProjectName, AssemblyInfo.LineIndex, AssemblyInfo.SoftwareName, DateTime.Now.ToString("yyyyMMdd"));
                        if (!IsDbNameExist(dbStr))
                        {
                            //查询是否存在数据表
                            createTable(dbStr);
                            mdbNameCache.AddLast(dbStr);
                        }
                        //保存数据
                        saveToDataBase(dbStr, cache);
                        cache.Clear();
                        continue;
                    }
                    else
                    {
                        if (isCancel)
                            break;
                    }

                    mSemaphore.Wait(60 * 1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("InfluxImpl::ThreadProc error:" + ex.ToString());
                }
            }

            Console.WriteLine("<<<<<<<<<<<<<InfluxImpl::ThreadProc(end)<<<<<<<<<<<<<<<<<<");
        }

        public void Write(LogMessageEventArgs args)
        {
            Push(new LogMessageRecord(args));
        }

        public LoggerType GetLoggerType()
        {
            return LoggerType.Hook;
        }

        public void Dispose()
        {

        }
    }
}
