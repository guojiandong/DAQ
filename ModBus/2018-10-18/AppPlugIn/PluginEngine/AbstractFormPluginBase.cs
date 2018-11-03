
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.PluginEngine
{
    [System.Runtime.InteropServices.Guid("BABE4306-64B6-454A-BB0D-64F191505E5F")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public abstract class AbstractFormPluginBase : AbstractPluginBase
    {
        public virtual UiCtrl.AbstractChildForm CreatePlugInForm(object tag, UiCtrl.AbstractMultiChildForm parent)
        {
            //throw new NotImplementedException();

            return null;
        }

        public virtual bool ToolStripMenuItemClick(UiCtrl.AbstractMultiChildForm parent, object tag, EventArgs e)
        {
            //throw new NotImplementedException();

            return false;
        }

        public virtual void ApplyResources(System.ComponentModel.ComponentResourceManager res)
        {
            //throw new NotImplementedException();
        }
    }
}
