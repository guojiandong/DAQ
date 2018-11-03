namespace MemoryEditor
{
    partial class FormSubSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Ok = new System.Windows.Forms.Button();
            this.txtEventName = new System.Windows.Forms.TextBox();
            this.txtMemoryBegin = new System.Windows.Forms.TextBox();
            this.txtMemoryEnd = new System.Windows.Forms.TextBox();
            this.txtDataSize = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "EventName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "MemoryBegin";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "MemoryEnd";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "DataSize";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Description";
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(394, 106);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 5;
            this.Ok.Text = "确定";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // txtEventName
            // 
            this.txtEventName.Location = new System.Drawing.Point(156, 24);
            this.txtEventName.Name = "txtEventName";
            this.txtEventName.Size = new System.Drawing.Size(100, 25);
            this.txtEventName.TabIndex = 6;
            // 
            // txtMemoryBegin
            // 
            this.txtMemoryBegin.Location = new System.Drawing.Point(156, 77);
            this.txtMemoryBegin.Name = "txtMemoryBegin";
            this.txtMemoryBegin.Size = new System.Drawing.Size(100, 25);
            this.txtMemoryBegin.TabIndex = 7;
            // 
            // txtMemoryEnd
            // 
            this.txtMemoryEnd.Location = new System.Drawing.Point(156, 123);
            this.txtMemoryEnd.Name = "txtMemoryEnd";
            this.txtMemoryEnd.Size = new System.Drawing.Size(100, 25);
            this.txtMemoryEnd.TabIndex = 8;
            this.txtMemoryEnd.TextChanged += new System.EventHandler(this.txtMemoryEnd_TextChanged);
            // 
            // txtDataSize
            // 
            this.txtDataSize.Location = new System.Drawing.Point(155, 166);
            this.txtDataSize.Name = "txtDataSize";
            this.txtDataSize.Size = new System.Drawing.Size(100, 25);
            this.txtDataSize.TabIndex = 9;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(156, 208);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(100, 25);
            this.txtDescription.TabIndex = 10;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtEventName);
            this.panel1.Controls.Add(this.Ok);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.txtMemoryBegin);
            this.panel1.Controls.Add(this.txtDataSize);
            this.panel1.Controls.Add(this.txtMemoryEnd);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(3, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 248);
            this.panel1.TabIndex = 11;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "编辑三级菜单参数";
            // 
            // FormSubSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 277);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Name = "FormSubSetting";
            this.Text = "编辑三级菜单";
            this.Load += new System.EventHandler(this.FormSubSetting_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.TextBox txtEventName;
        private System.Windows.Forms.TextBox txtMemoryBegin;
        private System.Windows.Forms.TextBox txtMemoryEnd;
        private System.Windows.Forms.TextBox txtDataSize;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
    }
}