
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Settings
{
    public static class ProfileHelper<T> where T : AbstractProfileSettings, new()
    {
        private const string TAG = "ProfileHelper";
        private static T gSettings;

        public readonly static string FILE_NAME = String.Format("{0}.xml", typeof(T).Name);
        
        public readonly static string FACTORY_FILE_NAME = String.Format("{0}.factory.xml", typeof(T).Name);

        private static string gRootDataDir = "";

        public static void InitDataRootPath(string path = "")
        {
            if (String.IsNullOrEmpty(path))
            {
                gRootDataDir = Utils.AppHelper.GetApplicationCommonDataPath();
            }
            else if(Path.IsPathRooted(path))
            {
                gRootDataDir = path;
            }
            else
            {
                gRootDataDir = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), path);
            }

            try
            {
                if (!Directory.Exists(gRootDataDir))
                {
                    Directory.CreateDirectory(gRootDataDir);
                }

                return;
            }
            catch (Exception ex)
            {
                Logging.Logger.Error(TAG, "create root data directory:" + gRootDataDir + " exception:", ex);
            }
            gRootDataDir = Directory.GetCurrentDirectory();
        }

        public static string GetRootDataDirectory()
        {
            //lock (typeof(ProfileSettings))
            {
                if (String.IsNullOrEmpty(gRootDataDir))
                {
                    InitDataRootPath();
                }
            }

            return gRootDataDir;
        }

        private static void doLoad()
        {
            string path = Path.Combine(GetRootDataDirectory(), FILE_NAME);
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), FACTORY_FILE_NAME);
                if (!File.Exists(path))
                    path = Path.Combine(Directory.GetCurrentDirectory(), FACTORY_FILE_NAME + ".deploy");
            }

            T p = LoadFrom(path);
            if (p != null)
            {
                gSettings = p;
            }
            else
            {
                gSettings = new T();
                gSettings.onFirstLoaded();
                Logging.Logger.Warn(TAG, "ProfileSettings " + path + " doesn't exist, default value will be used !");
            }

            gSettings.onLoaded();
        }

        public static T ReLoad()
        {
            lock (typeof(AbstractProfileSettings))
            {
                doLoad();
            }

            return gSettings;
        }

        public static T Load()
        {
            lock (typeof(AbstractProfileSettings))
            {
                if (gSettings == null)
                {
                    doLoad();
                }
            }

            return gSettings;
        }

        public static bool LoadFactory()
        {
            lock (typeof(AbstractProfileSettings))
            {
                string path = Path.Combine(GetRootDataDirectory(), FACTORY_FILE_NAME);

                T p = LoadFrom(path);
                if (p != null)
                {
                    gSettings = p;

                    gSettings.onLoaded();

                    return true;
                }
                else
                {
                    Logging.Logger.Warn(TAG, "ProfileSetting factory " + path + " doesn't exist, value will not be changed !");

                    return false;
                }
            }
        }

        public static bool SaveFactory()
        {
            string path = Path.Combine(GetRootDataDirectory(), FACTORY_FILE_NAME);
            return SaveAs(gSettings, path);
        }


        public static bool Save()
        {
            return Save(gSettings);
        }

        public static bool Save(T obj)
        {
            string path = Path.Combine(GetRootDataDirectory(), FILE_NAME);
            return SaveAs(obj, path);
        }

        public static bool SaveAs(T obj, string path)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj can't be null...");
            }

            try
            {
                Utils.SerializerHelper.Serializer(obj, path, Utils.SerializerHelper.SerializerMode.Xml);
                //Logger.Info("para write success.");

                lock (typeof(AbstractProfileSettings))
                {
                    if (gSettings != null)
                        gSettings.onSaved(obj);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.Logger.Error(TAG, "para write to file:" + path + " error!", ex);
            }
            return false;
        }

        public static T LoadFrom(string path)
        {
            try
            {
                T p = Utils.SerializerHelper.Deserialize<T>(path, Utils.SerializerHelper.SerializerMode.Xml);
                Logging.Logger.Info(TAG, "Load ProfileSettings from " + path + " success.");
                if (p != null)
                {
                    p.CheckVersionUpgrade();
                }

                return p;
            }
            catch (Exception ex)
            {
                Logging.Logger.Warn(TAG, "Load ProfileSettings from " + path + " failed!", ex);
            }
            return null;
        }
    }
}
