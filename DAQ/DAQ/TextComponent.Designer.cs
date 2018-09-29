namespace DAQ
{
    partial class TextComponent
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
            this.add = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.offset = new System.Windows.Forms.TextBox();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.save = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.SuspendLayout();
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(214, 171);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(75, 23);
            this.add.TabIndex = 0;
            this.add.Text = "添加";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.onAddTextComponent);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(420, 171);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "复位";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.onReset);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "_uint_16,",
            "_int_16,",
            "_unit_32,",
            "_int32"});
            this.comboBox1.Location = new System.Drawing.Point(419, 65);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(173, 71);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(119, 19);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "是否允许输入";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(345, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "数据类型";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "偏移量";
            // 
            // offset
            // 
            this.offset.Location = new System.Drawing.Point(235, 107);
            this.offset.Name = "offset";
            this.offset.Size = new System.Drawing.Size(100, 25);
            this.offset.TabIndex = 6;
            this.offset.TextChanged += new System.EventHandler(this.offset_TextChanged);
            this.offset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.offset_KeyPress);
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            this.eventLog1.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.eventLog1_EntryWritten);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(600, 170);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 7;
            this.save.Text = "保存";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // remove
            // 
            this.remove.Location = new System.Drawing.Point(319, 170);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(75, 23);
            this.remove.TabIndex = 8;
            this.remove.Text = "刪除";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // TextComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 238);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.save);
            this.Controls.Add(this.offset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.add);
            this.Location = new System.Drawing.Point(800, 500);
            this.Name = "TextComponent";
            this.Text = "新增文本";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextComponent_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox offset;
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button save;
    }
}