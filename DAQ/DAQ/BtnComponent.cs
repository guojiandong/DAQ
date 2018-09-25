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

            string offset = this.offset.Text;
            string note = this.note.Text;
            bool checkState = this.checkBox1.Checked;
            com.isEnable_Input = checkState.ToString();

            com.in_bit_offset = this.in_bit_offset.Text;
            com.in_word_offset = this.in_word_offset.Text;
            com.out_word_offset = this.out_word_offset.Text;
            com.out_bit_offset = this.out_bit_offset.Text;

            if (string.IsNullOrEmpty(offset))
            {
                MessageBox.Show("偏移量不能为空");
                return;
            }
            com.offset = this.offset.Text;
            com.note = note;
            
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
            this.offset.Text = com.offset;
            if (!string.IsNullOrEmpty(com.note))
                this.note.Text = com.note;
            this.checkBox1.Checked = (bool)Convert.ToBoolean(com.isEnable_Input);
            this.comboBox1.SelectedIndex = (int)com.operatorType ; 
        }

        private void save_Click(object sender, EventArgs e)
        {
            Component com = new Component();
            com.operatorType = (OperatorType)this.comboBox1.SelectedIndex;
            com.componentType = (int)ComponentType.BtnComponent;
            string offset = this.offset.Text;
            string note = this.note.Text;
            bool checkState = this.checkBox1.Checked;
            com.isEnable_Input = checkState.ToString();
            if (string.IsNullOrEmpty(offset))
            {
                MessageBox.Show("偏移量不能为空");
                return;
            }
            com.offset = this.offset.Text;
            com.note = note;

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
    }
}
