using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAQ
{
    public delegate void SetValueHandler(Component com);
    public delegate void RemoveHandler();
    public delegate void SaveHandler(Component com);

    public partial class Form1 : Form
    {
        public List<MemoryState> list_MemoryState;
        BtnComponent form_Btn;
        TextComponent form_Text;
        public UInt16[] memory_block = new UInt16[100];  // 開闢100個int16位數組

        public Form1()
        {
            list_MemoryState = new List<MemoryState>();
            InitializeComponent();
            InitMemoryState();
        }

        //  点击listview 中的一行， 调到与之向对应的datagridview 某一行
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Console.WriteLine("listView1_SelectedIndexChanged");
            List<string> comValue = new List<string>();
            ListViewItem item = listView1.FocusedItem;
            if (item == null)
            {
                return;
            }
            for (int i = 0; i < item.SubItems.Count; i++)
            {
                comValue.Add(item.SubItems[i].Text);
            }

            int indexOfDataGridView = -1;
            int componentType = int.Parse(comValue[0]);
            if (componentType == (int)ComponentType.BtnComponent)
                indexOfDataGridView = int.Parse(comValue[5]);
            else if (componentType == (int)ComponentType.TextComponent)
                indexOfDataGridView = int.Parse(comValue[4]);

            if (indexOfDataGridView != -1)
            {
                if (dataGridView1.Rows.Count > (indexOfDataGridView))
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.FirstDisplayedScrollingRowIndex = indexOfDataGridView;
                    dataGridView1.Rows[indexOfDataGridView].Selected = true;
                }
            }
        }

        private void onAddTextComponent(object sender, EventArgs e)
        {
            form_Text = new TextComponent();
            form_Text.Name = "文本类型";
            form_Text.setTextValue += new SetValueHandler(_setTextValue);
            form_Text.ChangeBtnState(form_Text.isCreateMode);
            form_Text.ShowDialog();
        }

        private void onAddBtnComponent(object sender, EventArgs e)
        {
            form_Btn = new BtnComponent();
            form_Btn.Name = "按钮类型";
            form_Btn.setBtnValue += new SetValueHandler(_setBtnValue);
            form_Btn.ChangeBtnState(form_Btn.isCreateMode);
            form_Btn.ShowDialog();
        }

        public void Form_BtnComponent_Closed()
        {

        }
        public void _setTextValue(Component com)
        {
            System.Console.WriteLine("_setTextValue text Type isEnable_Input:{0}, data_Type: {1}", com.isEnable_Input, com.data_Type);
            AddComponent(com);
        }

        public void _setBtnValue(Component com)
        {
            System.Console.WriteLine("_setBtnValue text Type isEnable_Input:{0}, data_Type: {1}", com.isEnable_Input, com.data_Type);
            AddComponent(com);
        }

        // 数据删除的回调
        public void _removeHandler()
        {
            //从内存映射表中删除
            List<string> comValue = new List<string>();
            ListViewItem item = listView1.FocusedItem;
            for (int i = 0; i < item.SubItems.Count; i++)
            {
                comValue.Add(item.SubItems[i].Text);
            }
            Component com = new Component();
            int componentType = int.Parse(comValue[0]);
            com.data_Type = (DataType)Enum.Parse(typeof(DataType), comValue[2]);
            com.componentType = componentType;
            if (componentType == (int)ComponentType.BtnComponent)
            {
                com.isEnable_Input = comValue[1];
                com.in_word_offset = comValue[5];
                com.in_bit_offset = comValue[6];
                com.out_word_offset = comValue[7];
                com.out_bit_offset = comValue[8];

                set_bit_value(ref memory_block, int.Parse(com.in_word_offset), int.Parse(com.in_bit_offset), false);
                if (!Convert.ToBoolean(com.isEnable_Input))
                {
                    set_bit_value(ref memory_block, int.Parse(com.out_word_offset), int.Parse(com.out_bit_offset), false);
                }
            }
            else if (componentType == (int)ComponentType.TextComponent)
            {
                com.isEnable_Input = comValue[1];
                com.offset = comValue[4];
                set_word_value(ref memory_block, int.Parse(comValue[4]), false);
                int cur_data_type = CheckDataType(com.data_Type);
                if (cur_data_type == 1 || cur_data_type == 2)
                {
                    set_word_value(ref memory_block, int.Parse(comValue[4]) + 1, false);
                }
                else if (cur_data_type == 3)
                {
                    set_word_value(ref memory_block, int.Parse(comValue[4]) + 1, false);
                    set_word_value(ref memory_block, int.Parse(comValue[4]) + 2, false);
                    set_word_value(ref memory_block, int.Parse(comValue[4]) + 3, false);
                }
            }

            int indexDel = listView1.Items.IndexOf(listView1.FocusedItem);
            listView1.Items.RemoveAt(indexDel);//删除
            UpdateMemoryState(com);
        }


        // 数据更改的回调
        public void _saveHandler(Component com)
        {
            //擦除内存上的旧值
            List<string> comValue = new List<string>();
            ListViewItem item = listView1.FocusedItem;
            for (int i = 0; i < item.SubItems.Count; i++)
            {
                comValue.Add(item.SubItems[i].Text);
            }
            int componentType = int.Parse(comValue[0]);
            if (componentType == (int)ComponentType.BtnComponent)
            {
                bool sameAddress = Convert.ToBoolean(comValue[1]);
                int in_word_offset = int.Parse(comValue[5]);
                int in_bit_offset = int.Parse(comValue[6]);
                int out_word_offset = int.Parse(comValue[7]);
                int out_bit_offset = int.Parse(comValue[8]);

                set_bit_value(ref memory_block, in_word_offset, in_bit_offset, false);
                if (!sameAddress)
                    set_bit_value(ref memory_block, out_word_offset, out_bit_offset, false);
            }
            else if (componentType == (int)ComponentType.TextComponent)
            {
                int offset = int.Parse(comValue[4]);
                set_word_value(ref memory_block, offset, false);
                int cur_data_type = CheckDataType(com.data_Type);
                if (cur_data_type == 1 || cur_data_type == 2)
                {
                    set_word_value(ref memory_block, offset + 1, false);
                }
                else if (cur_data_type == 3)
                {
                    set_word_value(ref memory_block, offset + 1, false);
                    set_word_value(ref memory_block, offset + 2, false);
                    set_word_value(ref memory_block, offset + 3, false);
                }
            }

            // 保存新的值到对应内存上
            bool canInsert = canInsert2ListView(com, true);
            if (!canInsert)
            {
                return;
            }

            // 刷新 值到listview中
            item.SubItems[0].Text = CheckEmpty(com.componentType.ToString());
            item.SubItems[1].Text = CheckEmpty(com.isEnable_Input);
            item.SubItems[2].Text = CheckEmpty(com.data_Type.ToString());
            item.SubItems[3].Text = CheckEmpty(com.operatorType.ToString());

            item.SubItems[4].Text = CheckEmpty(com.offset);
            item.SubItems[5].Text = CheckEmpty(com.in_word_offset);
            item.SubItems[6].Text = CheckEmpty(com.in_bit_offset);
            item.SubItems[7].Text = CheckEmpty(com.out_word_offset);
            item.SubItems[8].Text = CheckEmpty(com.out_bit_offset);
            item.SubItems[9].Text = CheckEmpty(com.note);
            item.SubItems[10].Text = CheckEmpty(com.pressType.ToString());
            item.SubItems[11].Text = CheckEmpty(com.point.ToString());


            //移除旧的内存关系
            Component com_del = new Component();
            com_del.componentType   = int.Parse(comValue[0]);
            com_del.isEnable_Input  = comValue[1];
            com_del.data_Type       = (DataType)Enum.Parse(typeof(DataType), comValue[2]);
            com_del.offset          = comValue[4];
            com_del.in_word_offset  = comValue[5];
            com_del.in_bit_offset   = comValue[6];
            com_del.out_word_offset = comValue[7];
            com_del.out_bit_offset  = comValue[8];

            //移除旧的内存映射
            UpdateMemoryState(com_del);

            //添加新的内存映射
            UpdateMemoryState(com);
        }

        public void AddComponent(Component com)
        {
            System.Console.WriteLine("AddComponent + componentType: {0}  ", com.componentType.ToString());
            bool canInsert = canInsert2ListView(com, false);
            if (!canInsert)
            {
                return;
            }

            ListViewItem lt = new ListViewItem();
            lt.Text = com.componentType.ToString();
            lt.SubItems.Add(com.isEnable_Input.ToString());
            lt.SubItems.Add(com.data_Type.ToString());
            lt.SubItems.Add(com.operatorType.ToString());
            lt.SubItems.Add(CheckEmpty(com.offset));
            lt.SubItems.Add(CheckEmpty(com.in_word_offset));
            lt.SubItems.Add(CheckEmpty(com.in_bit_offset));
            lt.SubItems.Add(CheckEmpty(com.out_word_offset));
            lt.SubItems.Add(CheckEmpty(com.out_bit_offset));
            lt.SubItems.Add(CheckEmpty(com.note));
            lt.SubItems.Add(CheckEmpty(com.pressType.ToString()));
            lt.SubItems.Add(CheckEmpty(com.point.ToString()));

            this.listView1.Items.Add(lt);
            UpdateMemoryState(com);
        }


        // 检测该控件是否可以插入对应地址
        public bool canInsert2ListView(Component com, bool isOverrid)
        {
            bool canInsert = checkMemory(com, isOverrid);
            return canInsert;
        }

        //将控件填写的地址映射到memory_block中
        public bool memoryMapped(bool isWholeWord, int word_offset, int bit_offset = 0)
        {
            bool isSucc = true;
            if (isWholeWord)  //Text类型 映射到整个word
            {
                isSucc = set_word_value(ref memory_block, word_offset, true);
            }
            else // btn类型 映射到word 的bit位
            {
                isSucc = set_bit_value(ref memory_block, word_offset, bit_offset, true);
            }
            return true;
        }

        //检测内存是否被占用，没有占用的话 就将该内存置位1， 否则，提示该内存已被占用。
        public bool checkMemory(Component com, bool isOverrid)
        {
            bool isSucc = false;
            if (com.componentType == (int)ComponentType.TextComponent)
            {
                int offset = int.Parse(com.offset);
                System.Console.WriteLine("Text offset: {0}", offset);

                bool memoryUsed_right = check_word_used(ref memory_block, offset);
                bool memoryUsed_left = check_word_used(ref memory_block, offset + 1);

                int cur_data_type = CheckDataType(com.data_Type);
                if (cur_data_type == 0)
                {
                    if (!memoryUsed_right || isOverrid)
                    {
                        isSucc = memoryMapped(true, offset);
                    }
                    else
                    {
                        MessageBox.Show("该内存已被占用，请选择其他内存");
                        return false;
                    }
                }
                else if (cur_data_type == 1 || cur_data_type == 2)
                {
                    bool succ1 = false;
                    bool succ2 = false;
                    if ( ( !memoryUsed_right && !memoryUsed_left ) || isOverrid)
                    {
                        succ1 = memoryMapped(true, offset);
                        succ2 = memoryMapped(true, offset + 1);
                        isSucc = succ1 && succ2;
                    }
                    else
                    {
                        MessageBox.Show("该内存已被占用，请选择其他内存");
                        return false;
                    }
                }
                else if (cur_data_type == 3)
                {
                    bool memoryUsed_1 = check_word_used(ref memory_block, offset + 2);
                    bool memoryUsed_2 = check_word_used(ref memory_block, offset + 3);
                    if ((!memoryUsed_right && !memoryUsed_left && !memoryUsed_1 && !memoryUsed_2) || isOverrid)
                    {
                        bool succ1 = memoryMapped(true, offset);
                        bool succ2 = memoryMapped(true, offset + 1);
                        bool succ3 = memoryMapped(true, offset + 2);
                        bool succ4 = memoryMapped(true, offset + 3);
                        isSucc = succ1 && succ2 && succ3 && succ3;
                    }
                    else
                    {
                        MessageBox.Show("该内存已被占用，请选择其他内存");
                        return false;
                    }
                }
            }
            else if (com.componentType == (int)ComponentType.BtnComponent)
            {
                bool isSameAddress = Convert.ToBoolean(com.isEnable_Input);
                UInt16 in_word_offset = UInt16.Parse(com.in_word_offset);
                UInt16 in_bit_offset = UInt16.Parse(com.in_bit_offset);
                UInt16 out_word_offset = UInt16.Parse(com.out_word_offset);
                UInt16 out_bit_offset = UInt16.Parse(com.out_bit_offset);
                System.Console.WriteLine("enabled  = {0}", isSameAddress);
                if (isSameAddress)
                {
                    bool memoryUsed = check_bit_used(memory_block[in_word_offset], in_bit_offset);
                    if (!memoryUsed || isOverrid)
                        isSucc = memoryMapped(false, in_word_offset, in_bit_offset);
                    else
                    {
                        MessageBox.Show("该内存已被占用，请选择其他内存");
                        return false;
                    }
                }
                else
                {
                    bool memoryUsed1 = check_bit_used(memory_block[in_word_offset], in_bit_offset);
                    bool memoryUsed2 = check_bit_used(memory_block[out_word_offset], out_bit_offset);

                    if (memoryUsed1 & !isOverrid)
                    {
                        MessageBox.Show("写入内存已被占用，请选择其他内存");
                        return false;
                    }
                    if (memoryUsed2 & !isOverrid)
                    {
                        MessageBox.Show("读取内存已被占用，请选择其他内存");
                        return false;
                    }

                    bool succ1 = memoryMapped(false, in_word_offset, in_bit_offset);
                    bool succ2 = memoryMapped(false, out_word_offset, out_bit_offset);
                    isSucc = succ1 & succ2;
                }
            }
            return isSucc;
        }

        private void onDouleClickItem(object sender, EventArgs e)
        {
            //  System.Console.WriteLine("onDouleClickItem");
            List<string> comValue = new List<string>();
            ListViewItem item = listView1.FocusedItem;
            for (int i = 0; i < item.SubItems.Count; i++)
            {
                comValue.Add(item.SubItems[i].Text);
            }
            if (listView1.SelectedItems.Count != 0)
            {
                int componentType = int.Parse(comValue[0]);
                if (componentType == (int)ComponentType.BtnComponent)
                {
                    BtnComponent form2 = new BtnComponent();
                    form2.Name = "按钮类型";
                    form2.setBtnValue += new SetValueHandler(_setBtnValue);
                    form2.removeHandler += new RemoveHandler(_removeHandler);
                    form2.saveHandler += new SaveHandler(_saveHandler);
                    form2.ChangeBtnState(ChangeMode.Change);

                    Component com = new Component();
                    com.componentType = int.Parse(comValue[0]);
                    com.isEnable_Input = comValue[1];
                    com.data_Type = (DataType)Enum.Parse(typeof(DataType), comValue[2]);
                    com.operatorType = OperatorType._On;
                    com.offset = comValue[4];
                    com.in_word_offset = comValue[5];
                    com.in_bit_offset = comValue[6];
                    com.out_word_offset = comValue[7];
                    com.out_bit_offset = comValue[8];
                    if (comValue.Count >= 9 && !string.IsNullOrEmpty(comValue[9]))
                        com.note = comValue[9];
                    com.pressType = (PressType)Enum.Parse(typeof(PressType), comValue[10]);

                    form2.InitUI(com);
                    form2.ShowDialog();

                }
                else if (componentType == (int)ComponentType.TextComponent)
                {
                    TextComponent form2 = new TextComponent();
                    form2.Name = "文本类型";
                    form2.setTextValue += new SetValueHandler(_setTextValue);
                    form2.removeHandler += new RemoveHandler(_removeHandler);
                    form2.saveHandler += new SaveHandler(_saveHandler);
                    form2.ChangeBtnState(ChangeMode.Change);

                    Component com = new Component();
                    com.isEnable_Input = comValue[1];
                    com.data_Type = (DataType)Enum.Parse(typeof(DataType), comValue[2]);
                    com.offset = comValue[4];
                    com.point = int.Parse(comValue[11]);

                    form2.InitUI(com);
                    form2.ShowDialog();
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exportXml_Click(object sender, EventArgs e)
        {
            // 生成List对象用于测试
            List<Component> list1 = new List<Component>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                ListViewItem item = listView1.Items[i];
                Component com = new Component();

                com.componentType = int.Parse ( item.SubItems[0].Text );
                com.isEnable_Input = item.SubItems[1].Text;
                com.data_Type = (DataType)Enum.Parse(typeof(DataType), item.SubItems[2].Text);
                com.operatorType = (OperatorType)Enum.Parse(typeof(OperatorType), item.SubItems[3].Text);
                com.offset = item.SubItems[4].Text;
                com.in_word_offset = (item.SubItems[5].Text);
                com.in_bit_offset =  (item.SubItems[6].Text);
                com.out_word_offset =  (item.SubItems[7].Text);
                com.out_bit_offset =  (item.SubItems[8].Text);
                com.note =  (item.SubItems[9].Text);
                com.pressType =  (PressType)Enum.Parse(typeof(PressType), item.SubItems[10].Text);
                com.point = int.Parse(item.SubItems[11].Text);
                list1.Add(com);
            }

            // 序列化
            string xml = XmlUtil.Serializer(typeof(List<Component>), list1);
            //Console.Write(xml);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"C: \Users\luxshare - ict\Desktop"; 
            sfd.Title = "请选择要保存的文件路径";
            sfd.Filter = "文本文件|*.xml|所有文件|*.*";
            sfd.ShowDialog();

            //获得用户要保存的文件的路径
            string path = sfd.FileName;
            if (path == "")
            {
                return;
            }
            using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] buffer = Encoding.Default.GetBytes(xml);
                fsWrite.Write(buffer, 0, buffer.Length);
            }
            MessageBox.Show("保存成功");
        }

        private void importXml_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty; //文件名
            //打开文件
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "xml";
            dlg.Filter = "xml Files|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
                fileName = dlg.FileName;
            if (fileName == null)
                return;
            //读取文件内容
            StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default);
            String xml = sr.ReadToEnd().TrimStart();
            sr.Close();

            List<Component> list2 = XmlUtil.Deserialize(typeof(List<Component>), xml) as List<Component>;
            if (list2 == null)
            {
                MessageBox.Show("反序列化错误！");
                return;
            }
            foreach (Component com in list2)
            {

                // 保存新的值到对应内存上
                //bool canInsert = canInsert2ListView(com, true);
                //if (!canInsert)
                //{
                //    return;
                //}
                AddComponent(com);
                UpdateMemoryState(com);
            }
        }

        //清空listview
        private void ClearListView_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            Array.Clear(memory_block, 0,memory_block.Length);
            InitMemoryState();
        }

        // 检查是否为空
        public static string CheckEmpty(string value)
        {
            string str = "0";
            if (string.IsNullOrEmpty(value))
                return str;
            return value;

        }

        // 位操作 系列  memory_block[100]  Uint16   Text類型占一個Uint16   btn類型：不定
        /*
         * |---------------------Uint16--------------------|.................................................. 
         * ------------------------|-----------------------|---------------------------
         * |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
         * |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  .....................
         * |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
         * |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
         * ------------------------|-----------------------|---------------------------
         * {·······················}|     {·}
         *                         |                               |
         * |-----------------------0(字偏移）--------------|--（位偏移）--------1(字偏移)------- ......................
         *                           
         * 
         * */

        /// <summary>
        /// 檢查UInt16 類型的數據的某一位是1還是0.
        /// </summary>
        /// <param name="value">被檢查的值</param>
        /// <param name="index">第幾位</param>
        /// <returns>檢測某一位是否已使用 true表示该位为1，false表示该位为0</returns>
        public bool check_bit_used(UInt16 value, UInt16 index)
        {
            return (value >> (index - 1) & 0x01) == 1;
        }

        /// <summary>
        /// 设定UInt16数据中某一位的值
        /// </summary>设定前的值</param>
        /// <param name="index">16
        /// <param name="value">位位数据的从右向左的偏移位索引(1~16)</param>
        /// <param name="bitValue">true设该位为1,false设为0</param>
        /// /// <returns>返回位设定后的值</returns>
        public static bool set_bit_value(ref UInt16[] block, int indexOfWord, int indexOfBit, bool bitValue)         //btn
        {
            if (indexOfBit > 16)  //索引出错
            {
                MessageBox.Show("位索引不能大于16！");
                return false;
            }

            if (indexOfWord > 99)  //索引出错
            {
                MessageBox.Show("字索引不能大于99！");
                return false;
            }

            UInt16 value = block[indexOfWord];
            int v = indexOfBit < 2 ? indexOfBit : (2 << (indexOfBit - 2));
            UInt16 result = bitValue ? (UInt16)(value | v) : (UInt16)(value & ~v);
            block[indexOfWord] = result;
            return true;
        }

        public static UInt16 MAX_UInt16 = 65535;
        /// <summary>
        /// 設置整個字的值 適用于text類型
        /// </summary>
        /// <param name="block"></param>
        /// <param name="index"></param>
        /// /// <param name="index">used：false 置为0， true 置为1</param>
        public static bool set_word_value(ref UInt16[] block, int index, bool used)        //text
        {
            if (index > 99)//索引出错
            {
                MessageBox.Show("字索引不能大于99！");
                return false;
            }
            if (used)
                block[index] = MAX_UInt16;
            else
                block[index] = 0;
            return true;
        }

        /// <summary>
        /// 檢查字是否被使用
        /// </summary>
        /// <param name="block"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool check_word_used(ref UInt16[] block, int index)
        {
            if (index > 99)//索引出错
            {
                MessageBox.Show("字索引不能大于99！");
                return false;
            }
            if (block[index] > 0)
                return true;
            return false;
        }

        // 检查 数据类型 0 ： 16 bit ， 1 ： 32bit 整数， 2：32bit Float ， 3 ： 64bit  
        public static int CheckDataType(DataType data_type)
        {
            if (data_type == DataType._16_BCD || data_type == DataType._16_Binary || data_type == DataType._16_Hex ||
                data_type == DataType._16_Signed || data_type == DataType._16_Unsigned)
            {
                return 0;
            }
            else if (data_type == DataType._32_BCD || data_type == DataType._32_Binary || data_type == DataType._32_Signed ||
                data_type == DataType._32_Hex || data_type == DataType._32_Unsigned)
            {
                return 1;
            }
            else if (data_type == DataType._32_Float)
            {
                return 2;
            }
            else if (false) // 64-bit TODO
            {
                return 3;
            }
            return -1;
        }


        //------------------------------------------------------------- 占用内存查看 DataGridView --------------------------------------//
        /*****
         * Color : UInt16
         *       white：白色标识该内存字没有被使用。      == 0
         *       green：绿色标识该内存少量被使用。        <= 3
         *       yellow：黄色标识该内存中量以上被使用。   >= 10
         *       red：   红色标识该内存即将使用完。       > 14  
         * 
         * 
         * */
        public void InitMemoryState()
        {
            list_MemoryState.Clear();
            for (int i = 0; i < 100; i++)
            {
                list_MemoryState.Add(new MemoryState(i, -1, false, 0,""));
            }
            //  初始化控件的数据，
            this.dataGridView1.DataSource = null;
            //将对象list_MemoryState中的数据与dataGridView1中的数据绑定
            this.dataGridView1.DataSource = this.list_MemoryState;
            //this.list_MemoryState[0].Bit_used_str = "1-2-3-4-5";
        }

        public void UpdateMemoryState(Component com)
        {
            // 根据类型取出 字偏移 计算其位使用情况
            if ( com.componentType == (int)ComponentType.TextComponent )
            {
                int offset = int.Parse(com.offset); // text 类型的offset 对应的是字
                int curWordValue = memory_block[offset];
                string binaryStr = GetStringBinary(curWordValue);
                int numberOf1 = NumberOf1(curWordValue);
                this.list_MemoryState[offset].Bit_used_str = binaryStr;
                this.list_MemoryState[offset].Com_type = numberOf1 == 0 ? -1:1;

                this.list_MemoryState[offset].State = numberOf1 > 0;
                this.list_MemoryState[offset].Used_count = numberOf1;
                int cur_data_type = CheckDataType(com.data_Type);
                if (cur_data_type == 1 || cur_data_type == 2)
                {
                    int curWordValue_1 = memory_block[offset];
                    int curOffset = offset + 1;
                    string binaryStr_1 = GetStringBinary(curWordValue_1);
                    int numberOf1_1 = NumberOf1(curWordValue_1);
                    this.list_MemoryState[curOffset].Bit_used_str = binaryStr_1;
                    this.list_MemoryState[curOffset].Com_type = numberOf1_1 == 0 ? -1 : 1;
                    this.list_MemoryState[curOffset].State = numberOf1_1 > 0;
                    this.list_MemoryState[curOffset].Used_count = numberOf1_1;
                }
                else if (cur_data_type == 3)
                {
                    int curOffset_1 = offset + 1;
                    int curWordValue_1 = memory_block[curOffset_1];
                    string binaryStr_1 = GetStringBinary(curWordValue_1);
                    int numberOf1_1 = NumberOf1(curWordValue_1);
                    this.list_MemoryState[curOffset_1].Bit_used_str = binaryStr_1;
                    this.list_MemoryState[curOffset_1].Com_type = numberOf1_1 == 0 ? -1 : 1;
                    this.list_MemoryState[curOffset_1].State = numberOf1_1 > 0;
                    this.list_MemoryState[curOffset_1].Used_count = numberOf1_1;

                    int curOffset_2 = offset + 2;
                    int curWordValue_2 = memory_block[curOffset_2];
                    string binaryStr_2 = GetStringBinary(curWordValue_2);
                    int numberOf1_2 = NumberOf1(curWordValue_2);
                    this.list_MemoryState[curOffset_2].Bit_used_str = binaryStr_2;
                    this.list_MemoryState[curOffset_2].Com_type = numberOf1_2 == 0 ? -1 : 1;
                    this.list_MemoryState[curOffset_2].State = numberOf1_2 > 0;
                    this.list_MemoryState[curOffset_2].Used_count = numberOf1_2;

                    int curOffset_3 = offset + 3;
                    int curWordValue_3 = memory_block[curOffset_3];
                    string binaryStr_3 = GetStringBinary(curWordValue_3);
                    int numberOf1_3 = NumberOf1(curWordValue_3);
                    this.list_MemoryState[curOffset_3].Bit_used_str = binaryStr_3;
                    this.list_MemoryState[curOffset_3].Com_type = numberOf1_3 == 0 ? -1 : 1;
                    this.list_MemoryState[curOffset_3].State = numberOf1_3 > 0;
                    this.list_MemoryState[curOffset_3].Used_count = numberOf1_3;
                }

            }
            else if ( com.componentType == (int)ComponentType.BtnComponent)
            {
                int in_word_offset = int.Parse(com.in_word_offset);
                int out_word_offset = int.Parse(com.out_word_offset);
                bool isSameAddress = Convert.ToBoolean(com.isEnable_Input);

                int curWordValue_in = memory_block[in_word_offset];
                string binaryStr_in = GetStringBinary(curWordValue_in);
                int numberOf1_in = NumberOf1(curWordValue_in);
                this.list_MemoryState[in_word_offset].Bit_used_str = binaryStr_in;
                this.list_MemoryState[in_word_offset].Com_type = numberOf1_in == 0 ? -1: 2;
                this.list_MemoryState[in_word_offset].State = numberOf1_in > 0;
                this.list_MemoryState[in_word_offset].Used_count = numberOf1_in;

                if (!isSameAddress)
                {
                    int curWordValue_out = memory_block[out_word_offset];
                    string binaryStr_out = GetStringBinary(curWordValue_out);
                    int numberOf1_out = NumberOf1(curWordValue_out);
                    this.list_MemoryState[out_word_offset].Bit_used_str = binaryStr_out;
                    this.list_MemoryState[out_word_offset].Com_type = numberOf1_out == 0? -1:2;
                    this.list_MemoryState[out_word_offset].State = numberOf1_out > 0;
                    this.list_MemoryState[out_word_offset].Used_count = numberOf1_out;
                }
             }

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.list_MemoryState;
        }


        public string GetStringBinary(int value)
        {
            string binary_str = Convert.ToString(value, 2);
            char[] binary_arr = binary_str.ToCharArray();
            string bit_used_str = "";
            foreach(var c in binary_arr)
            {
                bit_used_str = c+ "  " + bit_used_str ;
            }

            return bit_used_str;
        }

        private int NumberOf1(int n)
        {
            int count = 0;
            while (n != 0)
            {
                count++;
                n &= n - 1;
            }
            return count;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                int used_count = Convert.ToInt32(this.dataGridView1.Rows[e.RowIndex].Cells["Used_count"].Value);
                if (used_count == 0)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                else if (used_count >= 1 && used_count < 10)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                }
                else if (used_count >= 10 && used_count <= 14)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if ( used_count > 15)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                }
            }
            
        }
    }


}
