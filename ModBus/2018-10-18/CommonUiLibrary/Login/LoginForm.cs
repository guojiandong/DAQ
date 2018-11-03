using Ksat.AppPlugIn.Logging;
using Ksat.AppPlugIn.UiCtrl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Ksat.AppPlugInUiLibrary.Login
{
    public partial class LoginForm : AbstractChildForm
    {
        private ILoginResultListener mListener;
        public LoginForm(ILoginResultListener listener)
        {
            mListener = listener;

            InitializeComponent();
        }
        
        private void LoginForm_Load(object sender, EventArgs e)
        {
            UserRegist.Location = UserLog.Location;
            UserLog.Visible = true;
            UserRegist.Visible = false;
            user_name.Items.Clear();
            user_name.Items.Add(Properties.Resources.String_Guest);
            user_name.Items.Add(Properties.Resources.String_Admin);
            user_name.Items.Add(Properties.Resources.String_Engineer);
            user_name.Text = user_name.Items[0].ToString();

            DialogResult = DialogResult.None;

            UserNoText.Text = StorePassword.load().LastLoginId;
            if (LoginFormManager.Instance().LockAccountId)
            {
                UserNoText.Enabled = false;
            }
            else
            {
                UserNoText.Focus();
            }

            Logger.Info("LoginForm_Load(), id:"+ UserNoText.Text);
        }

        //protected override void ApplyResources(ComponentResourceManager res)
        //{
        //    base.ApplyResources(res);
        //}

        private void user_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            password.Text = "";
            switch ((AccountType)user_name.SelectedIndex)
            {
                case AccountType.Guest:
                    char_panel.Visible = false;
                    PassChange.Visible = false;
                    if(this.UserNoText.Enabled)
                        this.UserNoText.Focus();
                    else
                        this.password.Focus();
                    break;

                case AccountType.Administrator:
                    char_panel.Visible = true;
                    PassChange.Visible = true;
                    if (this.UserNoText.Enabled && String.IsNullOrEmpty(this.UserNoText.Text))
                        this.UserNoText.Focus();
                    else
                        this.password.Focus();
                    break;

                case AccountType.Engineer:
                    char_panel.Visible = true;
                    PassChange.Visible = true;
                    if (this.UserNoText.Enabled && String.IsNullOrEmpty(this.UserNoText.Text))
                        this.UserNoText.Focus();
                    else
                        this.password.Focus();
                    break;
                
                default:
                    break;
            }
        }

        private void password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                this.login.PerformClick();
                //login_Click(sender, e);
        }
        
        private void login_Click(object sender, EventArgs e)
        {
            Logger.Info("login_Click(), SelectedIndex:" 
                + user_name.SelectedIndex+", "+ UserNoText.Text
                +", pwd.txt:"+ password.Text+", ");
            StorePassword.load().LastLoginId = UserNoText.Text.Trim();
            switch ((AccountType)user_name.SelectedIndex)
            {
                case AccountType.Guest:
                    //LoginClass.Instance().setUNameEmpNO(AccountType.Guest, UserNoText.Text.Trim());
                    
                    DialogResult = DialogResult.OK;

                    this.Close();
                    break;
                case AccountType.Engineer:
                    if (password.Text == StorePassword.load().Engineer)
                    {
                        //LoginClass.Instance().setUNameEmpNO(UserName.Engineer, UserNoText.Text.Trim());
                        
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        password.SelectAll();
                        password.Focus();
                        InformationLabel.Text = Properties.Resources.String_Pwd_Error;
                    }
                    break;
                case AccountType.Administrator:
                    if (password.Text == StorePassword.load().Administrator)
                    {
                        //LoginClass.Instance().setUNameEmpNO(UserName.Administrator, UserNoText.Text.Trim());
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        password.SelectAll();
                        password.Focus();
                        InformationLabel.Text = Properties.Resources.String_Pwd_Error;
                    }
                    break;
                default:
                    break;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void PassChange_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserLog.Visible = false;
            UserRegist.Visible = true;
            CharLast.Text = "";
            CharNew.Text = "";
            Again.Text = "";
        }

        private void SureButton_Click(object sender, EventArgs e)
        {
            switch ((AccountType)user_name.SelectedIndex)
            {
                case AccountType.Engineer:
                    if (CharLast.Text.ToString() == StorePassword.load().Engineer)
                    {
                        if (CharNew.Text == Again.Text)
                        {
                            StorePassword.load().Engineer = CharNew.Text;

                            //XmlSerializer paramaterX = new XmlSerializer(typeof(PassWord));
                            //StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\pSetting.ini");
                            //paramaterX.Serialize(sw, pw);
                            //sw.Flush();
                            //sw.Close();
                            //CancelButton_Click(null, null);
                            this.CancelButton.PerformClick();
                        }
                        else
                        {
                            InfoLabel.Text = "Password is not same!";
                        }
                    }
                    else
                        InfoLabel.Text = Properties.Resources.String_Pwd_Error;
                    break;
                case AccountType.Administrator:
                    if (CharLast.Text.ToString() == StorePassword.load().Administrator)
                    {
                        if (CharNew.Text == Again.Text)
                        {
                            StorePassword.load().Administrator = CharNew.Text;
                            //XmlSerializer paramaterX = new XmlSerializer(typeof(PassWord));
                            //StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\pSetting.ini");
                            //paramaterX.Serialize(sw, pw);
                            //sw.Flush();
                            //sw.Close();
                            //CancelButton_Click(null, null);
                            this.CancelButton.PerformClick();
                        }
                        else
                        {
                            InfoLabel.Text = "Password is not same!";
                        }
                    }
                    else
                        InfoLabel.Text = Properties.Resources.String_Pwd_Error;
                    break;
                default:
                    break;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            UserLog.Visible = true;
            UserRegist.Visible = false;
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StorePassword.store();
            Logger.Info("LoginForm_FormClosed(), SelectedIndex:" + user_name.SelectedIndex + ", " + UserNoText.Text);
            if (mListener != null && DialogResult == DialogResult.OK)
            {
                mListener.onLoginResult(StorePassword.load().LastLoginId, (AccountType)user_name.SelectedIndex);
            }

            gSingleForm = null;
        }


        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Logger.Info("LoginForm_FormClosing(), SelectedIndex:" + user_name.SelectedIndex + ", " + UserNoText.Text);
        }

        private void Again_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SureButton.PerformClick();
        }


        private static Form gSingleForm;
        internal static Form show(ILoginResultListener listener)
        {
            lock (typeof(LoginForm))
            {
                if (gSingleForm == null)
                {
                    gSingleForm = new LoginForm(listener);
                }

                gSingleForm.Show();
            }

            return gSingleForm;
        }

        internal static DialogResult showDialog(ILoginResultListener listener)
        {
            lock (typeof(LoginForm))
            {
                if (gSingleForm == null)
                {
                    gSingleForm = new LoginForm(listener);
                }

                return gSingleForm.ShowDialog();
            }
        }

        private void UserNoText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if((AccountType)user_name.SelectedIndex == AccountType.Guest)
                    this.login.PerformClick();
                else
                    this.password.Focus();
            }
        }
    }
}
