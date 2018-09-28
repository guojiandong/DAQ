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
    public partial class TextComponent : Form
    {
        public event SetValueHandler setTextValue;
        public event RemoveHandler removeHandler;
        public event SaveHandler saveHandler;
        public ChangeMode isCreateMode = ChangeMode.Create;
        public ComponentType componentType = ComponentType.TextComponent;
        public TextComponent()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void onAddTextComponent(object sender, EventArgs e)
        {
            System.Console.WriteLine("onAddTextComponent");
            Component com = new Component();
            com.isEnable_Input = this.checkBox1.Checked.ToString();
            com.data_Type = (DataType)this.comboBox1.SelectedIndex;
            com.componentType = (int)ComponentType.TextComponent;
            string offset = this.offset.Text;
            if (string.IsNullOrEmpty(offset))
            {
                MessageBox.Show("偏移量不能为空");
                return;
            }
            com.offset = offset ;
            if (this.setTextValue != null)
                this.setTextValue(com);
        }

        private void onReset(object sender, EventArgs e)
        {
            System.Console.WriteLine("onReset Text Component");

            //reset 
            this.checkBox1.Checked = false;
            this.comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Console.WriteLine("comboBox1_SelectedIndexChanged");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void offset_TextChanged(object sender, EventArgs e)
        {

        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {

        }

        private void TextComponent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.setTextValue != null)
                this.setTextValue = null;
            if (this.removeHandler != null)
                this.removeHandler = null;
            if (this.saveHandler != null)
                this.saveHandler = null;
        }

        private void save_Click(object sender, EventArgs e)
        {
            Component com = new Component();
            com.operatorType = (OperatorType)this.comboBox1.SelectedIndex;
            com.componentType = (int)ComponentType.TextComponent;
            string offset = this.offset.Text;
            bool checkState = this.checkBox1.Checked;
            com.isEnable_Input = checkState.ToString();
            if (string.IsNullOrEmpty(offset))
            {
                MessageBox.Show("偏移量不能为空");
                return;
            }
            com.offset = this.offset.Text;
            com.note = "";

            if (this.saveHandler != null)
                this.saveHandler(com);

            this.Close();
        }

        public void ChangeBtnState(ChangeMode mode)
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

        private void remove_Click(object sender, EventArgs e)
        {
            if (this.removeHandler != null)
                this.removeHandler();
            this.Close();
        }

        public void InitUI(Component com)
        {
            this.offset.Text = com.offset;
            this.checkBox1.Checked = (bool)Convert.ToBoolean(com.isEnable_Input);
            this.comboBox1.SelectedIndex = (int)com.operatorType;
        }
    }
}
