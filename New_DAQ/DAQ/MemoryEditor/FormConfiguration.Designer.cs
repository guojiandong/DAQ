namespace MemoryEditor
{
    partial class FormConfiguration
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer2 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguration));
            this.treeListView1 = new System.Windows.Forms.TreeListView();
            this.EventName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MemoryBegin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MemoryEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DataSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripOperate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddSubSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSingleEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.image = new System.Windows.Forms.ImageList(this.components);
            this.Cancel = new System.Windows.Forms.Button();
            this.Apply = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuStripOperate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeListView1
            // 
            this.treeListView1.AllowDrop = true;
            this.treeListView1.AutoArrange = false;
            this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.EventName,
            this.MemoryBegin,
            this.MemoryEnd,
            this.DataSize,
            this.Description});
            treeListViewItemCollectionComparer2.Column = 0;
            treeListViewItemCollectionComparer2.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeListView1.Comparer = treeListViewItemCollectionComparer2;
            this.treeListView1.ContextMenuStrip = this.contextMenuStripOperate;
            this.treeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView1.Location = new System.Drawing.Point(0, 0);
            this.treeListView1.Name = "treeListView1";
            this.treeListView1.Size = new System.Drawing.Size(1150, 505);
            this.treeListView1.SmallImageList = this.image;
            this.treeListView1.Sorting = System.Windows.Forms.SortOrder.None;
            this.treeListView1.TabIndex = 0;
            this.treeListView1.UseCompatibleStateImageBehavior = false;
            this.treeListView1.AfterExpand += new System.Windows.Forms.TreeListViewEventHandler(this.treeListView1_AfterExpand);
            this.treeListView1.SelectedIndexChanged += new System.EventHandler(this.treeListView1_SelectedIndexChanged);
            this.treeListView1.DoubleClick += new System.EventHandler(this.treeListView1_DoubleClick);
            // 
            // EventName
            // 
            this.EventName.Text = "EventName";
            this.EventName.Width = 200;
            // 
            // MemoryBegin
            // 
            this.MemoryBegin.Text = "MemoryBegin";
            this.MemoryBegin.Width = 150;
            // 
            // MemoryEnd
            // 
            this.MemoryEnd.Text = "MemoryEnd";
            this.MemoryEnd.Width = 150;
            // 
            // DataSize
            // 
            this.DataSize.Text = "DataSize";
            this.DataSize.Width = 150;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 300;
            // 
            // contextMenuStripOperate
            // 
            this.contextMenuStripOperate.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripOperate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopy,
            this.menuPaste,
            this.menuMoveUp,
            this.menuMoveDown,
            this.menuAddEvent,
            this.menuAddSetting,
            this.menuAddSubSetting,
            this.menuSingleEdit,
            this.menuEditEvent,
            this.menuDelete});
            this.contextMenuStripOperate.Name = "contextMenuStripOperate";
            this.contextMenuStripOperate.Size = new System.Drawing.Size(169, 244);
            this.contextMenuStripOperate.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripOperate_Opening);
            // 
            // menuCopy
            // 
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(168, 24);
            this.menuCopy.Text = "复制";
            this.menuCopy.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuPaste
            // 
            this.menuPaste.Name = "menuPaste";
            this.menuPaste.Size = new System.Drawing.Size(168, 24);
            this.menuPaste.Text = "粘贴";
            this.menuPaste.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuMoveUp
            // 
            this.menuMoveUp.Name = "menuMoveUp";
            this.menuMoveUp.Size = new System.Drawing.Size(168, 24);
            this.menuMoveUp.Text = "上移";
            this.menuMoveUp.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuMoveDown
            // 
            this.menuMoveDown.Name = "menuMoveDown";
            this.menuMoveDown.Size = new System.Drawing.Size(168, 24);
            this.menuMoveDown.Text = "下移";
            this.menuMoveDown.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuAddEvent
            // 
            this.menuAddEvent.Name = "menuAddEvent";
            this.menuAddEvent.Size = new System.Drawing.Size(168, 24);
            this.menuAddEvent.Text = "添加事件";
            this.menuAddEvent.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuAddSetting
            // 
            this.menuAddSetting.Name = "menuAddSetting";
            this.menuAddSetting.Size = new System.Drawing.Size(168, 24);
            this.menuAddSetting.Tag = "Motor";
            this.menuAddSetting.Text = "添加二级菜单";
            this.menuAddSetting.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuAddSubSetting
            // 
            this.menuAddSubSetting.Name = "menuAddSubSetting";
            this.menuAddSubSetting.Size = new System.Drawing.Size(168, 24);
            this.menuAddSubSetting.Text = "添加三级菜单";
            this.menuAddSubSetting.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuSingleEdit
            // 
            this.menuSingleEdit.Name = "menuSingleEdit";
            this.menuSingleEdit.Size = new System.Drawing.Size(168, 24);
            this.menuSingleEdit.Text = "编辑";
            this.menuSingleEdit.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuEditEvent
            // 
            this.menuEditEvent.Name = "menuEditEvent";
            this.menuEditEvent.Size = new System.Drawing.Size(168, 24);
            this.menuEditEvent.Text = "编辑事件";
            this.menuEditEvent.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(168, 24);
            this.menuDelete.Text = "删除";
            this.menuDelete.Click += new System.EventHandler(this.onMeniEditorClick);
            // 
            // image
            // 
            this.image.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("image.ImageStream")));
            this.image.TransparentColor = System.Drawing.Color.Transparent;
            this.image.Images.SetKeyName(0, "t1.PNG");
            this.image.Images.SetKeyName(1, "t2.PNG");
            this.image.Images.SetKeyName(2, "t3.PNG");
            this.image.Images.SetKeyName(3, "t4.PNG");
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(227, 15);
            this.Cancel.Name = "Cancel";
            this.Cancel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Cancel.Size = new System.Drawing.Size(90, 30);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Apply
            // 
            this.Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Apply.Location = new System.Drawing.Point(861, 17);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(90, 30);
            this.Apply.TabIndex = 2;
            this.Apply.Text = "保存";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeListView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Apply);
            this.splitContainer1.Panel2.Controls.Add(this.Cancel);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1150, 565);
            this.splitContainer1.SplitterDistance = 505;
            this.splitContainer1.TabIndex = 3;
            // 
            // FormConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 565);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FormConfiguration";
            this.Text = "Memory Design Tool";
            this.Load += new System.EventHandler(this.FormConfiguration_Load);
            this.contextMenuStripOperate.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeListView treeListView1;
        private System.Windows.Forms.ColumnHeader EventName;
        private System.Windows.Forms.ColumnHeader MemoryBegin;
        private System.Windows.Forms.ColumnHeader MemoryEnd;
        private System.Windows.Forms.ColumnHeader DataSize;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ImageList image;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOperate;
        private System.Windows.Forms.ToolStripMenuItem menuAddSetting;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripMenuItem menuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem menuMoveDown;
        private System.Windows.Forms.ToolStripMenuItem menuAddEvent;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.ToolStripMenuItem menuPaste;
        private System.Windows.Forms.ToolStripMenuItem menuSingleEdit;
        private System.Windows.Forms.ToolStripMenuItem menuEditEvent;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.ToolStripMenuItem menuAddSubSetting;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

