using Ksat.AppPlugIn.Model.Settings;
using Ksat.AppPlugIn.UiCtrl;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ksat.AppPlugIn
{
    /// <summary>
    /// 组件信息描述接口
    /// 作用：描述该组件（或称为模块，即当前程序集）的一些主要信息，以便应用程序可以动态获取到
    /// </summary>
    [Guid("E44DE1C5-BA54-4191-9ED3-6A4E1ED908E2")]
    [ComVisible(true)]
    public interface IPlugInComponent
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        string PlugInName { get; }

        /// <summary>
        /// 插件版本，可实现按组件更新
        /// </summary>
        string PlugInVersion { get; }

        /// <summary>
        /// 插件ID
        /// </summary>
        int Identify { get;  }

        ///// <summary>
        ///// 向应用程序预注册的窗体类型列表
        ///// </summary>
        //IEnumerable<Type> FormTypes { get; }

        /// <summary>
        /// 获取插件配置信息
        /// </summary>
        AbstractProfileSettings PlugInSettings { get;  }

        /// <summary>
        /// 创建form
        /// </summary>
        /// <param name="tag">子窗体标识</param>
        /// <param name="parent">父窗体</param>
        /// <returns></returns>
        AbstractChildForm CreatePlugInForm(object tag, AbstractMultiChildForm parent);

        /// <summary>
        /// 点击菜单的时候调用
        /// </summary>
        /// <param name="parent">主form</param>
        /// <param name="tag">菜单的tag</param>
        /// <param name="e"></param>
        /// <returns>true ： 已经被处理， false ： 没有处理</returns>
        bool ToolStripMenuItemClick(AbstractMultiChildForm parent, object tag, EventArgs e);

        /// <summary>
        /// 切换语言的时候调用
        /// </summary>
        /// <param name="res"></param>
        void ApplyResources(System.ComponentModel.ComponentResourceManager res);

        /// <summary>
        /// 在程序启动的时候调用
        /// </summary>
        /// <param name="app"></param>
        void InitializePlugin(IAppContext app);

        /// <summary>
        /// 主form加载的时候调用
        /// </summary>
        /// <param name="form"></param>
        /// <param name="menuClickHandler"></param>
        void AppFormContainer_OnLoad(AbstractAppFormContainer form = null, System.EventHandler menuClickHandler = null);

        /// <summary>
        /// 主form销毁的时候调用
        /// </summary>
        /// <param name="form"></param>
        void AppFormContainer_OnClosed(AbstractAppFormContainer form = null);
    }
}
