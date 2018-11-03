using Ksat.InfluxDbClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ksat.InfluxDbClient
{
    public static class InfluxDb
    {
        /// <summary>
        /// InfluxDb基础Url
        /// </summary>
        public static string UrlBase;
        /// <summary>
        /// 设置账号密码
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        public static void SetAuthorization(string user, string pwd)
        {
            HttpHelper.SetAuthorization(user, pwd);
        }

        public static void SetSoftwareInfo(SoftWareInfo softWareInfo)
        {
            InfluxImpl.Instance().SoftwareInfo = softWareInfo;
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <returns></returns>
        public static string CreateDataBase(string db)
        {
            return CreateDataBase(UrlBase, db);
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="urlbase">基础Url</param>
        /// <param name="db">数据库名称</param>
        /// <returns></returns>
        public static string CreateDataBase(string urlbase, string db)
        {
            string sql = "q=CREATE DATABASE " + db;
            return HttpHelper.HttpHelperPost(UrlBase + "/query", sql);
        }
        /// <summary>
        /// 销毁数据库
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <returns></returns>
        public static string DropDataBase(string db)
        {
            return DropDataBase(UrlBase, db);
        }
        /// <summary>
        /// 销毁数据库
        /// </summary>
        /// <param name="urlbase">基础url</param>
        /// <param name="db">数据库名称</param>
        /// <returns></returns>
        public static string DropDataBase(string urlbase, string db)
        {
            string sql = "q=DROP DATABASE " + db;
            return HttpHelper.HttpHelperPost(UrlBase + "/query", sql);
        }

        public static void Start()
        {
            Start(UrlBase);
        }
        public static void Start(string url)
        {
            InfluxImpl.Instance().Start(url);
        }
        /// <summary>
        /// 停止数据库存储
        /// </summary>
        public static void Stop()
        {
            InfluxImpl.Instance().Stop();
        }
        /// <summary>
        /// 存储一条数据
        /// </summary>
        /// <param name="record">记录</param>
        public static void Push(RecordBase record)
        {
            InfluxImpl.Instance().Push(record);
        }
        /// <summary>
        /// 根据sql语句查询数据库
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static string Query(string dbname, string sql)
        {
            return Query(UrlBase, dbname, sql);
        }
        /// <summary>
        /// 根据sql语句查询数据库
        /// </summary>
        /// <param name="urlbase">基础url</param>
        /// <param name="dbname">数据库名称</param>
        /// <param name="sql">数据库语句</param>
        /// <returns></returns>
        public static string Query(string urlbase, string dbname, string sql)
        {
            string pathAndQuery = string.Format("/query?db={0}&epoch=u&q={1}", dbname, sql);
            string url = urlbase + pathAndQuery;

            string result = HttpHelper.HttpHelperGet(url);
            return result;
        }
    }
}
