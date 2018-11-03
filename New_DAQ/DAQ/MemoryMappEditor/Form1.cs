using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleExcel;

namespace MemoryMappEditor
{
    public partial class Form1 : Form
    {
        //DataProxy.GetInstance();
        public Form1()
        {
            InitializeComponent();
            DataProxy.GetInstance().InitDataProxy();
        }

        private void BaseHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseHeaderForm baseHeaderForm = new BaseHeaderForm();
            baseHeaderForm.ShowDialog();
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.CheckIn);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void rigsterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.Rigseter);
        }

        private void eapCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.EapCommand);
        }

        private void alarmTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.AlarmText);
        }

        private void heartBeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.HeartBeat);
        }

        private void motorSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.MotorSettings);
        }

        private void normalSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.NormalSettings);
        }

        private void eventChangedFlagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.EventChangedFlag);
        }

        private void breakPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.BreakPoint);
        }

        private void alarmCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.AlarmCode);
        }

        private void machineStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.MachineStatus);
        }

        private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEventForm(EventCode.CheckOut);
        }

        public void InitEventForm(EventCode code)
        {
            EventForm eventForm = new EventForm();
            eventForm.InitComponent(code);
            eventForm.ShowDialog();
        }

        private void exportExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var workbook = new WorkBook(ExcelVersion.V2007);
            var workbook = new WorkBook(@"C:\Users\luxshare-ict\Desktop\I.work\Editor\Test1.xls");
            var sheet1 = workbook.GetSheet(0);
            var sheet2 = workbook.NewSheet("PLC");
            var sheet3 = workbook.NewSheet("EAP");

            //直接给单元格赋值
            sheet1.Rows[0][0].Value = "Hello";

            var list = DataProxy.GetInstance().plcMemory;
            //将List对象添加到工作表中
            sheet2.ConvertFromQuery(list, 1);


            var list_eap = DataProxy.GetInstance().eapMemory;
            //将List对象添加到工作表中
            sheet3.ConvertFromQuery(list_eap, 1);


            string path = @"C:\Users\luxshare-ict\Desktop\I.work\Editor" + @"\test.xlsx";
            workbook.Save(path);
        }

        private void FixedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void EventDefineListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
