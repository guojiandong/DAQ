using Ksat.AppPlugIn.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestSocketConsole;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging;
using Ksat.AppPlugIn.Utils;
using Ksat.AppPlugIn.Communicate.FastIocp.Base;

namespace TestModBusForm
{
    public enum Func2Index
    {
        Read_Circle = 0,
        Read_Register = 1,
        Read_Discrete = 2,
        //Read_Input_Register = 0x04,
        Write_Single_Circle = 3,
        Write_Single_Register = 4,
        Write_Multi_Circle = 5,
        Write_Multi_Register = 6,
    }

    public partial class Form1 : Form
    {
        static byte[] HEADER_FLAG = new byte[] { 0x90, 0x91, 0x00, 0x00 };
        PlcMobBusSocketClientTask task;
        public string IP = "192.168.2.7";
        public int Port = 502;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTask();
        }

        public void LoadTask()
        {
            task = new PlcMobBusSocketClientTask();
            MultiThreadManager.Instance().AddTask(task);
            
            Console.WriteLine("Press any ke to exit.");
          //  task.Cancel();
        }

        private void ShowModbusData(byte[] modbus, int length, bool isSend)
        {
            string msg = "";// = BinaryUtil.ByteToHex(modbus, 0, length);

            string msgType = isSend ? " 发送 : " : " 接收 ：";

            textBox3.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + msgType + " :" +
                msg + Environment.NewLine);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            task.Cancel();
        }

        // 发送数据
        private void button3_Click(object sender, EventArgs e)
        {
            byte[] msg = null;

            if (this.comboBox1.SelectedIndex == (int)Func2Index.Read_Circle)
            {
                msg = ModBusTcpHelper.BuildReadCoilCommand(this.textBox4.Text, this.textBox5.Text);
            }
            else if (this.comboBox1.SelectedIndex == (int)Func2Index.Read_Discrete)
            {
                msg = ModBusTcpHelper.BuildReadDiscreteCommand(this.textBox4.Text, this.textBox5.Text);
            }
            else if (this.comboBox1.SelectedIndex == (int)Func2Index.Read_Register)
            {
                msg = ModBusTcpHelper.BuildReadRegisterCommand(this.textBox4.Text, this.textBox5.Text);
            }
            else if (this.comboBox1.SelectedIndex == (int)Func2Index.Write_Single_Circle)
            {
                msg = ModBusTcpHelper.BuildWriteOneCoilCommand(this.textBox4.Text, this.checkBox1.Checked);
            }
            else if (this.comboBox1.SelectedIndex == (int)Func2Index.Write_Single_Register)
            {
                msg = ModBusTcpHelper.BuildWriteOneRegisterCommand(this.textBox4.Text, BinaryUtil.HexToByte(this.textBox6.Text));
            }
            else if (this.comboBox1.SelectedIndex == (int)Func2Index.Write_Multi_Register)
            {
                msg = ModBusTcpHelper.BuildWriteMultiRegisterCommand(this.textBox4.Text, this.textBox5.Text, BinaryUtil.HexToByte(this.textBox6.Text));
            }
            else if (this.comboBox1.SelectedIndex == (int)Func2Index.Write_Multi_Circle)
            {
                msg = ModBusTcpHelper.BuildWriteMultiCoilCommand(this.textBox4.Text, this.textBox5.Text, this.textBox6.Text);
            }

            BeginInvoke(new Action<byte[], int, bool>(ShowModbusData), msg, msg.Length, true);

            ModBusTcpDataFrameMessage data = new ModBusTcpDataFrameMessage(msg, 0);
            IConnection iconn = task.GetConnectionByTag(IP);
            if (iconn != null)
                iconn.BeginSend(data.ToPacket());
        }

        // 断开链接
        private void button2_Click(object sender, EventArgs e)
        {
            task.Disconnect(IP);
        }


        // 连接
        private void button1_Click(object sender, EventArgs e)
        {
            string address = this.textBox1.Text;
            int port = int.Parse(this.textBox2.Text);
            if (!string.IsNullOrEmpty(address))
            {
                bool isConnect = task.TryConnect(address,address, port,null);
            }
        }


        public AbstractModBusTcpMessage Parse(/*IConnection connection,*/ byte[] buffer, out int readlength)
        {
            readlength = 0;
            if (buffer.Length < (HEADER_FLAG.Length))
            {
            }

            var receiveBuffer = buffer;
            int maxIndex = 0 + buffer.Length;
            int loopIndex = 0;
            try
            {
                while (loopIndex < maxIndex)
                {
                    if (receiveBuffer.Mark(0, buffer.Length, loopIndex, HEADER_FLAG))
                    {
                        loopIndex += HEADER_FLAG.Length;

                        AbstractModBusTcpMessage msg = TryParse(receiveBuffer, loopIndex, maxIndex - loopIndex, out readlength);
                        if (msg == null)
                        {
                            break;
                        }
                        readlength += loopIndex;
                        return msg;
                    }
                    else
                    {
                        loopIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            readlength = 0;
            return null;
        }


        public AbstractModBusTcpMessage TryParse(byte[] buffer, int offset, int length, out int readlength)
        {
            AbstractModBusTcpMessage msg = null;
            readlength = 0;
            int oldOffset = offset;
            offset += 3; // jump the 报文长度 2-byte  单元标识 1-byte

            int bit8 = (buffer[offset] & 128) == 128 ? 1 : 0;  // offset = 7 releative
            byte errorCode = 0x00;
            if (bit8 == 1) //error
            {
                offset += 1; // 异常状态码
                int error_offset = offset + 1;
                errorCode = buffer[error_offset];
                //TODO Error errorCode
                try
                {
                    msg = new ModBusTcpDataFrameMessage(buffer, oldOffset - 4);
                    if (msg != null)
                    {
                        readlength = 8 + 1; // 8: 标识MODBUS的前七个 标识位byte 1:一个错误码的byte
                    }
                }
                catch
                {
                    msg = null;
                }
            }
            else // success 
            {
                offset -= 3; // 回退到 标识后面数据的长度所在的位置byte[4] byte[5]
                int data_length = Ksat.AppPlugIn.Utils.NetworkBitConverter.ToInt16(buffer, offset);
                try
                {
                    msg = new ModBusTcpDataFrameMessage(buffer, oldOffset - 4);
                    if (msg != null)
                    {
                        readlength = 6 + data_length; // 6: 标识MODBUS的前六个byte
                    }
                }
                catch (Exception e)
                {
                    Exception e1 = e;
                    msg = null;
                }
            }
            return msg;

        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
