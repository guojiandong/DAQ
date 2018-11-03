namespace MemoryMappEditor
{
    partial class EventForm
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
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.txtDataSize = new System.Windows.Forms.TextBox();
            this.txtStationID = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Ok = new System.Windows.Forms.Button();
            this.editSubItem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Version";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "EventCode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "DataSize";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 245);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "StationID";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(157, 45);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(100, 25);
            this.txtVersion.TabIndex = 4;
            this.txtVersion.Enter += new System.EventHandler(this.txtVersion_Enter);
            this.txtVersion.Leave += new System.EventHandler(this.txtVersion_Leave);
            // 
            // txtDataSize
            // 
            this.txtDataSize.Location = new System.Drawing.Point(157, 169);
            this.txtDataSize.Name = "txtDataSize";
            this.txtDataSize.Size = new System.Drawing.Size(100, 25);
            this.txtDataSize.TabIndex = 6;
            this.txtDataSize.Enter += new System.EventHandler(this.txtDataSize_Enter);
            this.txtDataSize.Leave += new System.EventHandler(this.txtDataSize_Leave);
            // 
            // txtStationID
            // 
            this.txtStationID.Location = new System.Drawing.Point(157, 242);
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(100, 25);
            this.txtStationID.TabIndex = 7;
            this.txtStationID.Enter += new System.EventHandler(this.txtStationID_Enter);
            this.txtStationID.Leave += new System.EventHandler(this.txtStationID_Leave);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Rigseter = 0x2                      ",
            "EapCommand = 0x3",
            "AlarmText = 0x4",
            "MotorSettings = 0x5",
            "NormalSettings = 0x6",
            "HeartBeat = 0x7",
            "EventChangedFlag = 0x8",
            "BreakPoint = 0x9",
            "AlarmCode = 0xA",
            "MachineStatus = 0xB",
            "CheckIn = 0xC",
            " CheckOut = 0xD"});
            this.comboBox1.Location = new System.Drawing.Point(157, 108);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(245, 23);
            this.comboBox1.TabIndex = 8;
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(447, 44);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 9;
            this.Ok.Text = "确定";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // editSubItem
            // 
            this.editSubItem.Location = new System.Drawing.Point(447, 237);
            this.editSubItem.Name = "editSubItem";
            this.editSubItem.Size = new System.Drawing.Size(127, 23);
            this.editSubItem.TabIndex = 10;
            this.editSubItem.Text = "编辑二级菜单";
            this.editSubItem.UseVisualStyleBackColor = true;
            this.editSubItem.Click += new System.EventHandler(this.editSubItem_Click);
            // 
            // EventForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 331);
            this.Controls.Add(this.editSubItem);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtStationID);
            this.Controls.Add(this.txtDataSize);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "EventForm";
            this.Text = "EventForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.TextBox txtDataSize;
        private System.Windows.Forms.TextBox txtStationID;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button editSubItem;
    }
}