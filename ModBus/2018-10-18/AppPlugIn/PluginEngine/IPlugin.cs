
using System;
using System.Runtime.InteropServices;

namespace Ksat.AppPlugIn.PluginEngine
{
    /// <summary>
    /// 插件接口
    /// </summary>
    [System.Runtime.InteropServices.Guid("BC4C89DB-A53C-48B8-BECE-8CB5E4FEBF52")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public interface IPlugin
    {
        string PlugInName { get; }

        /// <summary>
        /// 执行初始化
        /// </summary>
        void Initialize(IAppContext app);

        /// <summary>
        /// 加载插件入口
        /// </summary>
        void Attach(IAppContext app);

        /// <summary>
        /// 卸载插件入口
        /// </summary>
        void Detach(IAppContext app);


        /// <summary>
        /// 获取插件配置信息
        /// </summary>
        Model.Settings.AbstractProfileSettings PlugInSettings { get; }

        /// <summary>
        /// 是否支持中途卸载
        /// </summary>
        bool SupportUnload { get; }

        /// <summary>
        /// 是否支持中途加载
        /// </summary>
        bool SupportLoad { get; }

        /// <summary>
        /// 检查是否可以加载
        /// </summary>
        /// <param name="isFirstCall">是否是初始化时候的加载</param>
        /// <returns></returns>
        bool CheckCanLoad(bool isFirstCall);

        /// <summary>
        /// 当前运行的插件版本
        /// </summary>
        string Version { get; }

    }
}
