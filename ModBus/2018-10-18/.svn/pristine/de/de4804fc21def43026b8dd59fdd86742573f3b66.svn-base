using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ksat.AppPlugInUiLibrary.Login
{
    [Serializable]
    public class StorePassword
    {
        public StorePassword()
        {
            Engineer = "666666";
            Administrator = "888888";
            LastLoginId = "";
        }

        public override string ToString()
        {
            return String.Format("Engineer:{0}, Administrator:{1}, id:{2}", Engineer, Administrator, LastLoginId);
        }

        public string Engineer { get; set; }
        public string Administrator { get; set; }

        public string LastLoginId { get; set; }

        private static string gRootDir = "";
        public const string FILE_NAME = "pwd.xml";
        public static string getLocalDataFile()
        {
            if (String.IsNullOrEmpty(gRootDir))
            {
                gRootDir = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData),
                        System.IO.Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName));
                try
                {
                    if (!Directory.Exists(gRootDir))
                    {
                        Directory.CreateDirectory(gRootDir);
                    }
                }
                catch (Exception)
                {
                    gRootDir = Directory.GetCurrentDirectory();
                }
            }

            return gRootDir;
        }

        private static StorePassword gStorePassword;
        public static StorePassword load()
        {
            lock (typeof(StorePassword))
            {
                if(gStorePassword == null)
                {
                    string path = Path.Combine(getLocalDataFile(), FILE_NAME);
                    Logger.Info("load(0), path:" + path);

                    try
                    {
                        XmlSerializer paramaterX = new XmlSerializer(typeof(StorePassword));
                        
                        StreamReader sr = new StreamReader(path);
                        gStorePassword = (StorePassword)paramaterX.Deserialize(sr);
                        sr.Close();

                        Logger.Info("load(1), path:" + path+", info:"+gStorePassword.ToString());
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("XmlSerializer() Deserialize failed..", ex);
                        gStorePassword = new StorePassword();
                    }
                }
            }

            return gStorePassword;
        }

        public static void store()
        {
            if (gStorePassword == null)
            {
                Logger.Warn("store(), invalid store");
                return;
            }

            gStorePassword.Store();
        }

        public void Store()
        {
            Logger.Info("Store(), info:" + this.ToString());
            try
            {
                XmlSerializer paramaterX = new XmlSerializer(typeof(StorePassword));
                StreamWriter sw = new StreamWriter(Path.Combine(getLocalDataFile(), FILE_NAME));
                lock (typeof(StorePassword))
                    paramaterX.Serialize(sw, this);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("XmlSerializer() Serialize failed..", ex);
            }
        }
    }
}
