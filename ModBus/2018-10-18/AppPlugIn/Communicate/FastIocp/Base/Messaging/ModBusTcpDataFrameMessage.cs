using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn;
using Ksat.AppPlugIn.Communicate.FastIocp.Base;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging
{
    /// <summary>
    /// ModBud Tcp 格式 ADU = ( MBAP + MODBUS_REQ)                  (12-byte)
    ///                                 
    /// Trans_Idt_Hi   Trans_Idt_Lo        00 00         // 传输标识 2-byte
    /// 
    /// Protocol_Idt_Hi  Protocol_Idt_Lo   00 00         // 协议标识 2-byte
    /// 
    /// Data_Length_Hi   Data_Length_Lo    00 08         // 报文长度 2-byte
    ///  
    /// Unit_Idt                           FF            // 单元标识 1-byte
    /// 
    /// ---------------------------------------------------------
    /// func_Idt                           01            // 功能码 1-byte  01：读线圈  02：读离散量输入  03：读多个寄存器  04：读输入寄存器 05：写单个线圈  06：写单个寄存器 10：写多个寄存器 0F：写多个线圈 
    /// 
    /// Addr_Start_Hi   Addr_Start_Lo      00 05         // 起始地址 2-byte
    /// 
    /// Register_Ct_Hi  Register_Ct_Lo     00  01        // (寄存器)数量 2-byte
    ///     
    /// 
    /// Data_Length_* : 后面的所有字符的字节数
    /// 


    public enum Func_Code    // 功能码
    {
        Read_Circle = 0x01,
        Read_Input_Discrete = 0x02,
        Read_Register = 0x03,
        Read_Input_Register = 0x04,
        Write_Single_Circle = 0x05,
        Write_Single_Register = 0x06,
        Write_Multi_Register = 0x0A,
        Write_Multi_Circle = 0x0F,
    }

    public class ModBusTcpDataFrameMessage : AbstractModBusTcpMessage
    {
        public ModBusTcpDataFrameMessage(int seqID = 0) : base(seqID)
        {
            this.Trans_Idt_Hi = 0x00;
            this.Trans_Idt_Lo = 0x00;
            this.Protocol_Idt_Hi = 0x00;
            this.Protocol_Idt_Lo = 0x00;
            this.Data_Length_Hi = 0x00;
            this.Data_Length_Lo = 0x00;
            this.Unit_Idt = 0xff;

            this.func_Idt = 0x00;
            this.ErrorCode = 0;
        }

        public byte Trans_Idt_Hi { get; set; }
        public byte Trans_Idt_Lo { get; set; }

        public byte Protocol_Idt_Hi { get; set; }
        public byte Protocol_Idt_Lo { get; set; }

        public byte Data_Length_Hi { get; set; }
        public byte Data_Length_Lo { get; set; }

        public byte Unit_Idt { get; set; }

        public byte func_Idt { get; set; }

        public byte[] Payload { get; set; }
        public int PayloadLength { get; set; }


        public override AbstractMessageBase Clone()
        {
            return new ModBusTcpDataFrameMessage(this);
        }

        public ModBusTcpDataFrameMessage(ModBusTcpDataFrameMessage other) : base(other.SeqID)
        {
            this.CopyFrom(other);
        }

        public ModBusTcpDataFrameMessage(byte[] buffer, int offset) : base(0)
        {
            int old_offset = offset;
            this.Trans_Idt_Hi = buffer[offset++];
            this.Trans_Idt_Lo = buffer[offset++];

            this.Protocol_Idt_Hi = buffer[offset++];
            this.Protocol_Idt_Lo = buffer[offset++];

            // 跳过modbus的前4个字节 获取协议的数据长度
            int data_length = Ksat.AppPlugIn.Utils.NetworkBitConverter.ToInt16(buffer, offset);

            this.Data_Length_Hi = buffer[offset++];
            this.Data_Length_Lo = buffer[offset++];
            this.Unit_Idt = buffer[offset++];
            this.func_Idt = buffer[offset++];

            //bool isRead = IsReadFunc(this.func_Idt);

            this.PayloadLength = data_length;
            this.Payload = new byte[data_length];
            Buffer.BlockCopy(buffer, old_offset + 6, this.Payload, 0, data_length);
        }

        public bool IsReadFunc(byte func_code)
        {
            if (func_code == (int)Func_Code.Read_Input_Discrete || func_code == (int)Func_Code.Read_Input_Register ||
                func_code == (int)Func_Code.Read_Register || func_code == (int)Func_Code.Read_Circle)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("ModBusTcpDataFrameMessage").Append(",");
            str.AppendFormat("{0:X2}", this.Trans_Idt_Hi).Append(",");
            str.AppendFormat("{0:X2}", this.Trans_Idt_Lo).Append(",");
            str.AppendFormat("{0:X2}", this.Protocol_Idt_Hi).Append(",");
            str.AppendFormat("{0:X2}", this.Protocol_Idt_Lo).Append(",");
            str.AppendFormat("{0:X2}", this.Data_Length_Hi).Append(",");
            str.AppendFormat("{0:X2}", this.Data_Length_Lo).Append(",");
            str.AppendFormat("{0:X2}", this.Unit_Idt).Append(",");
            str.AppendFormat("{0:X2}", this.func_Idt).Append(",");
            return str.ToString();
        }

        public override Packet ToPacket()
        {
            byte[] payload = new byte[6 + PayloadLength];
            int offset = 0;
            payload[offset++] = this.Trans_Idt_Hi;
            payload[offset++] = this.Trans_Idt_Lo;
            payload[offset++] = this.Protocol_Idt_Hi;
            payload[offset++] = this.Protocol_Idt_Lo;
            payload[offset++] = this.Data_Length_Hi;
            payload[offset++] = this.Data_Length_Lo;

            Buffer.BlockCopy(this.Payload, 0, payload, offset, PayloadLength);
            Packet packet = new Packet(payload, this.SeqID);
            return packet;
        }

        public byte[] ToMsg()
        {
            byte[] payload = new byte[6 + PayloadLength];
            int offset = 0;
            payload[offset++] = this.Trans_Idt_Hi;
            payload[offset++] = this.Trans_Idt_Lo;
            payload[offset++] = this.Protocol_Idt_Hi;
            payload[offset++] = this.Protocol_Idt_Lo;
            payload[offset++] = this.Data_Length_Hi;
            payload[offset++] = this.Data_Length_Lo;

            Buffer.BlockCopy(this.Payload, 0, payload, offset, PayloadLength);
            return payload;
        }

        #region Static Method

        #endregion
    }
}
