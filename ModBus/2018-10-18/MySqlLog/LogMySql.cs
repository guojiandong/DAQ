using Ksat.AppPlugIn.Logging;
using Ksat.LogMySql.Model;
using Ksat.LogMySql.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.LogMySql
{
    public static class LogMySql
    {
        public static void SetConnectionString(string connectionStr)
        {
            MySqlImpl.Instance().connectionString = connectionStr;
        }

        public static void SetSoftwareInfo(SoftWareInfo info)
        {
            if (info != null)
                MySqlImpl.Instance().AssemblyInfo = new SoftWareInfo(info);
            else
                throw new ArgumentNullException("para info is null");
        }

        public static void Start()
        {
            Logger.RegisterLogger(MySqlImpl.Instance());
            MySqlImpl.Instance().Start();
        }

        public static void Stop()
        {
            Logger.UnRegisterLogger(MySqlImpl.Instance());
            MySqlImpl.Instance().Stop();
        }

    }
}
