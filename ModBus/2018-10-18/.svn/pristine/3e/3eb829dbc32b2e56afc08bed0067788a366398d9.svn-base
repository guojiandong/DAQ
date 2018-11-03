using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Utils
{
    public static class AppHelper
    {
        /// <summary>
        /// C:\Users\ajunz\AppData\Local\TestConsoleApp
        /// </summary>
        /// <returns></returns>
        public static string GetLocalApplicationDataPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), GetApplicationName());
        }

        /// <summary>
        /// C:\Users\ajunz\AppData\Roaming\TestConsoleApp
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationDataPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), GetApplicationName());
        }

        /// <summary>
        /// C:\ProgramData\TestConsoleApp
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationCommonDataPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), GetApplicationName());
        }

        /// <summary>
        /// C:\Users\ajunz\TestConsoleApp
        /// </summary>
        /// <returns></returns>
        public static string GetUserProfileDataPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), GetApplicationName());
        }

        public static string GetApplicationName()
        {
            try
            {
                return System.IO.Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName);
            }
            catch
            {
                return "Unkown";
            }
#if true
            
#else
            string str = Process.GetCurrentProcess().MainModule.ModuleName;
            string[] strs = str.Split('.');
            if(strs != null && strs.Length > 0)
            {
                return strs[0];
            }
            return str;
#endif
        }

        public static void OpenCurrentApplicationFolder()
        {
            try
            {
                System.Diagnostics.Process.Start(System.Environment.CurrentDirectory);
            }
            catch (System.ComponentModel.Win32Exception we)
            {
                Logger.Error(Logger.Tag, "OpenCurrentApplicationFolder failed:", we);
            }
        }
    }
}
