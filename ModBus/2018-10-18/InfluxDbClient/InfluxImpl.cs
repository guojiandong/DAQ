using Ksat.InfluxDbClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ksat.InfluxDbClient
{
    internal class InfluxImpl
    {
        private static InfluxImpl influxImpl;
        public static InfluxImpl Instance()
        {
            if (influxImpl == null)
                influxImpl = new InfluxImpl();
            return influxImpl;
        }

        private Thread mThread;
        private string urlBase;
        public SoftWareInfo SoftwareInfo { get; set; }
        private string dbNameLast { get; set; }

        public bool IsDbNameExist(string str)
        {
            if (string.IsNullOrEmpty(dbNameLast) || dbNameLast != str)
                return false;
            else
                return true;
        }

        public InfluxImpl()
        {
            influxImpl = this;

            mRecordCache = new Queue<RecordBase>();
            mSemaphore = new SemaphoreSlim(0, int.MaxValue);
            SoftwareInfo = new SoftWareInfo();

        }

        private bool isCancel;
        public void Start(string urlbase)
        {
            urlBase = urlbase;

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
            mSemaphore.Release();
            mThread = null;
        }

        private Queue<RecordBase> mRecordCache;
        private SemaphoreSlim mSemaphore;

        public void Push(RecordBase record)
        {
            lock (mRecordCache)
            {
                mRecordCache.Enqueue(record);

            }
            if (mRecordCache != null && mRecordCache.Count > 10)
                mSemaphore.Release();
        }
        private void OnProcess()
        {
            Console.WriteLine(">>>>>>>>>>>>>InfluxImpl::ThreadProc(start)>>>>>>>>>>>>>");
            List<RecordBase> getRecords = new List<RecordBase>();
            while (true)
            {
                try
                {
                    RecordBase record = null;
                    lock (mRecordCache)
                    {
                        if (mRecordCache.Count > 0)
                        {
                            record = mRecordCache.Dequeue();

                            getRecords.Add(record);
                        }
                    }

                    if (getRecords.Count > 0)
                    {
                        string dbStr = SoftwareInfo.ProjectName;
                        //保存数据
                        if (!IsDbNameExist(dbStr))
                        {
                            Console.WriteLine(InfluxDb.CreateDataBase(dbStr));
                            dbNameLast = dbStr;
                        }
                        string urlstr = string.Format("{0}/write?db={1}", urlBase, dbStr);

                        List<string> sqls = new List<string>();
                        foreach (RecordBase item in getRecords)
                        {

                            string dtStr = string.Format("{0}_{1}_{2}_{3}_{4}", SoftwareInfo.ProjectName, SoftwareInfo.LineIndex, SoftwareInfo.SoftwareName, record.GetType().Name, DateTime.Now.ToString("yyyyMMdd"));

                            sqls.Add(dtStr + "," + record.GetInsertSqlString());
                        }


                        string sql = string.Join("\n", sqls);
                        Console.WriteLine(HttpHelper.HttpHelperPost(urlstr, sql));

                        getRecords.Clear();
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
    }
}
