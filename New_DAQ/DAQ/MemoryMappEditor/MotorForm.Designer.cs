namespace MemoryMappEditor
{
    partial class MotorForm
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
            this.AddAxis = new System.Windows.Forms.Button();
            this.AddTimeargs = new System.Windows.Forms.Button();
            this.AddSystemArgs = new System.Windows.Forms.Button();
            this.MachineDefine = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "轴";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // AddAxis
            // 
            this.AddAxis.Location = new System.Drawing.Point(130, 26);
            this.AddAxis.Name = "AddAxis";
            this.AddAxis.Size = new System.Drawing.Size(75, 23);
            this.AddAxis.TabIndex = 1;
            this.AddAxis.Text = "添加轴";
            this.AddAxis.UseVisualStyleBackColor = true;
            this.AddAxis.Click += new System.EventHandler(this.AddAxis_Click);
            // 
            // AddTimeargs
            // 
            this.AddTimeargs.Location = new System.Drawing.Point(130, 106);
            this.AddTimeargs.Name = "AddTimeargs";
            this.AddTimeargs.Size = new System.Drawing.Size(162, 23);
            this.AddTimeargs.TabIndex = 2;
            this.AddTimeargs.Text = "添加时间参数";
            this.AddTimeargs.UseVisualStyleBackColor = true;
            this.AddTimeargs.Click += new System.EventHandler(this.AddTimeargs_Click);
            // 
            // AddSystemArgs
            // 
            this.AddSystemArgs.Location = new System.Drawing.Point(130, 183);
            this.AddSystemArgs.Name = "AddSystemArgs";
            this.AddSystemArgs.Size = new System.Drawing.Size(162, 23);
            this.AddSystemArgs.TabIndex = 3;
            this.AddSystemArgs.Text = "添加系统参数";
            this.AddSystemArgs.UseVisualStyleBackColor = true;
            this.AddSystemArgs.Click += new System.EventHandler(this.AddSystemArgs_Click);
            // 
            // MachineDefine
            // 
            this.MachineDefine.Location = new System.Drawing.Point(130, 265);
            this.MachineDefine.Name = "MachineDefine";
            this.MachineDefine.Size = new System.Drawing.Size(162, 23);
            this.MachineDefine.TabIndex = 4;
            this.MachineDefine.Text = "机台自定义";
            this.MachineDefine.UseVisualStyleBackColor = true;
            this.MachineDefine.Click += new System.EventHandler(this.MachineDefine_Click);
            // 
            // MotorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 449);
            this.Controls.Add(this.MachineDefine);
            this.Controls.Add(this.AddSystemArgs);
            this.Controls.Add(this.AddTimeargs);
            this.Controls.Add(this.AddAxis);
            this.Controls.Add(this.label1);
            this.Name = "MotorForm";
            this.Text = "马达参数";
            this.Load += new System.EventHandler(this.MotorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddAxis;
        private System.Windows.Forms.Button AddTimeargs;
        private System.Windows.Forms.Button AddSystemArgs;
        private System.Windows.Forms.Button MachineDefine;
    }
}