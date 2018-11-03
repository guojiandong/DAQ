using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksat.AppPlugIn.UiCtrl
{
    public partial class AbstractUserControlBase : UserControl
    {
        public AbstractUserControlBase()
        {
            //InitializeComponent();
        }

        protected virtual void ApplyResources(System.ComponentModel.ComponentResourceManager res)
        {

        }

        /// <summary>
        /// 语言切换的接口
        /// </summary>
        internal void SwitchLanguage()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());

            //System.Globalization.CultureInfo mCultureInfo = ManageLanguage.Instance().getCultureInfo();
#if false //DEBUG
            Console.WriteLine(String.Format("UserControlBase::ApplyResources, {0}, {1}", GetType(), mCultureInfo));
#endif
            foreach (Control ctl in this.Controls)
            {
                findAndApplyResources(ctl, resources);
            }

            ApplyResources(resources);
        }

        private void findAndApplyResources(Control ctl, System.ComponentModel.ComponentResourceManager res)
        {
            if (ctl is ContainerControl)
            {
#if DEBUG
                Console.WriteLine(String.Format("ctl:{0} is ContainerControl, ignore", ctl.Name));
#endif
            }
            else
            {
                res.ApplyResources(ctl, ctl.Name);
            }

            if (ctl.HasChildren)
            {
                foreach (Control c in ctl.Controls)
                {
                    if (c is Form)
                    {
#if DEBUG
                        Console.WriteLine(String.Format("ctl is form, ignore"));
#endif
                    }
                    else
                    {
                        findAndApplyResources(c, res);
                    }
                }
            }
        }
    }
}
