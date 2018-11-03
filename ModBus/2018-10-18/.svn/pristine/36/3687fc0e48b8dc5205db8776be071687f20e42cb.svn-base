using Ksat.AppPlugIn.UiCtrl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksat.AppPlugInUiLibrary.Login
{
    public partial class SimpleLoginForm : AbstractChildForm
    {       
        public SimpleLoginForm() : this(null)
        {
        }

        private ISimpleLoginResultListener mListener;
        public SimpleLoginForm(ISimpleLoginResultListener listener)
        {
            mListener = listener;

            InitializeComponent();
        }

        private static Form gSingleForm;
        public static Form Show(ISimpleLoginResultListener listener)
        {
            lock (typeof(SimpleLoginForm))
            {
                if (gSingleForm == null)
                {
                    gSingleForm = new SimpleLoginForm(listener);
                }

                gSingleForm.Show();
            }

            return gSingleForm;
        }

        public static DialogResult ShowDialog()
        {
            lock (typeof(SimpleLoginForm))
            {
                if (gSingleForm == null)
                {
                    gSingleForm = new SimpleLoginForm(null);
                }

                return gSingleForm.ShowDialog();
            }
        }


        private void SimpleLoginForm_Load(object sender, EventArgs e)
        {
            password.Focus();
            DialogResult = DialogResult.None;
            //Logger.Info("SimpleLoginForm_Load(), id:" + UserNoText.Text);
        }

        private void password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                this.login.PerformClick();
        }

        private void login_Click(object sender, EventArgs e)
        {
            if (password.Text.ToString() == DateTime.Now.ToString("MMdd"))
            {
                DialogResult = DialogResult.OK;             
            }
            this.Close();
        }

        private void SimpleLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void SimpleLoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //StorePassword.store();
            //Logger.Info("LoginForm_FormClosed(), SelectedIndex:" + user_name.SelectedIndex + ", " + UserNoText.Text);
            if (mListener != null )
            {
                if (DialogResult == DialogResult.OK)
                {
                    mListener.onLoginResult(true);
                }
                else
                {
                    mListener.onLoginResult(false);
                }
            }
            gSingleForm = null;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
