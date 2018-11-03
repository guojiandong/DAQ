using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryMappEditor
{
    public partial class EventForm : Form
    {
        private const String DEFAULT_INT_VALUE_ZERO = "0";
        public EventForm()
        {
            InitializeComponent();
            SetDefault(txtVersion);
            SetDefault(txtStationID);
        }

        public void InitComponent(EventCode code)
        {
            int code_int = (int)code;
            comboBox1.SelectedIndex = code_int - 2;
            comboBox1.Enabled = false;
            int data_size = DataProxy.GetInstance().Event2DataSizeMap[code_int];
            txtDataSize.Text = data_size.ToString();
            txtDataSize.ForeColor = Color.Gray;
            editSubItem.Visible = code == EventCode.MotorSettings || code == EventCode.NormalSettings || code == EventCode.AlarmText;
        }
        public void SetDefault(TextBox text)
        {
            text.Text = DEFAULT_INT_VALUE_ZERO;
            text.ForeColor = Color.Gray;
        }
        private void SetDefaultText(TextBox text)
        {
            text.Text = DEFAULT_INT_VALUE_ZERO;
            text.ForeColor = Color.Gray;
        }

        private void txtVersion_Enter(object sender, EventArgs e)
        {
            if (txtVersion.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtVersion.Text = "";
                txtVersion.ForeColor = Color.Black;
            }
        }
        private void txtVersion_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtVersion.Text))
                SetDefaultText(txtVersion);
        }

        private void txtDataSize_Enter(object sender, EventArgs e)
        {
            if (txtDataSize.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtDataSize.Text = "";
                txtDataSize.ForeColor = Color.Black;
            }
        }
        private void txtDataSize_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDataSize.Text))
                SetDefaultText(txtDataSize);
        }

        private void txtStationID_Enter(object sender, EventArgs e)
        {
            if (txtStationID.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtStationID.Text = "";
                txtStationID.ForeColor = Color.Black;
            }
        }
        private void txtStationID_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtStationID.Text))
                SetDefaultText(txtStationID);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            int curSelectedIndex = this.comboBox1.SelectedIndex;
            if (curSelectedIndex == -1)
            {
                MessageBox.Show("请选择指令");
                return;
            }

            string curDataSize = this.txtDataSize.Text;
            if (string.IsNullOrEmpty(curDataSize) || int.Parse(curDataSize) == 0)
            {
                MessageBox.Show("DataSize 属性不能为空或O");
                return;
            }
            int curDataSize_int = int.Parse(curDataSize);
            int curEventCode = curSelectedIndex + 2;
            int lastEventDataSize = DataProxy.GetInstance().Event2DataSizeMap[curEventCode];
            if (curDataSize_int != lastEventDataSize)
            {
                DataProxy.GetInstance().SetEvent2DataSizeMapByEventCode(curEventCode, curDataSize_int);
                DataProxy.GetInstance().InitDataProxy();
            }
            this.Close();
        }

        private void editSubItem_Click(object sender, EventArgs e)
        {
            MotorForm motorForm = new MotorForm();
            motorForm.ShowDialog();
        }
    }
}
