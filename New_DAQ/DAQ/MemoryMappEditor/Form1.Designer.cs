namespace MemoryMappEditor
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.BaseHeaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rigsterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.eapCommandToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.alarmTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.heartBeatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.heartBeatToolStripMenuItemTop = new System.Windows.Forms.ToolStripMenuItem();
            this.motorSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.normalSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.eventChangedFlagToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.breakPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.alarmCodeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.machineStatusToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FixedListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.EventDefineListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Fixed_Up = new System.Windows.Forms.Button();
            this.Fixed_Down = new System.Windows.Forms.Button();
            this.Define_Up = new System.Windows.Forms.Button();
            this.Define_Down = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BaseHeaderToolStripMenuItem,
            this.registerToolStripMenuItem,
            this.heartBeatToolStripMenuItemTop,
            this.checkInToolStripMenuItem,
            this.checkOutToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.exportExcelToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(884, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // BaseHeaderToolStripMenuItem
            // 
            this.BaseHeaderToolStripMenuItem.Name = "BaseHeaderToolStripMenuItem";
            this.BaseHeaderToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.BaseHeaderToolStripMenuItem.Text = "Header";
            this.BaseHeaderToolStripMenuItem.Click += new System.EventHandler(this.BaseHeaderToolStripMenuItem_Click);
            // 
            // registerToolStripMenuItem
            // 
            this.registerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rigsterToolStripMenuItem,
            this.toolStripSeparator2,
            this.eapCommandToolStripMenuItem1,
            this.toolStripSeparator1,
            this.alarmTextToolStripMenuItem,
            this.toolStripSeparator3,
            this.heartBeatToolStripMenuItem});
            this.registerToolStripMenuItem.Name = "registerToolStripMenuItem";
            this.registerToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.registerToolStripMenuItem.Text = "EAP->PLC";
            // 
            // rigsterToolStripMenuItem
            // 
            this.rigsterToolStripMenuItem.Name = "rigsterToolStripMenuItem";
            this.rigsterToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.rigsterToolStripMenuItem.Text = "Register";
            this.rigsterToolStripMenuItem.Click += new System.EventHandler(this.rigsterToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
            // 
            // eapCommandToolStripMenuItem1
            // 
            this.eapCommandToolStripMenuItem1.Name = "eapCommandToolStripMenuItem1";
            this.eapCommandToolStripMenuItem1.Size = new System.Drawing.Size(185, 26);
            this.eapCommandToolStripMenuItem1.Text = "EapCommand";
            this.eapCommandToolStripMenuItem1.Click += new System.EventHandler(this.eapCommandToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // alarmTextToolStripMenuItem
            // 
            this.alarmTextToolStripMenuItem.Name = "alarmTextToolStripMenuItem";
            this.alarmTextToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.alarmTextToolStripMenuItem.Text = "AlarmText";
            this.alarmTextToolStripMenuItem.Click += new System.EventHandler(this.alarmTextToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
            // 
            // heartBeatToolStripMenuItem
            // 
            this.heartBeatToolStripMenuItem.Name = "heartBeatToolStripMenuItem";
            this.heartBeatToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.heartBeatToolStripMenuItem.Text = "HeartBeat";
            this.heartBeatToolStripMenuItem.Click += new System.EventHandler(this.heartBeatToolStripMenuItem_Click);
            // 
            // heartBeatToolStripMenuItemTop
            // 
            this.heartBeatToolStripMenuItemTop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.motorSettingsToolStripMenuItem,
            this.toolStripSeparator4,
            this.normalSettingsToolStripMenuItem,
            this.toolStripSeparator5,
            this.eventChangedFlagToolStripMenuItem1,
            this.toolStripSeparator6,
            this.breakPointToolStripMenuItem,
            this.toolStripSeparator7,
            this.alarmCodeToolStripMenuItem1,
            this.toolStripSeparator8,
            this.machineStatusToolStripMenuItem1});
            this.heartBeatToolStripMenuItemTop.Name = "heartBeatToolStripMenuItemTop";
            this.heartBeatToolStripMenuItemTop.Size = new System.Drawing.Size(93, 24);
            this.heartBeatToolStripMenuItemTop.Text = "PLC->EAP";
            // 
            // motorSettingsToolStripMenuItem
            // 
            this.motorSettingsToolStripMenuItem.Name = "motorSettingsToolStripMenuItem";
            this.motorSettingsToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.motorSettingsToolStripMenuItem.Text = "MotorSettings";
            this.motorSettingsToolStripMenuItem.Click += new System.EventHandler(this.motorSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(216, 6);
            // 
            // normalSettingsToolStripMenuItem
            // 
            this.normalSettingsToolStripMenuItem.Name = "normalSettingsToolStripMenuItem";
            this.normalSettingsToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.normalSettingsToolStripMenuItem.Text = "NormalSettings";
            this.normalSettingsToolStripMenuItem.Click += new System.EventHandler(this.normalSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(216, 6);
            // 
            // eventChangedFlagToolStripMenuItem1
            // 
            this.eventChangedFlagToolStripMenuItem1.Name = "eventChangedFlagToolStripMenuItem1";
            this.eventChangedFlagToolStripMenuItem1.Size = new System.Drawing.Size(219, 26);
            this.eventChangedFlagToolStripMenuItem1.Text = "EventChangedFlag";
            this.eventChangedFlagToolStripMenuItem1.Click += new System.EventHandler(this.eventChangedFlagToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(216, 6);
            // 
            // breakPointToolStripMenuItem
            // 
            this.breakPointToolStripMenuItem.Name = "breakPointToolStripMenuItem";
            this.breakPointToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.breakPointToolStripMenuItem.Text = "BreakPoint";
            this.breakPointToolStripMenuItem.Click += new System.EventHandler(this.breakPointToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(216, 6);
            // 
            // alarmCodeToolStripMenuItem1
            // 
            this.alarmCodeToolStripMenuItem1.Name = "alarmCodeToolStripMenuItem1";
            this.alarmCodeToolStripMenuItem1.Size = new System.Drawing.Size(219, 26);
            this.alarmCodeToolStripMenuItem1.Text = "AlarmCode";
            this.alarmCodeToolStripMenuItem1.Click += new System.EventHandler(this.alarmCodeToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(216, 6);
            // 
            // machineStatusToolStripMenuItem1
            // 
            this.machineStatusToolStripMenuItem1.Name = "machineStatusToolStripMenuItem1";
            this.machineStatusToolStripMenuItem1.Size = new System.Drawing.Size(219, 26);
            this.machineStatusToolStripMenuItem1.Text = "MachineStatus";
            this.machineStatusToolStripMenuItem1.Click += new System.EventHandler(this.machineStatusToolStripMenuItem_Click);
            // 
            // checkInToolStripMenuItem
            // 
            this.checkInToolStripMenuItem.Name = "checkInToolStripMenuItem";
            this.checkInToolStripMenuItem.Size = new System.Drawing.Size(78, 24);
            this.checkInToolStripMenuItem.Text = "CheckIn";
            this.checkInToolStripMenuItem.Click += new System.EventHandler(this.checkInToolStripMenuItem_Click);
            // 
            // checkOutToolStripMenuItem
            // 
            this.checkOutToolStripMenuItem.Name = "checkOutToolStripMenuItem";
            this.checkOutToolStripMenuItem.Size = new System.Drawing.Size(92, 24);
            this.checkOutToolStripMenuItem.Text = "CheckOut";
            this.checkOutToolStripMenuItem.Click += new System.EventHandler(this.checkOutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readMeToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // readMeToolStripMenuItem
            // 
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            this.readMeToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.readMeToolStripMenuItem.Text = "ReadMe";
            // 
            // exportExcelToolStripMenuItem
            // 
            this.exportExcelToolStripMenuItem.Name = "exportExcelToolStripMenuItem";
            this.exportExcelToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.exportExcelToolStripMenuItem.Text = "ExportExcel";
            this.exportExcelToolStripMenuItem.Click += new System.EventHandler(this.exportExcelToolStripMenuItem_Click);
            // 
            // FixedListBox
            // 
            this.FixedListBox.FormattingEnabled = true;
            this.FixedListBox.ItemHeight = 15;
            this.FixedListBox.Location = new System.Drawing.Point(27, 66);
            this.FixedListBox.Name = "FixedListBox";
            this.FixedListBox.Size = new System.Drawing.Size(606, 154);
            this.FixedListBox.TabIndex = 1;
            this.FixedListBox.SelectedIndexChanged += new System.EventHandler(this.FixedListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "固定事件";
            // 
            // EventDefineListBox
            // 
            this.EventDefineListBox.FormattingEnabled = true;
            this.EventDefineListBox.ItemHeight = 15;
            this.EventDefineListBox.Location = new System.Drawing.Point(27, 269);
            this.EventDefineListBox.Name = "EventDefineListBox";
            this.EventDefineListBox.Size = new System.Drawing.Size(603, 169);
            this.EventDefineListBox.TabIndex = 3;
            this.EventDefineListBox.SelectedIndexChanged += new System.EventHandler(this.EventDefineListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "自定义事件区";
            // 
            // Fixed_Up
            // 
            this.Fixed_Up.Location = new System.Drawing.Point(709, 81);
            this.Fixed_Up.Name = "Fixed_Up";
            this.Fixed_Up.Size = new System.Drawing.Size(75, 23);
            this.Fixed_Up.TabIndex = 5;
            this.Fixed_Up.Text = "上移";
            this.Fixed_Up.UseVisualStyleBackColor = true;
            // 
            // Fixed_Down
            // 
            this.Fixed_Down.Location = new System.Drawing.Point(709, 139);
            this.Fixed_Down.Name = "Fixed_Down";
            this.Fixed_Down.Size = new System.Drawing.Size(75, 23);
            this.Fixed_Down.TabIndex = 6;
            this.Fixed_Down.Text = "下移";
            this.Fixed_Down.UseVisualStyleBackColor = true;
            // 
            // Define_Up
            // 
            this.Define_Up.Location = new System.Drawing.Point(709, 311);
            this.Define_Up.Name = "Define_Up";
            this.Define_Up.Size = new System.Drawing.Size(75, 23);
            this.Define_Up.TabIndex = 7;
            this.Define_Up.Text = "上移";
            this.Define_Up.UseVisualStyleBackColor = true;
            // 
            // Define_Down
            // 
            this.Define_Down.Location = new System.Drawing.Point(709, 371);
            this.Define_Down.Name = "Define_Down";
            this.Define_Down.Size = new System.Drawing.Size(75, 23);
            this.Define_Down.TabIndex = 8;
            this.Define_Down.Text = "下移";
            this.Define_Down.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 464);
            this.Controls.Add(this.Define_Down);
            this.Controls.Add(this.Define_Up);
            this.Controls.Add(this.Fixed_Down);
            this.Controls.Add(this.Fixed_Up);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EventDefineListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FixedListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "编辑器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem BaseHeaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem heartBeatToolStripMenuItemTop;
        private System.Windows.Forms.ToolStripMenuItem checkInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rigsterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eapCommandToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem alarmTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem heartBeatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem motorSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eventChangedFlagToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem breakPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alarmCodeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem machineStatusToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readMeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportExcelToolStripMenuItem;
        private System.Windows.Forms.ListBox FixedListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox EventDefineListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Fixed_Up;
        private System.Windows.Forms.Button Fixed_Down;
        private System.Windows.Forms.Button Define_Up;
        private System.Windows.Forms.Button Define_Down;
    }
}

