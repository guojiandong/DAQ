using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public List<Component> listViewData;
        BtnComponent form_Btn;
        TextComponent form_Text;
        public Int16[] memory_block = new Int16[100];  // 開闢100個int16位數組

        public Form1()
        {
            listViewData = new List<Component>();
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Console.WriteLine("listView1_SelectedIndexChanged");
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
            form_Btn.ShowDialog(); //FormClosedEventHandler
                                   // form_Btn.FormClosing += new FormClosingEventHandler(Form_BtnComponent_Closed);
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

        public void _removeHandler()
        {
            int indexDel = listView1.Items.IndexOf(listView1.FocusedItem);
            listView1.Items.RemoveAt(indexDel);//删除
        }

        public void _saveHandler(Component com)
        {
            ListViewItem item = listView1.FocusedItem;
            item.SubItems[0].Text = CheckEmpty(com.componentType.ToString());
            item.SubItems[1].Text = CheckEmpty(com.isEnable_Input);
            item.SubItems[2].Text = CheckEmpty(com.data_Type.ToString());
            item.SubItems[3].Text = CheckEmpty(com.operatorType.ToString());
            item.SubItems[4].Text = CheckEmpty(com.offset);
            item.SubItems[5].Text = CheckEmpty(com.note);
        }

        public void AddComponent(Component com)
        {
            System.Console.WriteLine("AddComponent + componentType: {0}", com.componentType.ToString());

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

            this.listView1.Items.Add(lt);
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
                if (componentType == 2)
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
                    com.operatorType = OperatorType.Auto;
                    com.offset = comValue[4];
                    com.in_word_offset = comValue[5];
                    com.in_bit_offset = comValue[6];
                    com.out_word_offset = comValue[7];
                    com.out_bit_offset = comValue[8];
                    if (comValue.Count >= 9 && !string.IsNullOrEmpty(comValue[8]))
                        com.note = comValue[8];

                    form2.InitUI(com);
                    form2.ShowDialog();

                }
                else if (componentType == 1)
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

                    form2.InitUI(com);
                    form2.ShowDialog();
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        //把ListView数据写入XML
        private void exportXml_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable("Test");
            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                dataTable.Columns.Add(listView1.Columns[i].Text);
            }
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                ListViewItem listViewItem = listView1.Items[i];
                int curLineCount = listViewItem.SubItems.Count;
                for (int j = 0; j < listView1.Columns.Count; ++j)
                {
                    if (j < curLineCount)
                        dataRow[j] = listView1.Items[i].SubItems[j].Text;
                    else
                        dataRow[j] = "空值";
                }
                dataTable.Rows.Add(dataRow);
            }

            OpenFileDialog dialog = new OpenFileDialog();
            string file = string.Empty;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = dialog.FileName;
            }
            dataTable.WriteXml("C:\\Users\\luxshare-ict\\Desktop\\ABC.xml");//C:\Users\luxshare-ict\Desktop\I.work
            dataTable.Dispose();
            MessageBox.Show("導出成功");
        }


        //把XML数据读入ListView
        private void importXml_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            /*
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有xml文件(*.*)|*.xml";
            string file = string.Empty;
             
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = dialog.FileName;
            }
            */

            //"C:\\Users\\luxshare-ict\\Desktop\\ABC.xml"
            dataSet.ReadXml("C:\\Users\\luxshare-ict\\Desktop\\ABC.xml");
            DataTable dataTable = dataSet.Tables[0];
            listView1.Columns.Clear();
            listView1.Items.Clear();
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                listView1.Columns.Add(dataTable.Columns[i].ColumnName);
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                ListViewItem listViewItem = new ListViewItem(dataTable.Rows[i][0].ToString());
                for (int j = 1; j < dataTable.Columns.Count; j++)
                {
                    listViewItem.SubItems.Add(dataTable.Rows[i][j].ToString());
                }
                listView1.Items.Add(listViewItem);
            }
            dataTable.Dispose();
            dataSet.Dispose();
        }

        private void ClearListView_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
        }

        public static string CheckEmpty(string value)
        {
            string str = "0";
            if (string.IsNullOrEmpty(value))
                return str;
            return value;

        }

        // 位操作 系列  memory_block[100]  int16   Text類型占一個int16   btn類型：不定
        /*
         * |----------------------int16--------------------|.................................................. 
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
        /// 檢查Int16 類型的數據的某一位是1還是0.
        /// </summary>
        /// <param name="value">被檢查的值</param>
        /// <param name="index">第幾位</param>
        /// <returns>檢測某一位是否已使用 true表示该位为1，false表示该位为0</returns>
        public bool check_bit_used(Int16 value, Int16 index)
        {
            return (value >> (index - 1) & 0x01) == 1;
        }

        /// <summary>
        /// 设定Int16数据中某一位的值
        /// </summary>设定前的值</param>
        /// <param name="index">16
        /// <param name="value">位位数据的从右向左的偏移位索引(0~15)</param>
        /// <param name="bitValue">true设该位为1,false设为0</param>
        /// /// <returns>返回位设定后的值</returns>
        public static UInt16 set_bit_value(UInt16 value, int index, bool bitValue)         //btn
        {
            if (index > 15) throw new ArgumentOutOfRangeException("index"); //索引出错
            int v = index < 2 ? index : (2 << (index - 2));
            return bitValue ? (UInt16)(value | v) : (UInt16)(value & ~v);
        }

        public static UInt16 MAX_UInt16 = 32767;
        /// <summary>
        /// 設置整個字的值 適用于text類型
        /// </summary>
        /// <param name="block"></param>
        /// <param name="index"></param>
        public static void set_word_value(ref UInt16[] block, int index)                   //text
        {
            if (index > 99) throw new ArgumentOutOfRangeException("index"); //索引出错
            block[index] = MAX_UInt16;
        }

        /// <summary>
        /// 檢查字是否被使用
        /// </summary>
        /// <param name="block"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool check_word_used(ref UInt16[] block, int index)
        {
            if (index > 99) throw new ArgumentOutOfRangeException("index"); //索引出错
            if (block[index] > 0)
                return true;
            return false;
        }
    }


}
