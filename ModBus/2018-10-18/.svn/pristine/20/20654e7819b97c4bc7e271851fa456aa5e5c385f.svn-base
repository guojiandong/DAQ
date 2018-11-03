using Ksat.AppPlugIn.MultiLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksat.AppPlugIn.UiCtrl
{
    public partial class AbstractFormBase : Form
    {
        public AbstractFormBase()
        {
            //gSingleFormList.AddLast(new KeyValuePair<string, FormBase>(GetType().ToString(), this));
#if DEBUG
            Console.WriteLine(String.Format("AbstractFormBase::FormBase(), {0} ", GetType().ToString()));
#endif
            MultiLanguageManager.Instance().RegObject(this);
        }



        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            MultiLanguageManager.Instance().RemoveObject(this);

#if DEBUG
            Console.WriteLine(String.Format("AbstractFormBase::OnFormClosed(), {0} ", GetType().ToString()));
#endif
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

            //ArrayList list = new ArrayList();
            LinkedList<Control> list = new LinkedList<Control>();

            LinkedList<AbstractUserControlBase> userCtrl_list = new LinkedList<AbstractUserControlBase>();
            FindControls(list, this, userCtrl_list);


            this.ResumeLayout(false);
            this.Text = resources.GetString("$this.Text");
#if false //DEBUG
            Console.WriteLine(String.Format("AbstractFormBase::ApplyResources, title:{0}, userctrlcount:{1} ", this.Text, userCtrl_list.Count));
#endif

            //foreach (Control ctl in list)
            LinkedListNode<Control> nodeNow = list.First;
            while (nodeNow != null)
            {
                resources.ApplyResources(nodeNow.Value, nodeNow.Value.Name);

                nodeNow = nodeNow.Next;
            }

            ApplyResources(resources);

            //foreach (UserControlBase ctl in userCtrl_list)
            LinkedListNode<AbstractUserControlBase> nodeUserNow = userCtrl_list.First;
            while (nodeUserNow != null)
            {
                nodeUserNow.Value.SwitchLanguage();
                nodeUserNow = nodeUserNow.Next;
            }

            this.PerformLayout();
        }

        /// <summary>
        /// 把可以本地化的控件放入LIST
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ctl"></param>
        private void FindControls(LinkedList<Control> list, Control ctl, LinkedList<AbstractUserControlBase> userCtrl_list)
        {
            //容器不可以本地化
            if (ctl is ContainerControl)
            {
#if DEBUG
                Console.WriteLine(String.Format("ctl:{0} is ContainerControl, ignore", ctl.Name));
#endif
                //if (ctl.GetType().IsAssignableFrom(typeof(UserControlBase)))
                if (ctl.GetType().IsSubclassOf(typeof(AbstractUserControlBase)))
                {
                    userCtrl_list.AddLast((AbstractUserControlBase)ctl);
                    return;
                }

            }
            else
            {
                list.AddLast(ctl);
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
                        FindControls(list, c, userCtrl_list);
                    }
                }
            }
        }
    }
}
