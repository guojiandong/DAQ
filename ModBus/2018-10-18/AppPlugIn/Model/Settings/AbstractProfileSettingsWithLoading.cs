
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Model.Settings
{
    [Serializable]
    public abstract class AbstractProfileSettingsWithLoading : AbstractProfileSettings
    {
        //private const string TAG = "AbstractProfileSettingsWithLoading";

        //private static AbstractProfileSettingsWithLoading gSettings;

        //public AbstractProfileSettingsWithLoading() : base()
        //{

        //}



        //private static string FILE_NAME = String.Format("{0}.xml", typeof(T).Name);


        //private static string FACTORY_FILE_NAME = String.Format("{0}.factory.xml", typeof(T).Name);

        //private static string gRootDataDir = "";

        //public static void InitDataRootPath()
        //{
        //    gRootDataDir = AppHelper.GetApplicationCommonDataPath();

        //    try
        //    {
        //        if (!Directory.Exists(gRootDataDir))
        //        {
        //            Directory.CreateDirectory(gRootDataDir);
        //        }

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(TAG, "create root data directory:" + gRootDataDir + " exception:", ex);
        //    }
        //    gRootDataDir = Directory.GetCurrentDirectory();
        //}

        //public static string GetRootDataDirectory()
        //{
        //    //lock (typeof(ProfileSettings))
        //    {
        //        if (String.IsNullOrEmpty(gRootDataDir))
        //        {
        //            InitDataRootPath();
        //        }
        //    }

        //    return gRootDataDir;
        //}


        //public static AbstractProfileSettingsWithLoading Load()
        //{
        //    lock (typeof(AbstractProfileSettingsWithLoading))
        //    {
        //        if (gSettings == null)
        //        {
        //            string path = Path.Combine(GetRootDataDirectory(), FILE_NAME);

        //            AbstractProfileSettingsWithLoading p = LoadFrom(path);
        //            if (p != null)
        //            {
        //                gSettings = p;
        //            }
        //            else
        //            {
        //                gSettings = new T();
        //                gSettings.onInitialise();
        //                Logger.Warn(TAG, "ProfileSettings " + path + " doesn't exist, default value will be used !");
        //            }
        //        }
        //    }

        //    return gSettings;
        //}

        //public static bool LoadFactory()
        //{
        //    lock (typeof(AbstractProfileSettings))
        //    {
        //        string path = Path.Combine(GetRootDataDirectory(), FACTORY_FILE_NAME);

        //        T p = LoadFrom(path);
        //        if (p != null)
        //        {
        //            gSettings = p;
        //            return true;
        //        }
        //        else
        //        {
        //            Logger.Warn(TAG, "ProfileSetting factory " + path + " doesn't exist, value will not be changed !");

        //            return false;
        //        }
        //    }
        //}

        //public static bool SaveFactory()
        //{
        //    string path = Path.Combine(GetRootDataDirectory(), FACTORY_FILE_NAME);
        //    return SaveAs(gSettings, path);
        //}


        //public static bool Save()
        //{
        //    return Save(gSettings);
        //}

        //public static bool Save(T obj)
        //{
        //    string path = Path.Combine(GetRootDataDirectory(), FILE_NAME);
        //    return SaveAs(obj, path);
        //}

        //public static bool SaveAs(T obj, string path)
        //{
        //    if (obj == null)
        //    {
        //        throw new ArgumentNullException("obj can't be null...");
        //    }

        //    try
        //    {
        //        SerializerHelper.Serializer(obj, path, SerializerHelper.SerializerMode.Xml);
        //        //Logger.Info("para write success.");

        //        lock (typeof(AbstractProfileSettings))
        //        {
        //            if (gSettings != null)
        //                gSettings.onSaved(obj);
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(TAG, "para write to file:" + path + " error!", ex);
        //    }
        //    return false;
        //}

        //public static T LoadFrom(string path)
        //{
        //    try
        //    {
        //        T p = SerializerHelper.Deserialize<T>(path, SerializerHelper.SerializerMode.Xml);
        //        Logger.Info(TAG, "Load ProfileSettings from " + path + " success.");
        //        if (p != null)
        //        {
        //            p.CheckVersionUpgrade();
        //        }

        //        return p;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Warn(TAG, "Load ProfileSettings from " + path + " failed!", ex);
        //    }
        //    return null;
        //}
    }
}
