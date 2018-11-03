using Ksat.AppPlugIn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksat.AppPlugIn.UiCtrl
{
    public partial class AbstractAppFormContainer : AbstractMultiChildForm
    {
        public AbstractAppFormContainer()
        {
        }

        public virtual EventHandler GetMenuStripContainerEventHandler()
        {
            throw new NotImplementedException();
        }

        public virtual ToolStripMenuItem GetToolStripMenuItem(RootMenuItemEnum item)
        {
            if (base.MainMenuStrip == null) return null;
            if(item == RootMenuItemEnum.File)
            {
                
                
            }

            string tag = item.ToString().ToLower();
            foreach (ToolStripItem m in MainMenuStrip.Items)
            {
                if (m.Tag == null) continue;

                if(tag.Equals(m.Tag))
                {
                    return (ToolStripMenuItem)m;
                }
            }

            return null;
            //throw new NotImplementedException();
        }

        public virtual StatusStrip GetStatusStripContainer()
        {
            return null;
        }

        public virtual MenuStrip GetMenuStripContainer()
        {
            return null;
        }
    }
}
