namespace MemoryEditor
{
    partial class FormSetting
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
            this.txtEventName = new System.Windows.Forms.TextBox();
            this.txtMemoryBegin = new System.Windows.Forms.TextBox();
            this.txtMemoryEnd = new System.Windows.Forms.TextBox();
            this.txtDataSize = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "EventName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "MemoryBegin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "MemoryEnd";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "DataSize";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 265);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Description";
            // 
            // txtEventName
            // 
            this.txtEventName.Location = new System.Drawing.Point(201, 36);
            this.txtEventName.Name = "txtEventName";
            this.txtEventName.Size = new System.Drawing.Size(100, 25);
            this.txtEventName.TabIndex = 5;
            this.txtEventName.Text = " ";
            // 
            // txtMemoryBegin
            // 
            this.txtMemoryBegin.Location = new System.Drawing.Point(201, 93);
            this.txtMemoryBegin.Name = "txtMemoryBegin";
            this.txtMemoryBegin.Size = new System.Drawing.Size(100, 25);
            this.txtMemoryBegin.TabIndex = 6;
            // 
            // txtMemoryEnd
            // 
            this.txtMemoryEnd.Location = new System.Drawing.Point(201, 153);
            this.txtMemoryEnd.Name = "txtMemoryEnd";
            this.txtMemoryEnd.Size = new System.Drawing.Size(100, 25);
            this.txtMemoryEnd.TabIndex = 7;
            // 
            // txtDataSize
            // 
            this.txtDataSize.Location = new System.Drawing.Point(201, 212);
            this.txtDataSize.Name = "txtDataSize";
            this.txtDataSize.Size = new System.Drawing.Size(100, 25);
            this.txtDataSize.TabIndex = 8;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(201, 262);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(100, 25);
            this.txtDescription.TabIndex = 9;
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(416, 110);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 11;
            this.Ok.Text = "确定";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // FormAttributeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 324);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtDataSize);
            this.Controls.Add(this.txtMemoryEnd);
            this.Controls.Add(this.txtMemoryBegin);
            this.Controls.Add(this.txtEventName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormAttributeEdit";
            this.Text = "编辑二级菜单";
            this.Load += new System.EventHandler(this.FormAttributeEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEventName;
        private System.Windows.Forms.TextBox txtMemoryBegin;
        private System.Windows.Forms.TextBox txtMemoryEnd;
        private System.Windows.Forms.TextBox txtDataSize;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button Ok;
    }
}