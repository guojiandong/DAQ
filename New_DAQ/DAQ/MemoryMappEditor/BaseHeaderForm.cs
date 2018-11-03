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
    public partial class BaseHeaderForm : Form
    {
        //TextBox的默认值
        private const String DEFAULT_INT_VALUE_ZERO = "0";
        public BaseHeaderForm()
        {
            InitializeComponent();
            SetDefault(txtVersion);
            SetDefault(txtEventCount);
            SetDefault(txtLineID);
            SetDefault(txtProjectName);
            SetDefault(txtReserved);
            SetDefault(txtRevision);
            SetDefault(txtSectionID);
            SetDefault(txtMachineID);
            SetDefault(txtPlcFlagBegin, ConstFile.PLC_EVENT_DATA_START.ToString());
            SetDefault(txtPlcWriteFlag, ConstFile.Flag);
            SetDefault(txtEapFlag, ConstFile.Flag);
            SetDefault(txtEapWriteBegin, ConstFile.EAP_EVENT_DATA_START.ToString());

            this.comboBox_HeartBeatPriority.SelectedIndex = 3;
            this.comboBox_PollPriority.SelectedIndex = 3;
        }

        public void SetDefault(TextBox text,string defulat = DEFAULT_INT_VALUE_ZERO)
        {
            text.Text = defulat;
            text.ForeColor = Color.Gray;
        }

        private void SetDefaultText(TextBox text, string defulat = DEFAULT_INT_VALUE_ZERO)
        {
            text.Text = defulat;
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
        private void txtRevision_Enter(object sender, EventArgs e)
        {
            if (txtRevision.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtRevision.Text = "";
                txtRevision.ForeColor = Color.Black;
            }
        }
        private void txtRevision_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtRevision.Text))
                SetDefaultText(txtRevision);
        }
        private void txtProjectName_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtProjectName_Enter(object sender, EventArgs e)
        {
            if (txtProjectName.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtProjectName.Text = "";
                txtProjectName.ForeColor = Color.Black;
            }
        }
        private void txtProjectName_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtProjectName.Text))
                SetDefaultText(txtProjectName);
        }
        private void txtLineID_Enter(object sender, EventArgs e)
        {
            if (txtLineID.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtLineID.Text = "";
                txtLineID.ForeColor = Color.Black;
            }
        }
        private void txtLineID_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtLineID.Text))
                SetDefaultText(txtLineID);
        }
        private void txtSectionID_Enter(object sender, EventArgs e)
        {
            if (txtSectionID.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtSectionID.Text = "";
                txtSectionID.ForeColor = Color.Black;
            }
        }
        private void txtSectionID_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSectionID.Text))
                SetDefaultText(txtSectionID);
        }
        private void txtMachineID_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtMachineID.Text))
                SetDefaultText(txtMachineID);
        }
        private void txtMachineID_Enter(object sender, EventArgs e)
        {
            if (txtMachineID.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtMachineID.Text = "";
                txtMachineID.ForeColor = Color.Black;
            }
        }
        private void txtReserved_Enter(object sender, EventArgs e)
        {
            if (txtReserved.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtReserved.Text = "";
                txtReserved.ForeColor = Color.Black;
            }
        }
        private void txtReserved_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtReserved.Text))
                SetDefaultText(txtReserved);
        }
        private void txtPlcWriteFlag_Enter(object sender, EventArgs e)
        {
            if (txtPlcWriteFlag.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtPlcWriteFlag.Text = "";
                txtPlcWriteFlag.ForeColor = Color.Black;
            }
        }
        private void txtPlcWriteFlag_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPlcWriteFlag.Text))
                SetDefaultText(txtPlcWriteFlag,ConstFile.Flag);
        }
        private void txtPlcFlagBegin_Enter(object sender, EventArgs e)
        {
            if (txtPlcFlagBegin.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtPlcFlagBegin.Text = "";
                txtPlcFlagBegin.ForeColor = Color.Black;
            }
        }
        private void txtPlcFlagBegin_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPlcFlagBegin.Text))
                SetDefaultText(txtPlcFlagBegin,ConstFile.PLC_EVENT_DATA_START.ToString());
        }
        private void txtEapFlag_Enter(object sender, EventArgs e)
        {
            if (txtEapFlag.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtEapFlag.Text = "";
                txtEapFlag.ForeColor = Color.Black;
            }
        }
        private void txtEapFlag_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtEapFlag.Text))
                SetDefaultText(txtEapFlag, ConstFile.Flag);
        }
        private void EapWriteBegin_Enter(object sender, EventArgs e)
        {
            if (txtEapWriteBegin.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtEapWriteBegin.Text = "";
                txtEapWriteBegin.ForeColor = Color.Black;
            }
        }
        private void EapWriteBegin_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtEapWriteBegin.Text))
                SetDefaultText(txtEapWriteBegin,ConstFile.EAP_EVENT_DATA_START.ToString());
        }
        private void txtEventCode_Enter(object sender, EventArgs e)
        {
            if (txtEventCount.Text == DEFAULT_INT_VALUE_ZERO)
            {
                txtEventCount.Text = "";
                txtEventCount.ForeColor = Color.Black;
            }
        }
        private void txtEventCode_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtEventCount.Text))
                SetDefaultText(txtEventCount);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            BaseHeader baseHead = new BaseHeader();
            baseHead.Version = txtVersion.Text;
            baseHead.Revision = txtRevision.Text;
            baseHead.ProjectName = txtProjectName.Text;
            baseHead.LineID = int.Parse(txtLineID.Text);
            baseHead.SectionID = int.Parse(txtSectionID.Text);
            baseHead.MachineID = int.Parse(txtMachineID.Text);
            baseHead.PollPriority = (PriorityMode)comboBox_PollPriority.SelectedIndex;
            baseHead.HeartbeatPriority = (PriorityMode)comboBox_HeartBeatPriority.SelectedIndex;
            baseHead.Reserved = int.Parse(txtReserved.Text);
            baseHead.PlcWriteAddressFlag = txtPlcWriteFlag.Text.ToString();
            baseHead.PlcWriteAddressBegin = int.Parse(txtPlcFlagBegin.Text);
            baseHead.EapWriteAreaFlag = txtEapFlag.Text.ToString();
            baseHead.EapWriteAreaBegin = int.Parse(txtEapWriteBegin.Text);
            baseHead.EventCount = int.Parse(txtEventCount.Text);

            DataProxy.GetInstance().baseHeader = baseHead;
            DataProxy.GetInstance().DataJitter();
        }
    }
}
