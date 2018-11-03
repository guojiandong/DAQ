using Ksat.AppPlugIn.Logging;
using Ksat.InfluxDbClient.Model;
using Ksat.InfluxDbClient.Model.Attri;
using Ksat.LogMySql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestInfluxDbClient();
          // TestLogger();
        }


        static void TestLogger()
        {
            Logger.EnableLogger(LoggerType.Console);
            LogMySql.SetConnectionString("Database=db_log;Data Source=172.17.201.222;Port=3306;User Id=dbdevlogger;Password=dblogger0000;Charset=utf8;SslMode=None");
            LogMySql.SetSoftwareInfo(new Ksat.LogMySql.Model.SoftWareInfo()
            {
                ProjectName = "Test",
                LineIndex = 1,
                SoftwareName = "console"
            });
            LogMySql.Start();
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");
            Logger.Info("Hehe");

            Thread.Sleep(1000);
            Console.ReadKey();
        }

        static void TestInfluxDbClient()
        {
            Ksat.InfluxDbClient.InfluxDb.UrlBase = "http://172.17.201.221:8086";

            Ksat.InfluxDbClient.InfluxDb.Start();

            Ksat.InfluxDbClient.InfluxDb.Push(new Record() { CreateTime = DateTime.Now.Ticks, name = "pengmeng" });

            Thread.Sleep(2000);
            Console.WriteLine(Ksat.InfluxDbClient.InfluxDb.Query("_Default", "select * from _Default_0__Default_Record_20180628"));
            Console.ReadKey();
        }
    }
    public class Record : RecordBase
    {
        [Field]
        public string name { get; set; }
    }
}
