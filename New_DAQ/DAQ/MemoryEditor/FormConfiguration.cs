using Ksat.SharedProjectSettings.Utils;
using MemoryEdito;
using MemoryEditor.Station;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryEditor
{
    public partial class FormConfiguration : Ksat.AppPlugIn.UiCtrl.AbstractMultiChildForm
    {
        private TreeListViewItem mCopyListItem;
        MemoryDesiginProfile profile;
        public FormConfiguration()
        {
            InitializeComponent();
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            mCopyListItem = null;
            //自动加载
            initItems();
        }

        private void initItems()
        {
            MemoryDesiginProfile.InitDataRootPath("E:\\DAQ_DEMO\\DAQ\\MemoryEditor");
            //  string path = "E:\\DAQ_DEMO\\DAQ\\MemoryEditor\\MemoryDesiginProfile.xml";
            //  profile = MemoryDesiginProfile.LoadFrom(path);
            profile = MemoryDesiginProfile.Load();
            this.treeListView1.Items.Clear();
            foreach (EventSection section in profile.EventSections)
            {
                TreeListViewItem item_section = createTreeListViewItemWithChild(section);
                treeListView1.Items.Add(item_section);
            }
        }

        private TreeListViewItem createTreeListViewItemWithChild(EventSection section)
        {
            TreeListViewItem item_section = CreateTreeListViewItem(section, 0);

            foreach (Setting setting in section.Settings)
            {
                createTreeListViewItemWithChild(setting, item_section);
            }

            return item_section;
        }

        private TreeListViewItem createTreeListViewItemWithChild(Setting setting, TreeListViewItem parent_item)
        {
            TreeListViewItem item_setting = CreateTreeListViewItem(setting, 1, parent_item);

            foreach (SubSetting subSeeting in setting.SubSettings)
            {
                 CreateTreeListViewItem(subSeeting, 3, item_setting);
            }
            return item_setting;
        }

        private TreeListViewItem CreateTreeListViewItem(IEventNode info, int imageindex = 0, TreeListViewItem parent_item = null)
        {
            TreeListViewItem node = new TreeListViewItem(info.EventName, imageindex);
            if (info is Setting)
                node = new TreeListViewItem(info.EventName, imageindex);
            //else if(info is SubMenu)
            node.Tag = info;
            node.Items.Sortable = false;
            //node.SubItems[]
            node.SubItems.Add(info.MemoryBegin.ToString());
            node.SubItems.Add(info.MemoryEnd.ToString());
            node.SubItems.Add(info.DataSize.ToString());
            node.SubItems.Add(info.Description);

            if (parent_item != null)
                parent_item.Items.Add(node);

            return node;
        }

        //节点的双击事件
        private void treeListView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (true)
                    return;

                if (treeListView1.FocusedItem == null)
                    return;

                object tag = treeListView1.FocusedItem.Tag;
                if (tag == null)
                    return;

                if (tag is EventSection)
                {
                   onMeniEditorClick(this.menuEditEvent, e);
                }
                else if (tag is Setting)
                {
                   onMeniEditorClick(this.menuSingleEdit, e);
                }
                else if (tag is SubSetting)
                {
                   onMeniEditorClick(this.menuSingleEdit, e);
                }

            }
            catch (Exception ex)
            {
            }
        }

        // 更新节点的值域
        private void UpdateTreeNode(TreeListViewItem item, AbstractEventNodeBase info)
        {
            item.SubItems[0].Text = info.EventName;
            item.SubItems[1].Text = info.MemoryBegin.ToString();
            item.SubItems[2].Text = info.MemoryEnd.ToString();
            item.SubItems[3].Text = info.DataSize.ToString();
            item.SubItems[4].Text = info.Description;
        }

        // 菜单 功能函数
        private void onMeniEditorClick(object sender, EventArgs e)
        {
            object tag = null;
            if (treeListView1.FocusedItem == null)
                tag = new EventSection();
            else
                tag = treeListView1.FocusedItem.Tag;


            if (tag == null)
                return;

            #region modify 编辑菜单
            if (sender.Equals(this.menuSingleEdit)) // 编辑
            {
                if(tag is EventSection)
                {
                    FormEventSection form = new FormEventSection(null, ((EventSection)treeListView1.FocusedItem.Tag));
                    form.ShowDialog();
                    if (form.CurrentNodeInfo != null)
                    {
                        UpdateTreeNode(treeListView1.FocusedItem, form.CurrentNodeInfo);
                    }
                }
                else if(tag is Setting)
                {
                    FormSetting form = new FormSetting(null, ((Setting)treeListView1.FocusedItem.Tag));
                    form.ShowDialog();
                    if (form.CurrentNodeInfo != null)
                    {
                        UpdateTreeNode(treeListView1.FocusedItem, form.CurrentNodeInfo);
                    }
                }
                else if (tag is SubSetting)
                {
                    FormSubSetting form = new FormSubSetting(null,((SubSetting)treeListView1.FocusedItem.Tag));
                    form.ShowDialog();
                    if (form.CurrentNodeInfo != null)
                    {
                        UpdateTreeNode(treeListView1.FocusedItem, form.CurrentNodeInfo);
                    }
                }

            }
            #endregion
            #region delete 删除
            else if (sender.Equals(this.menuDelete))
            {
                DialogResult dr = MessageBox.Show("确定删除吗?", "删除", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    if (tag is Setting)
                    {
                        EventSection parent = (EventSection)treeListView1.FocusedItem.Parent.Tag;
                        parent.Settings.Remove((Setting)tag);
                    }
                    else if (tag is SubSetting)
                    {
                        Setting parent = (Setting)treeListView1.FocusedItem.Parent.Tag;
                        parent.SubSettings.Remove((SubSetting)tag);
                    }
                    else if (tag is EventSection)
                    {
                        EventSection curTag = (EventSection)treeListView1.FocusedItem.Tag;
                        profile.EventSections.Remove((EventSection)tag);
                    }
                    else
                    {
                        return;
                    }

                    treeListView1.FocusedItem.Remove();
                }
            }
            #endregion
            #region copy paste 拷贝 粘贴
            else if (sender.Equals(this.menuCopy))
            {
                this.mCopyListItem = treeListView1.FocusedItem;
            }
            else if (sender.Equals(this.menuPaste))
            {
                if (mCopyListItem == null || mCopyListItem.Tag == null)
                {
                    mCopyListItem = null;
                    return;
                }
                DialogResult dr = MessageBox.Show("确定要复制到该节点吗?", "节点复制", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    startPasteListItem(mCopyListItem, treeListView1.FocusedItem);

                    
                }
            }
            #endregion
            #region Add Setting  添加二级菜单
            else if (sender.Equals(this.menuAddSetting))
            {
                EventSection parent_node = null;
                if (tag is EventSection)
                    parent_node = (EventSection)tag;
                else
                    parent_node = ((EventSection)treeListView1.FocusedItem.Tag);

                FormSetting form = new FormSetting(parent_node);
                form.ShowDialog();

                if (form.CurrentNodeInfo != null)
                {
                    if (tag is EventSection)
                    {
                        CreateTreeListViewItem(form.CurrentNodeInfo, 1, treeListView1.FocusedItem);
                        ((EventSection)tag).Settings.Add(form.CurrentNodeInfo);
                    }
                    else
                    {
                        TreeListViewItem node = CreateTreeListViewItem(form.CurrentNodeInfo, 1, treeListView1.FocusedItem.Parent);
                        ((EventSection)treeListView1.FocusedItem.Parent.Tag).Settings.Add(form.CurrentNodeInfo);
                    }
                }
            }
            #endregion
            #region Add SubSetting  添加三级级菜单
            else if (sender.Equals(this.menuAddSubSetting))
            {
                Setting parent_node = null;
                if (tag is Setting)
                    parent_node = (Setting)tag;
                else
                    parent_node = ((Setting)treeListView1.FocusedItem.Tag);

                FormSubSetting form = new FormSubSetting(null,null);
                form.ShowDialog();

                if (form.CurrentNodeInfo != null)
                {
                    if (tag is Setting)
                    {
                        CreateTreeListViewItem(form.CurrentNodeInfo, 2, treeListView1.FocusedItem);
                        ((Setting)tag).SubSettings.Add(form.CurrentNodeInfo);
                    }
                    else
                    {
                        TreeListViewItem node = CreateTreeListViewItem(form.CurrentNodeInfo, 2, treeListView1.FocusedItem.Parent);
                        ((Setting)treeListView1.FocusedItem.Parent.Tag).SubSettings.Add(form.CurrentNodeInfo);
                    }
                }
            }
            #endregion
            #region Add Event 添加事件Event
            else if (sender.Equals(this.menuAddEvent))
            {
                //EventSection parent_node = null;
                if (tag is EventSection)
                {
                    FormEventSection form = new FormEventSection(null);
                    form.ShowDialog();

                    if (form.CurrentNodeInfo != null)
                    {
                        if (tag is EventSection)
                        {
                           TreeListViewItem node = CreateTreeListViewItem(form.CurrentNodeInfo, 0);
                            profile.EventSections.Add((EventSection)node.Tag);
                            treeListView1.Items.Add(node);
                        }
                    }
                }
                  //  parent_node = (EventSection)tag;
            }
            #endregion
            #region Move Up Down 上移下移
            else if (sender.Equals(this.menuMoveUp))
            {
                TreeListViewItemCollection collection = treeListView1.FocusedItem.Container;
                if (tag is EventSection)
                {
                    int focused_index = collection.GetIndexOf(treeListView1.FocusedItem);
                }
                else if (tag is Setting)
                {

                }
                else if (tag is SubSetting)
                {

                }
            }
            else if (sender.Equals(this.menuMoveDown))
            {
                if (tag is EventSection)
                {

                }
                else if (tag is Setting)
                {

                }
                else if (tag is SubSetting)
                {

                }
            }
            #endregion
        }

        // 粘贴 
        private void startPasteListItem(TreeListViewItem from, TreeListViewItem to)
        {
            object to_tag = to.Tag;
            object from_tag = from.Tag;
            if (to_tag == null)
                return;

            string from_parent_tag = String.Empty;
            if (from.Parent != null && from.Parent.Tag != null)
                from_parent_tag = (from.Parent.Tag as AbstractEventNodeBase).EventName;

            string to_tag_string = (to_tag as AbstractEventNodeBase).EventName;

            if (from_tag is AbstractEventNodeBase)
            {
                AbstractEventNodeBase from_copy = (from_tag as AbstractEventNodeBase).DeepCopy();

                if (from_copy is Setting)
                {
                    try
                    {
                        (to_tag as EventSection).Settings.Add(from_copy as Setting);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("请选择要粘贴的一级目录");
                        return;
                    }

                    if (!String.IsNullOrEmpty(from_parent_tag))
                    {
                        from_copy.EventName = from_copy.EventName.Replace(from_parent_tag, to_tag_string);

                        foreach (SubSetting subsetting in (from_copy as Setting).SubSettings)
                        {
                            subsetting.EventName = subsetting.EventName.Replace(from_parent_tag, to_tag_string);
                        }
                    }
                    createTreeListViewItemWithChild(from_copy as Setting, to);
                    to.ExpandAll();

                }
                else if (from_copy is SubSetting)
                {
                    try
                    {
                        (to_tag as Setting).SubSettings.Add(from_copy as SubSetting);
                    }catch(Exception ex)
                    {
                        MessageBox.Show("请选择要粘贴的二级目录");
                        return;
                    }
                    if (!String.IsNullOrEmpty(from_parent_tag))
                    {
                        from_copy.EventName = from_copy.EventName.Replace(from_parent_tag, to_tag_string);
                    }

                    CreateTreeListViewItem(from_copy as SubSetting, 3,to);
                    to.ExpandAll();
                }
                mCopyListItem = null;
            }
        }

        // 展开
        private void treeListView1_AfterExpand(object sender, TreeListViewEventArgs e)
        {

        }

        // 菜单操作
        private void contextMenuStripOperate_Opening(object sender, CancelEventArgs e)
        {
            if (this.treeListView1.FocusedItem == null)
            {
                return;
            }

            object tag = treeListView1.FocusedItem.Tag;
            if (tag == null)
                return;

            //初始化时候把右键属性都隐藏
            for (int i = 0; i < contextMenuStripOperate.Items.Count; i++)
            {
                contextMenuStripOperate.Items[i].Visible = false;
            }
            this.menuDelete.Visible = true;// 开启删除按钮
            this.menuSingleEdit.Visible = true;// 编辑

            this.menuMoveDown.Visible = false;
            this.menuMoveUp.Visible = false;

            if (tag != null)
            {
                treeListView1.ContextMenuStrip = contextMenuStripOperate;

                if (tag is EventSection)
                {
                    this.menuAddEvent.Visible = true;
                    this.menuAddSetting.Visible = true;
                }
                else if (tag is Setting)
                {
                    this.menuCopy.Visible = true;
                    this.menuAddSubSetting.Visible = true;
                }
                else if (tag is SubSetting)
                {
                    this.menuCopy.Visible = true;
                }

                if (mCopyListItem != null && mCopyListItem.Tag != null)
                {
                    this.menuPaste.Visible = true;
                }
            }
            else
            {
                this.treeListView1.ContextMenuStrip = contextMenuStripOperate;
            }
        }

        // 保存
        private void Apply_Click(object sender, EventArgs e)
        {
          //  MemoryDesiginProfile profile = MemoryDesiginProfile.Load();
            string path = "E:\\DAQ_DEMO\\DAQ\\MemoryEditor\\MemoryDesiginProfile.xml";
            MemoryDesiginProfile profile = MemoryDesiginProfile.Load();
            if (profile.Save())
            {
                MessageBox.Show("保存成功！");
            }
            else
            {
                MessageBox.Show("保存失败！");
            }
        }

        // 取消
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
