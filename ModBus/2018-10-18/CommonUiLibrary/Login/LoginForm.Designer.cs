namespace Ksat.AppPlugInUiLibrary.Login
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.UserRegist = new System.Windows.Forms.Panel();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Again = new System.Windows.Forms.TextBox();
            this.SureButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CharNew = new System.Windows.Forms.TextBox();
            this.CharLast = new System.Windows.Forms.TextBox();
            this.UserLog = new System.Windows.Forms.Panel();
            this.UserNoText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.InformationLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.user_name = new System.Windows.Forms.ComboBox();
            this.PassChange = new System.Windows.Forms.LinkLabel();
            this.login = new System.Windows.Forms.Button();
            this.char_panel = new System.Windows.Forms.Panel();
            this.password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.UserRegist.SuspendLayout();
            this.UserLog.SuspendLayout();
            this.char_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserRegist
            // 
            this.UserRegist.Controls.Add(this.InfoLabel);
            this.UserRegist.Controls.Add(this.label5);
            this.UserRegist.Controls.Add(this.Again);
            this.UserRegist.Controls.Add(this.SureButton);
            this.UserRegist.Controls.Add(this.CancelButton);
            this.UserRegist.Controls.Add(this.label4);
            this.UserRegist.Controls.Add(this.label3);
            this.UserRegist.Controls.Add(this.CharNew);
            this.UserRegist.Controls.Add(this.CharLast);
            resources.ApplyResources(this.UserRegist, "UserRegist");
            this.UserRegist.Name = "UserRegist";
            // 
            // InfoLabel
            // 
            resources.ApplyResources(this.InfoLabel, "InfoLabel");
            this.InfoLabel.ForeColor = System.Drawing.Color.Red;
            this.InfoLabel.Name = "InfoLabel";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // Again
            // 
            resources.ApplyResources(this.Again, "Again");
            this.Again.Name = "Again";
            this.Again.UseSystemPasswordChar = true;
            this.Again.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Again_KeyPress);
            // 
            // SureButton
            // 
            resources.ApplyResources(this.SureButton, "SureButton");
            this.SureButton.Name = "SureButton";
            this.SureButton.UseVisualStyleBackColor = true;
            this.SureButton.Click += new System.EventHandler(this.SureButton_Click);
            // 
            // CancelButton
            // 
            resources.ApplyResources(this.CancelButton, "CancelButton");
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // CharNew
            // 
            resources.ApplyResources(this.CharNew, "CharNew");
            this.CharNew.Name = "CharNew";
            this.CharNew.UseSystemPasswordChar = true;
            // 
            // CharLast
            // 
            resources.ApplyResources(this.CharLast, "CharLast");
            this.CharLast.Name = "CharLast";
            this.CharLast.UseSystemPasswordChar = true;
            // 
            // UserLog
            // 
            this.UserLog.Controls.Add(this.UserNoText);
            this.UserLog.Controls.Add(this.label6);
            this.UserLog.Controls.Add(this.InformationLabel);
            this.UserLog.Controls.Add(this.label1);
            this.UserLog.Controls.Add(this.user_name);
            this.UserLog.Controls.Add(this.PassChange);
            this.UserLog.Controls.Add(this.login);
            this.UserLog.Controls.Add(this.char_panel);
            this.UserLog.Controls.Add(this.cancel);
            resources.ApplyResources(this.UserLog, "UserLog");
            this.UserLog.Name = "UserLog";
            // 
            // UserNoText
            // 
            resources.ApplyResources(this.UserNoText, "UserNoText");
            this.UserNoText.Name = "UserNoText";
            this.UserNoText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserNoText_KeyPress);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // InformationLabel
            // 
            resources.ApplyResources(this.InformationLabel, "InformationLabel");
            this.InformationLabel.ForeColor = System.Drawing.Color.Red;
            this.InformationLabel.Name = "InformationLabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // user_name
            // 
            this.user_name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.user_name.FormattingEnabled = true;
            resources.ApplyResources(this.user_name, "user_name");
            this.user_name.Name = "user_name";
            this.user_name.SelectedIndexChanged += new System.EventHandler(this.user_name_SelectedIndexChanged);
            // 
            // PassChange
            // 
            resources.ApplyResources(this.PassChange, "PassChange");
            this.PassChange.Name = "PassChange";
            this.PassChange.TabStop = true;
            this.PassChange.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.PassChange_LinkClicked);
            // 
            // login
            // 
            resources.ApplyResources(this.login, "login");
            this.login.Name = "login";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // char_panel
            // 
            this.char_panel.Controls.Add(this.password);
            this.char_panel.Controls.Add(this.label2);
            resources.ApplyResources(this.char_panel, "char_panel");
            this.char_panel.Name = "char_panel";
            // 
            // password
            // 
            resources.ApplyResources(this.password, "password");
            this.password.Name = "password";
            this.password.UseSystemPasswordChar = true;
            this.password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.password_KeyPress);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // LoginForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UserRegist);
            this.Controls.Add(this.UserLog);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoginForm_FormClosed);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.UserRegist.ResumeLayout(false);
            this.UserRegist.PerformLayout();
            this.UserLog.ResumeLayout(false);
            this.UserLog.PerformLayout();
            this.char_panel.ResumeLayout(false);
            this.char_panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel UserRegist;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Again;
        private System.Windows.Forms.Button SureButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CharNew;
        private System.Windows.Forms.TextBox CharLast;
        private System.Windows.Forms.Panel UserLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel PassChange;
        private System.Windows.Forms.Button login;
        private System.Windows.Forms.Panel char_panel;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label InformationLabel;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.TextBox UserNoText;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox user_name;
    }
}