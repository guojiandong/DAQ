using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Model.Settings;
using Ksat.AppPlugIn.UiCtrl;

namespace Ksat.AppPlugIn.Impl
{
    [Guid("7D68D700-6FCA-4D24-AFEB-48171621D53D")]
    [ComVisible(true)]
    public abstract class AbstractPlugInComponentBase : IPlugInComponent
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PlugInName
        {
            get;
            private set;
        }

        /// <summary>
        /// 插件版本，可实现按组件更新
        /// </summary>
        public string PlugInVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// 插件ID
        /// </summary>
        public int Identify
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取插件配置信息
        /// </summary>
        public abstract AbstractProfileSettings PlugInSettings { get; }

        /// <summary>
        /// 创建插件
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="compoentVersion"></param>
        /// <param name="id"></param>
        public AbstractPlugInComponentBase(string name, string version, int id)
        {
            this.PlugInName = name;
            this.PlugInVersion = version;
            this.Identify = id;
        }


        /// <summary>
        /// 创建子窗体
        /// </summary>
        /// <param name="tag">子窗体标识</param>
        /// <param name="parent">父窗体</param>
        /// <returns></returns>
        public virtual AbstractChildForm CreatePlugInForm(object tag, AbstractMultiChildForm parent)
        {
            return null;
        }

        public virtual bool ToolStripMenuItemClick(AbstractMultiChildForm parent, object tag, EventArgs e)
        {
            return false;
        }

        public virtual void ApplyResources(ComponentResourceManager res)
        {
            //throw new NotImplementedException();
        }

        public virtual void InitializePlugin(IAppContext app)
        {
            //throw new NotImplementedException();
        }

        public virtual void AppFormContainer_OnLoad(AbstractAppFormContainer form = null, System.EventHandler menuClickHandler = null)
        {

        }

        public virtual void AppFormContainer_OnClosed(AbstractAppFormContainer form = null)
        {

        }

        //public AbstractChildForm CreatePlugInForm(AbstractMultiChildForm parent, string tag)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
