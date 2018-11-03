using Ksat.AppPlugIn.Model;
using Ksat.AppPlugIn.Model.Settings;
using Ksat.AppPlugIn.PluginEngine;
using Ksat.AppPlugIn.Threading.Task;
using Ksat.AppPlugIn.UiCtrl;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Ksat.AppPlugIn
{
    /// <summary>
    /// 应用程序上下文对象接口
    /// 作用：用于收集应用程序必备的一些公共信息并共享给整个应用程序所有模块使用（含动态加载进来的组件）
    /// </summary>
    [Guid("34CA75F3-1D08-44F4-A070-7D59065173B0")]
    [ComVisible(true)]
    public interface IAppContext
    {
        /// <summary>
        /// 应用程序名称
        /// </summary>
        string AppName { get; }

        /// <summary>
        /// 应用程序版本
        /// </summary>
        string AppVersion { get; }

        /// <summary>
        /// 应用程序根目录
        /// </summary>
        string AppBaseDir { get;  }

        /// <summary>
        /// 用户登录信息，真实项目中为一个实体类
        /// </summary>
        UserInfo SessionUserInfo { get; }

        ///// <summary>
        ///// 用户登录权限信息，这里类型是STRING，真实项目中为一个实体类
        ///// </summary>
        //string PermissionInfo { get; }

        /// <summary>
        /// 应用程序全局缓存，整个应用程序（含动态加载的组件）均可进行读写访问
        /// </summary>
        Dictionary<string, object> AppCache { get; }

        /// <summary>
        /// 应用程序主界面窗体，各组件中可以订阅或获取主界面的相关信息
        /// </summary>
        AbstractAppFormContainer AppFormContainer { get; }

        /// <summary>
        /// 注册列表中的插件
        /// </summary>
        PluginList PlugIns { get;  }
        //IEnumerable<PluginInfo> PlugIns { get;  }


        /// <summary>
        /// 配置文件
        /// </summary>
        MainProfileSettings Settings { get;  }


        /// <summary>
        /// 主线程
        /// </summary>
        Threading.ThreadTask MainTaskInstance { get; }
    }
}
