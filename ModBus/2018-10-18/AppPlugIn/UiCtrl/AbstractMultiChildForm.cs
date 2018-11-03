using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.UiCtrl
{
    public partial class AbstractMultiChildForm : AbstractChildForm, AbstractChildForm.IChildFormClosedListener
    {
        private LinkedList<AbstractChildForm> mChildFormList = new LinkedList<AbstractChildForm>();

        public AbstractMultiChildForm()
        {
        }

        protected override void onInitializeComponent()
        {
            // 
        }

        public bool CheckChildFormExisting<T>(object tag = null) where T : AbstractChildForm
        {
            if (tag == null)
            {
                //throw new NoNullAllowedException("GetChildForm(tag) tag can't be null...");
                tag = typeof(T).FullName;
            }

            //foreach (AbstractChildForm item in mChildFormList)
            LinkedListNode<AbstractChildForm> nodeNow = mChildFormList.First;
            while (nodeNow != null)
            {
                if (nodeNow.Value.GetType() == typeof(T))
                {
                    if (tag != null && !nodeNow.Value.Tag.Equals(tag))
                        continue;

                    return true;
                }

                nodeNow = nodeNow.Next;
            }

            return false;
        }

        public T GetChildForm<T>(object tag = null) where T : AbstractChildForm
        {
            if (tag == null)
            {
                //throw new NoNullAllowedException("GetChildForm(tag) tag can't be null...");
                tag = typeof(T).FullName;
            }

            //foreach (AbstractChildForm item in mChildFormList)
            LinkedListNode<AbstractChildForm> nodeNow = mChildFormList.First;
            while (nodeNow != null)
            {
                AbstractChildForm item = nodeNow.Value;
                if (item.Tag.Equals(tag) && item.GetType() == typeof(T))
                {
                    return (T)item;
                }
                nodeNow = nodeNow.Next;
            }

            return null;
        }

        public T CreateChildForm<T>(object tag = null) where T : AbstractChildForm
        {
            if (tag == null)
            {
                //throw new NoNullAllowedException("CreateChildForm(tag) tag can't be null...");
                tag = typeof(T).FullName;
            }

            //foreach (AbstractChildForm item in mChildFormList)
            LinkedListNode<AbstractChildForm> nodeNow = mChildFormList.First;
            while (nodeNow != null)
            {
                AbstractChildForm item = nodeNow.Value;
                if (item.Tag.Equals(tag) && item.GetType() == typeof(T))
                {
                    //item.Show();
                    return (T)item;
                }

                nodeNow = nodeNow.Next;
            }

            //AbstractChildForm form = (AbstractChildForm)Activator.CreateInstance(typeof(T), new object[] { this });
            AbstractChildForm form = (AbstractChildForm)Activator.CreateInstance<T>();
            form.Tag = tag;

            onChildFormCreated(form);
            form.SetChildFormClosedListener(this);

            mChildFormList.AddLast(form);

            //Logger.Info("CreateChildForm(), child form count:" + mChildFormList.Count);

            return (T)form;
        }

        protected virtual void onChildFormCreated(AbstractChildForm form)
        {
            form.Owner = this;
            if (base.IsMdiContainer)
            {
                form.MdiParent = this;
            }
        }

        public T ShowChildForm<T>(object tag = null) where T : AbstractChildForm
        {
            if (tag == null)
            {
                tag = typeof(T).FullName;
            }

            AbstractChildForm form = CreateChildForm<T>(tag);

            form.Show();

            //if(form.WindowState == System.Windows.Forms.FormWindowState.Maximized)
            //{
            //    form.
            //}
            //Logger.Info("showChildForm(), child form count:" + mChildFormList.Count);

            return (T)form;
        }

        protected void removeChildForm(AbstractChildForm form)
        {
            //Logger.Info("removeChildForm(), " + form.Tag);

            //foreach (AbstractChildForm item in mChildFormList)
            LinkedListNode<AbstractChildForm> nodeNow = mChildFormList.First;
            while (nodeNow != null)
            {
                if (nodeNow.Value.Equals(form))
                {
                    mChildFormList.Remove(nodeNow);
                    break;
                }

                nodeNow = nodeNow.Next;
            }

            //Logger.Info("removeChildForm(), child form count:" + mChildFormList.Count);
        }

        #region implate IChildFormClosedListener
        public void OnChildFormClosed(AbstractChildForm form)
        {
            removeChildForm(form);
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            //Logger.Info(">>>>>>>>>>>>AbstractMultiChildForm::OnClosed(), start to close all childs... " + Tag);
#if true
            AbstractChildForm child = null;
            while (mChildFormList.First != null)
            {
                child = mChildFormList.First.Value;

                mChildFormList.RemoveFirst();

                child.Close();
            }
#else
            LinkedListNode<AbstractChildForm> nodeNow = mChildFormList.First;
            while (nodeNow != null)
            {
                nodeNow.Value.BeginInvoke(new EventHandler(delegate
                {
                    nodeNow.Value.Close();
                }), null);
                
                nodeNow = nodeNow.Next;
            }
#endif
            //Logger.Info("<<<<<<<<<<<<<<<AbstractMultiChildForm::OnClosed(), " + Tag);

            base.OnClosed(e);
        }
    }
}
