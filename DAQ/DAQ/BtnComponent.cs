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
    public partial class BtnComponent : Form
    {
        public event SetValueHandler setBtnValue;
        public event RemoveHandler removeHandler;
        public event SaveHandler saveHandler;
        public ChangeMode isCreateMode = ChangeMode.Create;
        public ComponentType componentType = ComponentType.BtnComponent;
        public BtnComponent()
        {
            InitializeComponent();
        }

        private void BtnComponent_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void onAddBtnComponent(object sender, EventArgs e)
        {
            Component com = new Component();
            this.remove.Hide();
            this.add.Show();

            com.operatorType = (OperatorType)this.comboBox1.SelectedIndex;
            com.componentType = (int)ComponentType.BtnComponent;

            bool diffAddress = this.checkBox1.Checked;
            com.isEnable_Input = diffAddress.ToString();

            if (diffAddress)  // 不同地址的時候，寫入、讀取的字偏移，位偏移都必須填寫
            {
                if (string.IsNullOrEmpty(this.in_bit_offset.Text) || string.IsNullOrEmpty(this.in_word_offset.Text) || 
                    string.IsNullOrEmpty(this.out_word_offset.Text) || string.IsNullOrEmpty(this.out_bit_offset.Text))
                    {
                        MessageBox.Show("讀取，寫入偏移量均不能为空");
                        return;
                    }
            }else    // 相同的寫入讀取地址，可以值寫一行
            {
                if (string.IsNullOrEmpty(this.in_bit_offset.Text) || string.IsNullOrEmpty(this.in_word_offset.Text))
                {
                    MessageBox.Show("字偏移量，位偏移量均不能为空");
                    return;
                }
            }

            com.in_bit_offset = CheckEmpty(this.in_bit_offset.Text);
            com.in_word_offset = CheckEmpty(this.in_word_offset.Text);
            com.out_word_offset = CheckEmpty(this.out_word_offset.Text);
            com.out_bit_offset = CheckEmpty(this.out_bit_offset.Text);
            com.note = CheckEmpty(this.note.Text);
            com.offset = "0";

            if (this.setBtnValue != null)
                this.setBtnValue(com);
        }

        private void onResetBtn(object sender, EventArgs e)
        {
            //reset
            this.checkBox1.Checked = false;
            this.comboBox1.SelectedIndex = 0;
            this.offset.Text = "";
            this.note.Text = "";
            this.in_word_offset.Text = "";
            this.in_bit_offset.Text = "";
            this.out_word_offset.Text = "";
            this.out_bit_offset.Text = "";
        }

        private void offset_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void note_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnComponent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (setBtnValue != null)
                this.setBtnValue = null;
            if (removeHandler != null)
                this.removeHandler = null;
            if (saveHandler != null)
                this.saveHandler = null;
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (removeHandler != null)
                removeHandler();
            this.Close();
        }

        public void ChangeBtnState( ChangeMode mode)
        {
            if (mode == ChangeMode.Create)
            {
                this.add.Show();
                this.remove.Hide();
            }
            else if (mode == ChangeMode.Change)
            {
                this.add.Hide();
                this.remove.Show();
            }
        }

        public void InitUI(Component com)
        {
            this.offset.Text = CheckEmpty( com.offset );
            this.note.Text = CheckEmpty( com.note );
            this.checkBox1.Checked = (bool)Convert.ToBoolean(com.isEnable_Input);
            this.comboBox1.SelectedIndex = (int)com.operatorType ;
            this.in_word_offset.Text = CheckEmpty( com.in_word_offset );
            this.in_bit_offset.Text = CheckEmpty( com.in_bit_offset );
            this.out_word_offset.Text = CheckEmpty( com.out_word_offset );
            this.out_bit_offset.Text = CheckEmpty( com.out_bit_offset );
        }

        private void save_Click(object sender, EventArgs e)
        {
            Component com = new Component();
            com.operatorType = (OperatorType)this.comboBox1.SelectedIndex;
            com.componentType = (int)ComponentType.BtnComponent;
            string offset = this.offset.Text;
            string note = this.note.Text;
            bool diffAddress = this.checkBox1.Checked;
            com.isEnable_Input = diffAddress.ToString();

            if (diffAddress)  // 不同地址的時候，寫入、讀取的字偏移，位偏移都必須填寫
            {
                if (string.IsNullOrEmpty(this.in_bit_offset.Text) || string.IsNullOrEmpty(this.in_word_offset.Text) ||
                    string.IsNullOrEmpty(this.out_word_offset.Text) || string.IsNullOrEmpty(this.out_bit_offset.Text))
                {
                    MessageBox.Show("讀取，寫入偏移量均不能为空");
                    return;
                }
            }
            else    // 相同的寫入讀取地址，可以值寫一行
            {
                if (string.IsNullOrEmpty(this.in_bit_offset.Text) || string.IsNullOrEmpty(this.in_word_offset.Text))
                {
                    MessageBox.Show("字偏移量，位偏移量均不能为空");
                    return;
                }
            }

            com.offset = "0";
            com.in_bit_offset = CheckEmpty(this.in_bit_offset.Text);
            com.in_word_offset = CheckEmpty(this.in_word_offset.Text);
            com.out_word_offset = CheckEmpty(this.out_word_offset.Text);
            com.out_bit_offset = CheckEmpty(this.out_bit_offset.Text);
            com.note = CheckEmpty(this.note.Text);

            if (saveHandler != null)
                saveHandler(com);

            this.Close();
        }

        public void Form_BtnComponent_Closed()
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void in_word_offset_TextChanged(object sender, EventArgs e)
        {

        }

        private void in_word_offset_label_Click(object sender, EventArgs e)
        {

        }

        private void in_bit_offset_TextChanged(object sender, EventArgs e)
        {

        }

        private void out_word_offset_TextChanged(object sender, EventArgs e)
        {

        }

        private void out_bit_offset_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = this.checkBox1.Checked;
            this.out_word_offset.Enabled = !isChecked;
            this.out_bit_offset.Enabled = !isChecked;
        }

        public static string CheckEmpty(string value)
        {
            string str = "0";
            if (string.IsNullOrEmpty(value))
                return str;
            return value;

        }
    }
}
