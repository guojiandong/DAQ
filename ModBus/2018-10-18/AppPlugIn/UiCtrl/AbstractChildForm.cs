using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.UiCtrl
{
    public partial class AbstractChildForm : AbstractFormBase
    {
        private IChildFormClosedListener mListener;
        public AbstractChildForm() : this(null)
        {

        }

        public AbstractChildForm(IChildFormClosedListener listener)
        {
            mListener = listener;
            //InitializeComponent();
        }

        protected virtual void onInitializeComponent()
        {
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            //this.MaximizeBox = false;
        }

        internal AbstractChildForm SetChildFormClosedListener(IChildFormClosedListener listener)
        {
            mListener = listener;
            return this;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (mListener != null)
            {
                mListener.OnChildFormClosed(this);
            }
        }

        public interface IChildFormClosedListener
        {
            void OnChildFormClosed(AbstractChildForm form);
        }
    }
}
