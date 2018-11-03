namespace DAQ
{
    partial class BtnComponent
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
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.note = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.remove = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.in_word_offset = new System.Windows.Forms.TextBox();
            this.in_bit_offset = new System.Windows.Forms.TextBox();
            this.out_word_offset = new System.Windows.Forms.TextBox();
            this.out_bit_offset = new System.Windows.Forms.TextBox();
            this.in_word_offset_label = new System.Windows.Forms.Label();
            this.in_bit_offset_label = new System.Windows.Forms.Label();
            this.out_bit_offset_label = new System.Windows.Forms.Label();
            this.out_word_offset_label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pressLabel = new System.Windows.Forms.Label();
            this.pressstate = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(696, 59);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(75, 23);
            this.add.TabIndex = 0;
            this.add.Text = "添加";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.onAddBtnComponent);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(696, 105);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "复位";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.onResetBtn);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(36, 61);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(179, 19);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "寫入讀取是都相同地址";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "_On = 0,",
            "_Off = 1,",
            "_Switch = 2,",
            "_Reversion = 3,"});
            this.comboBox1.Location = new System.Drawing.Point(312, 63);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "操作类型";
            // 
            // note
            // 
            this.note.Location = new System.Drawing.Point(524, 61);
            this.note.Name = "note";
            this.note.Size = new System.Drawing.Size(100, 25);
            this.note.TabIndex = 7;
            this.note.TextChanged += new System.EventHandler(this.note_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(481, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "注释";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // remove
            // 
            this.remove.Location = new System.Drawing.Point(696, 150);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(75, 23);
            this.remove.TabIndex = 9;
            this.remove.Text = "删除";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(696, 199);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 10;
            this.save.Text = "保存";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // in_word_offset
            // 
            this.in_word_offset.Location = new System.Drawing.Point(121, 219);
            this.in_word_offset.Name = "in_word_offset";
            this.in_word_offset.Size = new System.Drawing.Size(100, 25);
            this.in_word_offset.TabIndex = 11;
            this.in_word_offset.TextChanged += new System.EventHandler(this.in_word_offset_TextChanged);
            this.in_word_offset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.in_word_offset_KeyPress);
            // 
            // in_bit_offset
            // 
            this.in_bit_offset.Location = new System.Drawing.Point(325, 217);
            this.in_bit_offset.Name = "in_bit_offset";
            this.in_bit_offset.Size = new System.Drawing.Size(100, 25);
            this.in_bit_offset.TabIndex = 12;
            this.in_bit_offset.TextChanged += new System.EventHandler(this.in_bit_offset_TextChanged);
            this.in_bit_offset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.in_bit_offset_KeyPress);
            // 
            // out_word_offset
            // 
            this.out_word_offset.Location = new System.Drawing.Point(121, 170);
            this.out_word_offset.Name = "out_word_offset";
            this.out_word_offset.Size = new System.Drawing.Size(100, 25);
            this.out_word_offset.TabIndex = 13;
            this.out_word_offset.TextChanged += new System.EventHandler(this.out_word_offset_TextChanged);
            this.out_word_offset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.out_word_offset_KeyPress);
            // 
            // out_bit_offset
            // 
            this.out_bit_offset.Location = new System.Drawing.Point(325, 163);
            this.out_bit_offset.Name = "out_bit_offset";
            this.out_bit_offset.Size = new System.Drawing.Size(100, 25);
            this.out_bit_offset.TabIndex = 14;
            this.out_bit_offset.TextChanged += new System.EventHandler(this.out_bit_offset_TextChanged);
            this.out_bit_offset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.out_bit_offset_KeyPress);
            // 
            // in_word_offset_label
            // 
            this.in_word_offset_label.AutoSize = true;
            this.in_word_offset_label.Location = new System.Drawing.Point(33, 222);
            this.in_word_offset_label.Name = "in_word_offset_label";
            this.in_word_offset_label.Size = new System.Drawing.Size(82, 15);
            this.in_word_offset_label.TabIndex = 15;
            this.in_word_offset_label.Text = "寫入字偏移";
            this.in_word_offset_label.Click += new System.EventHandler(this.in_word_offset_label_Click);
            // 
            // in_bit_offset_label
            // 
            this.in_bit_offset_label.AutoSize = true;
            this.in_bit_offset_label.Location = new System.Drawing.Point(237, 222);
            this.in_bit_offset_label.Name = "in_bit_offset_label";
            this.in_bit_offset_label.Size = new System.Drawing.Size(82, 15);
            this.in_bit_offset_label.TabIndex = 16;
            this.in_bit_offset_label.Text = "寫入位偏移";
            this.in_bit_offset_label.Click += new System.EventHandler(this.label5_Click);
            // 
            // out_bit_offset_label
            // 
            this.out_bit_offset_label.AutoSize = true;
            this.out_bit_offset_label.Location = new System.Drawing.Point(237, 173);
            this.out_bit_offset_label.Name = "out_bit_offset_label";
            this.out_bit_offset_label.Size = new System.Drawing.Size(82, 15);
            this.out_bit_offset_label.TabIndex = 17;
            this.out_bit_offset_label.Text = "讀取位偏移";
            // 
            // out_word_offset_label
            // 
            this.out_word_offset_label.AutoSize = true;
            this.out_word_offset_label.Location = new System.Drawing.Point(33, 173);
            this.out_word_offset_label.Name = "out_word_offset_label";
            this.out_word_offset_label.Size = new System.Drawing.Size(82, 15);
            this.out_word_offset_label.TabIndex = 18;
            this.out_word_offset_label.Text = "讀取字偏移";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(448, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "（Btn類型專用）";
            this.label5.Click += new System.EventHandler(this.label5_Click_1);
            // 
            // pressLabel
            // 
            this.pressLabel.AutoSize = true;
            this.pressLabel.Location = new System.Drawing.Point(446, 126);
            this.pressLabel.Name = "pressLabel";
            this.pressLabel.Size = new System.Drawing.Size(67, 15);
            this.pressLabel.TabIndex = 22;
            this.pressLabel.Text = "按钮状态";
            // 
            // pressstate
            // 
            this.pressstate.FormattingEnabled = true;
            this.pressstate.Items.AddRange(new object[] {
            "auto_up",
            "keep"});
            this.pressstate.Location = new System.Drawing.Point(524, 117);
            this.pressstate.Name = "pressstate";
            this.pressstate.Size = new System.Drawing.Size(121, 23);
            this.pressstate.TabIndex = 23;
            this.pressstate.SelectedIndexChanged += new System.EventHandler(this.pressstate_SelectedIndexChanged);
            // 
            // BtnComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 342);
            this.Controls.Add(this.pressstate);
            this.Controls.Add(this.pressLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.out_word_offset_label);
            this.Controls.Add(this.out_bit_offset_label);
            this.Controls.Add(this.in_bit_offset_label);
            this.Controls.Add(this.in_word_offset_label);
            this.Controls.Add(this.out_bit_offset);
            this.Controls.Add(this.out_word_offset);
            this.Controls.Add(this.in_bit_offset);
            this.Controls.Add(this.in_word_offset);
            this.Controls.Add(this.save);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.note);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.add);
            this.Name = "BtnComponent";
            this.Text = "新增按钮";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BtnComponent_FormClosing);
            this.Load += new System.EventHandler(this.BtnComponent_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox note;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.TextBox in_word_offset;
        private System.Windows.Forms.TextBox in_bit_offset;
        private System.Windows.Forms.TextBox out_word_offset;
        private System.Windows.Forms.TextBox out_bit_offset;
        private System.Windows.Forms.Label in_word_offset_label;
        private System.Windows.Forms.Label in_bit_offset_label;
        private System.Windows.Forms.Label out_bit_offset_label;
        private System.Windows.Forms.Label out_word_offset_label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label pressLabel;
        private System.Windows.Forms.ComboBox pressstate;
    }
}