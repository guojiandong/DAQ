using Ksat.AppPlugIn.UiCtrl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksat.AppPlugInUiLibrary.UiCtrl
{
    public partial class LoadingForm : AbstractChildForm
    {
        public LoadingForm() : this(null)
        {
        }

        public LoadingForm(Form owner)
        {
            Owner = owner;

            InitializeComponent();


            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            this.loadingCircleControl1.Start();
        }
        
        private void LoadingForm_Resize(object sender, EventArgs e)
        {

            //m_OuterCircleRadius = Math.Min(this.Width, this.Height) / 2;
            //m_InnerCircleRadius = (int)(m_OuterCircleRadius * InnerCircleRadiusPercent);

            //GetControlCenterPoint();
            //Invalidate();
        }

        private void LoadingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.loadingCircleControl1.Stop();
        }

        private void LoadingForm_Shown(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                this.Left = (Owner.Width - this.Width) / 2;
                this.Top = (Owner.Height - this.Height) / 2;
            }
            else if (Parent != null)
            {
                this.Left = (Parent.Width - this.Width) / 2;
                this.Top = (Parent.Height - this.Height) / 2;
            }
        }
    }
}