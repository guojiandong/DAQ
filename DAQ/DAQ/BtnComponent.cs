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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void BtnComponent_Load(object sender, EventArgs e)
        {}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {}

        private void onAddBtnComponent(object sender, EventArgs e)
        {
            Component com = new Component();
            this.remove.Hide();
            this.add.Show();
            this.save.Hide();

            com.operatorType = (OperatorType)this.comboBox1.SelectedIndex;
            com.componentType = (int)ComponentType.BtnComponent;

            bool sameAddress = this.checkBox1.Checked;
            com.isEnable_Input = sameAddress.ToString();

            if (!sameAddress)  // 不同地址的時候，寫入、讀取的字偏移，位偏移都必須填寫
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

                this.out_bit_offset.Text = this.in_bit_offset.Text.Trim();
                this.out_word_offset.Text = this.in_word_offset.Text.Trim();
            }

            if (isSameOffsetValue())
            {
                MessageBox.Show("读取，写入的字偏移量，位偏移量不能完全一致！");
                return;
            }
            com.in_bit_offset = CheckEmpty(this.in_bit_offset.Text);
            com.in_word_offset = CheckEmpty(this.in_word_offset.Text);
            com.out_word_offset = CheckEmpty(this.out_word_offset.Text);
            com.out_bit_offset = CheckEmpty(this.out_bit_offset.Text);
            com.note = CheckEmpty(this.note.Text);
            com.offset = "0";
            if (this.pressstate.SelectedIndex == -1)
                this.pressstate.SelectedIndex = 0;
            com.pressType = (PressType)this.pressstate.SelectedIndex;

            if (this.comboBox1.SelectedIndex == -1)
                this.comboBox1.SelectedIndex = 0;
            com.operatorType = (OperatorType)this.pressstate.SelectedIndex;

            if (UInt16.Parse(com.in_bit_offset) > 16 || UInt16.Parse(com.out_bit_offset) > 16 ||
                UInt16.Parse(com.out_word_offset) > 99 || UInt16.Parse(com.in_word_offset) > 16)
            {
                MessageBox.Show("请检查您的字偏移量不能大于100， 位偏移量不能大于16");
                return;
            }

            if (this.setBtnValue != null)
                this.setBtnValue(com);
        }

        private void onResetBtn(object sender, EventArgs e)
        {
            this.checkBox1.Checked = false;
            this.comboBox1.SelectedIndex = 0;
            this.note.Text = "";
            this.in_word_offset.Text = "";
            this.in_bit_offset.Text = "";
            this.out_word_offset.Text = "";
            this.out_bit_offset.Text = "";
            this.pressstate.SelectedIndex = 0;
        }

        private void label3_Click(object sender, EventArgs e)
        {}

        private void note_TextChanged(object sender, EventArgs e)
        {}

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
                this.save.Hide();
            }
            else if (mode == ChangeMode.Change)
            {
                this.add.Hide();
                this.remove.Show();
                this.save.Show();
            }
        }

        public void InitUI(Component com)
        {
            this.note.Text = CheckEmpty( com.note );
            this.checkBox1.Checked = (bool)Convert.ToBoolean(com.isEnable_Input);
            this.comboBox1.SelectedIndex = (int)com.operatorType ;
            this.in_word_offset.Text = CheckEmpty( com.in_word_offset );
            this.in_bit_offset.Text = CheckEmpty( com.in_bit_offset );
            this.out_word_offset.Text = CheckEmpty( com.out_word_offset );
            this.out_bit_offset.Text = CheckEmpty( com.out_bit_offset );
            this.pressstate.SelectedIndex = (int)com.pressType;
        }

        private void save_Click(object sender, EventArgs e)
        {
            Component com = new Component();
            com.operatorType = (OperatorType)this.comboBox1.SelectedIndex;
            com.componentType = (int)ComponentType.BtnComponent;
            string note = this.note.Text;
            bool sameAddress = this.checkBox1.Checked;
            com.isEnable_Input = sameAddress.ToString();

            if (!sameAddress)  // 不同地址的時候，寫入、讀取的字偏移，位偏移都必須填寫
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
                this.out_bit_offset.Text = this.in_bit_offset.Text.Trim();
                this.out_word_offset.Text = this.in_word_offset.Text.Trim();
            }

            if (isSameOffsetValue())
            {
                MessageBox.Show("读取，写入的字偏移量，位偏移量不能完全一致！");
                return;
            }

            com.offset = "0";
            com.in_bit_offset = CheckEmpty(this.in_bit_offset.Text);
            com.in_word_offset = CheckEmpty(this.in_word_offset.Text);
            com.out_word_offset = CheckEmpty(this.out_word_offset.Text);
            com.out_bit_offset = CheckEmpty(this.out_bit_offset.Text);
            com.note = CheckEmpty(this.note.Text);
            if (this.pressstate.SelectedIndex == -1)
                this.pressstate.SelectedIndex = 0;
            com.pressType = (PressType)pressstate.SelectedIndex;

            if (UInt16.Parse(com.in_bit_offset) > 16  || UInt16.Parse(com.out_bit_offset) > 16 || 
                UInt16.Parse(com.out_word_offset) > 99 || UInt16.Parse(com.in_word_offset) > 99 )
            {
                MessageBox.Show("请检查您的字偏移量不能大于100， 位偏移量不能大于16");
                return;
            }

            if (saveHandler != null)
                saveHandler(com);

            this.Close();
        }

        public void Form_BtnComponent_Closed()
        {}

        private void label5_Click(object sender, EventArgs e)
        {}

        private void label2_Click(object sender, EventArgs e)
        {}

        private void in_word_offset_label_Click(object sender, EventArgs e)
        {}


        private void in_word_offset_TextChanged(object sender, EventArgs e)
        {
            int iMax = 100;//首先设置上限值
            if (in_word_offset.Text != null && in_word_offset.Text != "")//判断TextBox的内容不为空，如果不判断会导致后面的非数字对比异常
            {
                if (int.Parse(in_word_offset.Text) > iMax)//num就是传进来的值,如果大于上限（输入的值），那就强制为上限-1，或者就是上限值？
                {
                    in_word_offset.Text = (iMax - 1).ToString();
                }
            }
        }

        private void in_bit_offset_TextChanged(object sender, EventArgs e)
        {
            int iMax = 17;//首先设置上限值
            if (in_bit_offset.Text != null && in_bit_offset.Text != "")//判断TextBox的内容不为空，如果不判断会导致后面的非数字对比异常
            {
                if (int.Parse(in_bit_offset.Text) > iMax)//num就是传进来的值,如果大于上限（输入的值），那就强制为上限-1，或者就是上限值？
                {
                    in_bit_offset.Text = (iMax - 1).ToString();
                }
            }
        }

        private void out_word_offset_TextChanged(object sender, EventArgs e)
        {
            int iMax = 100;//首先设置上限值
            if (out_word_offset.Text != null && out_word_offset.Text != "")//判断TextBox的内容不为空，如果不判断会导致后面的非数字对比异常
            {
                if (int.Parse(out_word_offset.Text) > iMax)//num就是传进来的值,如果大于上限（输入的值），那就强制为上限-1，或者就是上限值？
                {
                    out_word_offset.Text = (iMax - 1).ToString();
                }
            }
        }

        private void out_bit_offset_TextChanged(object sender, EventArgs e)
        {
            int iMax = 17;//首先设置上限值
            if (out_bit_offset.Text != null && out_bit_offset.Text != "")//判断TextBox的内容不为空，如果不判断会导致后面的非数字对比异常
            {
                if (int.Parse(out_bit_offset.Text) > iMax)//num就是传进来的值,如果大于上限（输入的值），那就强制为上限-1，或者就是上限值？
                {
                    out_bit_offset.Text = (iMax - 1).ToString();
                }
            }
        }

        private void label5_Click_1(object sender, EventArgs e)
        {}

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

        public bool isSameOffsetValue()
        {
            if (this.checkBox1.Checked)
            {
                return false;
            }
            else
            {
                if (this.in_bit_offset.Text.Trim() == this.out_bit_offset.Text.Trim() && 
                    this.in_word_offset.Text.Trim() == this.out_word_offset.Text.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        private void offset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void out_word_offset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void out_bit_offset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void in_word_offset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void in_bit_offset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {}

        private void pressstate_SelectedIndexChanged(object sender, EventArgs e)
        {}
    }
}
