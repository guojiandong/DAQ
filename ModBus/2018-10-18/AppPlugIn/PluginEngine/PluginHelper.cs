using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ksat.AppPlugIn.PluginEngine
{
    /// <summary>
    /// 插件服务管理
    /// </summary>
    [System.Runtime.InteropServices.Guid("CBF19357-15DE-4388-855D-DCFBC3662CF7")]
    public static class PluginHelper
    {
        /// <summary>
        /// 内置的插件类名映射字典
        /// </summary>
        public readonly static Dictionary<InnerPlugin, string> InnerPluginTypeList;



        static PluginHelper()
        {
            InnerPluginTypeList = new Dictionary<InnerPlugin, string>();
            InnerPluginTypeList.Add(InnerPlugin.DefaultPlugin, typeof(DefaultPlugin).FullName);
        }

        /// <summary>
        /// 通过文件路径来查找所有服务
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static PluginInfo[] GetPluginsInAssembly(string assemblyPath)
        {
            try
            {
                return GetPluginsInAssembly(System.Reflection.Assembly.LoadFile(assemblyPath));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 查找指定程序集中所有的服务类
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static PluginInfo[] GetPluginsInAssembly(System.Reflection.Assembly assembly)
        {
            System.Type[] types = assembly.GetTypes();

            List<PluginInfo> typeList = new List<PluginInfo>();
            Array.ForEach(types, s =>
            {
                if (!s.IsPublic || s.IsAbstract) return;
                Type t = s.GetInterface(typeof(IPlugin).FullName);
                if (t == null) return;

                object[] infos = s.GetCustomAttributes(typeof(PluginAttribute), true);

                PluginInfo info = new PluginInfo();
                info.Assembly = System.IO.Path.GetFileName(assembly.Location);
                info.TypeName = s.FullName;

                if (infos == null || infos.Length == 0)
                {
                    info.Enabled = true;
                }
                else
                {
                    info.Enabled = (infos[0] as PluginAttribute).Description.DefaultEnabled;
                }

                try
                {
                    object[] depands = s.GetCustomAttributes(typeof(PluginDepandAttribute), true);
                    if (depands != null && depands.Length > 0)
                    {
                        foreach(object obj in depands)
                        {
                            if (String.IsNullOrEmpty((obj as PluginDepandAttribute).Depand))
                                continue;

                            info.Depands.Add((obj as PluginDepandAttribute).Depand);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn("PluginHelper", "GetPluginsInAssembly(1), TypeName: " + info.TypeName+", ex:"+ex.ToString());
                }
                

                typeList.Add(info);
            });

            return typeList.ToArray();
        }

        /// <summary>
        /// 获得内置的插件
        /// </summary>
        /// <returns></returns>
        public static PluginInfo[] GetPluginsInAssembly()
        {
            return GetPluginsInAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 在指定的路径中查找服务提供者
        /// </summary>
        /// <param name="loaderPath">文件夹列表</param>
        /// <returns>查找的结果</returns>
        public static PluginList GetPlugins(params string[] loaderPath)
        {
            PluginList list = new PluginList();
            Action<string> loader = s =>
            {
                PluginInfo[] slist = GetPluginsInAssembly(s);

                if (slist != null) list.AddPlugin(slist);
            };
            Action<string> folderLoader = s =>
            {
                if (String.IsNullOrEmpty(s)) return;

                if (!System.IO.Path.IsPathRooted(s))
                {
                    s = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), s);
                }
                else
                {
                    if (!System.IO.Directory.Exists(s))
                    {
                        Logger.Warn("PluginHelper", "GetPlugins(1), path not existing :" + s);
                        return;
                    }
                }
                    

                Logger.Info("PluginHelper", "GetPlugins(2), path:" + s);

                string[] files = null;
#if false
                files = System.IO.Directory.GetFiles(s, "*.exe");
                Array.ForEach(files, loader);
#endif

                files = System.IO.Directory.GetFiles(s, "*.dll");
                Array.ForEach(files, loader);
            };

            folderLoader(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
            Array.ForEach(loaderPath, folderLoader);

            return list;
        }
    }
}
