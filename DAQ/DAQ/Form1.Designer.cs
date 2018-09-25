namespace DAQ
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Add = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.exportXml = new System.Windows.Forms.Button();
            this.importXml = new System.Windows.Forms.Button();
            this.ClearListView = new System.Windows.Forms.Button();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader9,
            this.columnHeader11,
            this.columnHeader12});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(-1, 1);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(829, 446);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.onDouleClickItem);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "是否输入";
            this.columnHeader2.Width = 137;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "数据类型";
            this.columnHeader3.Width = 136;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "操作类型";
            this.columnHeader4.Width = 117;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "偏移量";
            this.columnHeader5.Width = 87;
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(849, 28);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(104, 23);
            this.Add.TabIndex = 1;
            this.Add.Text = "新增文本";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.onAddTextComponent);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(849, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "新增按鈕";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.onAddBtnComponent);
            // 
            // exportXml
            // 
            this.exportXml.Location = new System.Drawing.Point(872, 260);
            this.exportXml.Name = "exportXml";
            this.exportXml.Size = new System.Drawing.Size(75, 23);
            this.exportXml.TabIndex = 4;
            this.exportXml.Text = "導出xml";
            this.exportXml.UseVisualStyleBackColor = true;
            this.exportXml.Click += new System.EventHandler(this.exportXml_Click);
            // 
            // importXml
            // 
            this.importXml.Location = new System.Drawing.Point(873, 308);
            this.importXml.Name = "importXml";
            this.importXml.Size = new System.Drawing.Size(75, 23);
            this.importXml.TabIndex = 6;
            this.importXml.Text = "導入xml";
            this.importXml.UseVisualStyleBackColor = true;
            this.importXml.Click += new System.EventHandler(this.importXml_Click);
            // 
            // ClearListView
            // 
            this.ClearListView.Location = new System.Drawing.Point(873, 360);
            this.ClearListView.Name = "ClearListView";
            this.ClearListView.Size = new System.Drawing.Size(75, 23);
            this.ClearListView.TabIndex = 7;
            this.ClearListView.Text = "清空列表";
            this.ClearListView.UseVisualStyleBackColor = true;
            this.ClearListView.Click += new System.EventHandler(this.ClearListView_Click);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "寫入字偏移";
            this.columnHeader6.Width = 96;
            // 
            // columnHeader7
            // 
            this.columnHeader7.DisplayIndex = 7;
            this.columnHeader7.Text = "寫入位偏移";
            this.columnHeader7.Width = 92;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "讀出字偏移";
            this.columnHeader9.Width = 90;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "讀出位偏移";
            this.columnHeader11.Width = 103;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "注釋";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 459);
            this.Controls.Add(this.ClearListView);
            this.Controls.Add(this.importXml);
            this.Controls.Add(this.exportXml);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "編輯器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button exportXml;
        private System.Windows.Forms.Button importXml;
        private System.Windows.Forms.Button ClearListView;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
    }
}

