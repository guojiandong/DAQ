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
            this.type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.enable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dataType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.operatorType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.offset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inWordOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inBitOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.outWordOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.outBitOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.note = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Add = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.exportXml = new System.Windows.Forms.Button();
            this.importXml = new System.Windows.Forms.Button();
            this.ClearListView = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.type,
            this.enable,
            this.dataType,
            this.operatorType,
            this.offset,
            this.inWordOffset,
            this.inBitOffset,
            this.outWordOffset,
            this.outBitOffset,
            this.note});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(1, 1);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(999, 351);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.onDouleClickItem);
            // 
            // type
            // 
            this.type.Text = "ID";
            this.type.Width = 40;
            // 
            // enable
            // 
            this.enable.Text = "允許輸入|寫入讀取地址不同";
            this.enable.Width = 218;
            // 
            // dataType
            // 
            this.dataType.Text = "数据类型";
            this.dataType.Width = 83;
            // 
            // operatorType
            // 
            this.operatorType.Text = "操作类型";
            this.operatorType.Width = 80;
            // 
            // offset
            // 
            this.offset.Text = "偏移量";
            this.offset.Width = 69;
            // 
            // inWordOffset
            // 
            this.inWordOffset.Text = "寫入字偏移";
            this.inWordOffset.Width = 96;
            // 
            // inBitOffset
            // 
            this.inBitOffset.Text = "寫入位偏移";
            this.inBitOffset.Width = 92;
            // 
            // outWordOffset
            // 
            this.outWordOffset.Text = "讀出字偏移";
            this.outWordOffset.Width = 90;
            // 
            // outBitOffset
            // 
            this.outBitOffset.Text = "讀出位偏移";
            this.outBitOffset.Width = 103;
            // 
            // note
            // 
            this.note.Text = "注釋";
            this.note.Width = 114;
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(1052, 21);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(104, 23);
            this.Add.TabIndex = 1;
            this.Add.Text = "新增文本";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.onAddTextComponent);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1052, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "新增按鈕";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.onAddBtnComponent);
            // 
            // exportXml
            // 
            this.exportXml.Location = new System.Drawing.Point(1065, 232);
            this.exportXml.Name = "exportXml";
            this.exportXml.Size = new System.Drawing.Size(75, 23);
            this.exportXml.TabIndex = 4;
            this.exportXml.Text = "導出xml";
            this.exportXml.UseVisualStyleBackColor = true;
            this.exportXml.Click += new System.EventHandler(this.exportXml_Click);
            // 
            // importXml
            // 
            this.importXml.Location = new System.Drawing.Point(1065, 280);
            this.importXml.Name = "importXml";
            this.importXml.Size = new System.Drawing.Size(75, 23);
            this.importXml.TabIndex = 6;
            this.importXml.Text = "導入xml";
            this.importXml.UseVisualStyleBackColor = true;
            this.importXml.Click += new System.EventHandler(this.importXml_Click);
            // 
            // ClearListView
            // 
            this.ClearListView.Location = new System.Drawing.Point(1065, 329);
            this.ClearListView.Name = "ClearListView";
            this.ClearListView.Size = new System.Drawing.Size(75, 23);
            this.ClearListView.TabIndex = 7;
            this.ClearListView.Text = "清空列表";
            this.ClearListView.UseVisualStyleBackColor = true;
            this.ClearListView.Click += new System.EventHandler(this.ClearListView_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 359);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(999, 150);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 523);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ClearListView);
            this.Controls.Add(this.importXml);
            this.Controls.Add(this.exportXml);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "编辑器";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader type;
        private System.Windows.Forms.ColumnHeader enable;
        private System.Windows.Forms.ColumnHeader dataType;
        private System.Windows.Forms.ColumnHeader operatorType;
        private System.Windows.Forms.ColumnHeader offset;
        private System.Windows.Forms.Button exportXml;
        private System.Windows.Forms.Button importXml;
        private System.Windows.Forms.Button ClearListView;
        private System.Windows.Forms.ColumnHeader inWordOffset;
        private System.Windows.Forms.ColumnHeader inBitOffset;
        private System.Windows.Forms.ColumnHeader outWordOffset;
        private System.Windows.Forms.ColumnHeader outBitOffset;
        private System.Windows.Forms.ColumnHeader note;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

