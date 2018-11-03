using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging.Fins
{
    /// <summary>
    /// 交互数据消息
    /// </summary>
    public class FinsTcpDataFrameMessage : AbstractFinsTcpMessage
    {
        public FinsTcpDataFrameMessage(int seqID = 0) : base(CommandCode.DataFrame, seqID)
        {
            this.ICF = 0x81;
            this.RSV = 0;
            this.GCT = 0x02;

            this.DNA = 0x00;
            this.DA1 = 0x00;
            this.DA2 = 0x00;

            this.SNA = 0x00;
            this.SA1 = 0x00;
            this.SA2 = 0x00;

            this.SID = 0xFF;
            this.MRC = 0x01;
            this.SRC = 0x02;

            this.BitState = 0x82;

            this.RegisterAddress = 0x0000;
            this.RegisterOffset = 0x00;

            this.PayloadLength = 0x0000;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("FinsDataFrame").Append(",");
            str.Append(this.Command).Append(",");
            str.AppendFormat("Err:{0:X8}", this.ErrorCode).Append(",");

            str.AppendFormat("{0:X2} {1:X2} {2:X2}", this.ICF, this.RSV, this.GCT).Append(",");
            str.AppendFormat("{0:X2} {1:X2} {2:X2}", this.DNA, this.DA1, this.DA2).Append(",");
            str.AppendFormat("{0:X2} {1:X2} {2:X2}", this.SNA, this.SA1, this.SA2).Append(",");
            str.AppendFormat("SID:{0:X2} ", this.SID ).Append(",");
            str.AppendFormat("RC:{0:X2} {1:X2}", this.MRC, this.SRC ).Append(",");

            str.AppendFormat("BitState:{0:X2}", this.BitState).Append(",");

            str.AppendFormat("Addr:{0:X4}", this.RegisterAddress).Append(",");

            str.AppendFormat("{0:X2} ", this.RegisterOffset).Append(",");

            str.AppendFormat("PayloadLength:{0:X4}", this.PayloadLength).Append(",");

            if (this.Payload != null)
            {
                str.AppendFormat("Payload.Length:{0:X4}", this.Payload.Length).Append(", Payload:");

                for (int i=0; i<Math.Min(32, this.Payload.Length); i++)
                {
                    if (i % 4 == 0)
                        str.Append(" ");

                    str.AppendFormat("{0:X2}", this.Payload[i]);
                }
            }
            else
            {
                str.Append("PayLoad is null");
            }
            
            return str.ToString();
        }

        public override AbstractMessageBase Clone()
        {
            return new FinsTcpDataFrameMessage(this);
        }

        public FinsTcpDataFrameMessage(FinsTcpDataFrameMessage other) : base(CommandCode.DataFrame, other.SeqID)
        {
            this.CopyFrom(other);
        }

        public FinsTcpDataFrameMessage(byte[] buffer, int offset, int count) : base(CommandCode.DataFrame, 0)
        {
            int old_offset = offset;
            this.ICF = buffer[offset++];
            this.RSV = buffer[offset++];
            this.GCT = buffer[offset++];

            this.DNA = buffer[offset++];
            this.DA1 = buffer[offset++];
            this.DA2 = buffer[offset++];

            this.SNA = buffer[offset++];
            this.SA1 = buffer[offset++];
            this.SA2 = buffer[offset++];

            this.SID = buffer[offset++];
            this.MRC = buffer[offset++];
            this.SRC = buffer[offset++];

            this.BitState = buffer[offset++];

            if (!IsResponse())
            {
                this.RegisterAddress = (UInt16)Utils.NetworkBitConverter.ToInt16(buffer, offset);
                offset += 2;
            }
            
            this.RegisterOffset = buffer[offset++];

            int data_length = 0;
            if (IsResponse())
            {
                data_length = count - (offset - old_offset);
            }
            else
            {
                data_length = Utils.NetworkBitConverter.ToInt16(buffer, offset) * 2;
                offset += 2;
            }

            this.PayloadLength = (UInt16)(data_length / 2);

            if (data_length > 0)
            {
                this.Payload = new byte[data_length];
                Buffer.BlockCopy(buffer, offset, this.Payload, 0, data_length);
            }
        }

        public void CopyFromWithOutPayload(FinsTcpDataFrameMessage other)
        {
            base.CopyFrom(other);

            this.ICF = other.ICF;
            this.RSV = other.RSV;
            this.GCT = other.GCT;

            this.DNA = other.DNA;
            this.DA1 = other.DA1;
            this.DA2 = other.DA2;

            this.SNA = other.SNA;
            this.SA1 = other.SA1;
            this.SA2 = other.SA2;

            this.SID = other.SID;
            this.MRC = other.MRC;
            this.SRC = other.SRC;

            this.BitState = other.BitState;

            this.RegisterAddress = other.RegisterAddress;
            this.RegisterOffset = other.RegisterOffset;

            this.PayloadLength = 0x0000;
        }

        public void CopyFrom(FinsTcpDataFrameMessage other)
        {
            base.CopyFrom(other);

            this.ICF = other.ICF;
            this.RSV = other.RSV;
            this.GCT = other.GCT;

            this.DNA = other.DNA;
            this.DA1 = other.DA1;
            this.DA2 = other.DA2;

            this.SNA = other.SNA;
            this.SA1 = other.SA1;
            this.SA2 = other.SA2;

            this.SID = other.SID;
            this.MRC = other.MRC;
            this.SRC = other.SRC;

            this.BitState = other.BitState;

            this.RegisterAddress = other.RegisterAddress;
            this.RegisterOffset = other.RegisterOffset;

            this.PayloadLength = other.PayloadLength;

            if (other.Payload != null)
            {
                this.Payload = new byte[other.Payload.Length];
                Buffer.BlockCopy(other.Payload, 0, this.Payload, 0, this.Payload.Length);
            }
        }
        

        public override Packet ToPacket()
        {
            int datalength = 8 + 12 + 2;
            if (!IsResponse())
            {
                datalength += 2 + 2;
            }

            if (this.Payload != null)
                datalength += (this.Payload.Length + (this.Payload.Length%2));


            byte[] payload = new byte[datalength + 8];

            int offset = FixHeader(payload, datalength);
            payload[offset++] = ICF;

            payload[offset++] = RSV;

            payload[offset++] = GCT;

            payload[offset++] = DNA;

            payload[offset++] = DA1;

            payload[offset++] = DA2;

            payload[offset++] = SNA;

            payload[offset++] = SA1;

            payload[offset++] = SA2;

            payload[offset++] = SID;

            payload[offset++] = MRC;

            payload[offset++] = SRC;

            payload[offset++] = BitState;

            if (!IsResponse())
            {
                Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(this.RegisterAddress), 0, payload, offset, 2);
                offset += 2;
            }

            payload[offset++] = this.RegisterOffset;
            
            if (this.Payload != null && this.Payload.Length > 0)
            {
                int payload_size = this.Payload.Length / 2;
                //if ((this.Payload.Length % 2) > 0)
                //    payload_size++;

                if (!IsResponse())
                {
                    Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes((short)payload_size), 0, payload, offset, 2);
                    offset += 2;
                }

                Buffer.BlockCopy(this.Payload, 0, payload, offset, this.Payload.Length);
                offset += this.Payload.Length;
            }
            else if(!IsResponse())
            {
                Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes((short)this.PayloadLength), 0, payload, offset, 2);
                offset += 2;
            }

            Base.Packet pkg = new Base.Packet(payload, this.SeqID);
            pkg.Tag = this.GetType().Name;

            return pkg;
            //return new Base.Packet(payload, SeqID);
        }

        /// <summary>
        /// 80：ICF段，0x80标识要求有回复，0x81标识不要求有回复；0xC0标识是回复信息
        /// </summary>
        public byte ICF { get; set; }

        public bool IsMustReplay()
        {
            if (this.ICF == 0x80)
                return true;

            return false;
        }

        public FinsTcpDataFrameMessage RequestReplay(bool flag = false)
        {
            this.ICF = (byte)(flag ? 0x80 : 0x81);

            return this;
        }

        public FinsTcpDataFrameMessage SetAsResponse()
        {
            this.ICF = (byte)0xC0;
            return this;
        }

        /// <summary>
        /// 00：RSV段，默认为00；
        /// </summary>
        public byte RSV { get; set; }

        /// <summary>
        /// 02：GCT段，表示穿过的网络层数量，0层为02,1层为01，2层为00；
        /// </summary>
        public byte GCT { get; set; }

        /// <summary>
        /// 0层 ,1层 ，2层 
        /// </summary>
        /// <param name="level">0 ~ 2</param>
        /// <returns></returns>
        public FinsTcpDataFrameMessage SetNetworkLevel(int level = 2)
        {
            if (level == 1)
                this.GCT = 0x01;
            else if (level == 0)
                this.GCT = 0x02;
            else
                this.GCT = 0x00;
            return this;
        }

        /// <summary>
        /// 00：DNA段，目的网络地址；
        /// </summary>
        public byte DNA { get; set; }

        /// <summary>
        /// 18：DA1段，目的节点地址，默认是目的PLC的ip地址的最后位（上述PLC的ip地址为192.1.1.24，因此该段为18）；
        /// </summary>
        public byte DA1 { get; set; }

        public FinsTcpDataFrameMessage SetRemoteAddress(string ipaddress)
        {
            if (String.IsNullOrEmpty(ipaddress)) throw new ArgumentNullException("ipaddress can't be null.");

            //System.Net.IPAddress ip = Utils.NetUtils.GetIpAddress(ipaddress);
            //if (ip == null)
            //{
            //    throw new ArgumentException("parse remote ip address:" + ipaddress + " failed.");
            //}

            int address = System.Net.IPAddress.HostToNetworkOrder((int)Utils.IPUtility.ConvertToNumber(ipaddress));

            this.DA1 = Utils.NetworkBitConverter.GetBytes(address)[0];

            return this;
        }

        public FinsTcpDataFrameMessage SetRemoteAddress(IPEndPoint ipaddress)
        {
            if (ipaddress == null) throw new ArgumentNullException("ipaddress can't be null.");
            
            return this.SetRemoteAddress(ipaddress.Address);
        }

        public FinsTcpDataFrameMessage SetRemoteAddress(IPAddress ipaddress)
        {
            if (ipaddress == null) throw new ArgumentNullException("ipaddress can't be null.");

            int address = System.Net.IPAddress.HostToNetworkOrder((int)Utils.IPUtility.ConvertToNumber(ipaddress));
            this.DA1 = Utils.NetworkBitConverter.GetBytes(address)[0];

            return this;
        }

        /// <summary>
        /// 00：DA2段，目的单元地址；
        /// </summary>
        public byte DA2 { get; set; }

        /// <summary>
        /// 00：SNA段，源网络地址；
        /// </summary>
        public byte SNA { get; set; }

        /// <summary>
        /// 30：SA1段，源节点地址，即上位机ip地址的最后位（上述上位机的ip地址为192.1.1.48，因此该段为30）；
        /// </summary>
        public byte SA1 { get; set; }

        public FinsTcpDataFrameMessage SetLocalAddress(string ipaddress)
        {
            if (String.IsNullOrEmpty(ipaddress)) throw new ArgumentNullException("ipaddress can't be null.");

            //System.Net.IPAddress ip = Utils.NetUtils.GetIpAddress(ipaddress);
            //if (ip == null)
            //{
            //    throw new ArgumentException("parse remote ip address:" + ipaddress + " failed.");
            //}

            int address = System.Net.IPAddress.HostToNetworkOrder((int)Utils.IPUtility.ConvertToNumber(ipaddress));

            this.SA1 = Utils.NetworkBitConverter.GetBytes(address)[0];

            return this;
        }

        public FinsTcpDataFrameMessage SetLocalAddress(IPEndPoint ipaddress)
        {
            if (ipaddress == null) throw new ArgumentNullException("ipaddress can't be null.");

            return this.SetLocalAddress(ipaddress.Address);
        }

        public FinsTcpDataFrameMessage SetLocalAddress(IPAddress ipaddress)
        {
            if (ipaddress == null) throw new ArgumentNullException("ipaddress can't be null.");

            int address = System.Net.IPAddress.HostToNetworkOrder((int)Utils.IPUtility.ConvertToNumber(ipaddress));
            this.SA1 = Utils.NetworkBitConverter.GetBytes(address)[0];

            return this;
        }

        /// <summary>
        /// 00：SA2段，源单元地址；
        /// </summary>
        public byte SA2 { get; set; }

        /// <summary>
        /// 00：SID段；
        /// </summary>
        public byte SID { get; set; }

        /// <summary>
        /// 0101：读写具体命令，0101表示读，0102表示写；
        /// </summary>
        public byte MRC { get; set; }

        /// <summary>
        /// 0101：读写具体命令，0101表示读，0102表示写；
        /// </summary>
        public byte SRC { get; set; }

        /// <summary>
        /// read mode or write mode
        /// </summary>
        /// <param name="write"></param>
        /// <returns></returns>
        public FinsTcpDataFrameMessage SetWriteAble(bool write = false)
        {
            this.MRC = (byte)0x01;
            if (write)
            {
                this.SRC = (byte)0x02;
            }
            else
            {
                this.SRC = (byte)0x01;
            }

            return this;
        }

        public bool IsWriteAble()
        {
            if (this.SRC == 0x02)
                return true;

            return false;
        }

        /// <summary>
        /// 12）B1：相应区域和具体方式，B1表示WR区按字，B0表示CIO区按字，30   表示CIO区按位；
        /// 82 表示D区
        /// </summary>
        public byte BitState { get; set; }

        /// <summary>
        /// 13）0064：寄存器地址；
        /// </summary>
        public UInt16 RegisterAddress { get; set; }


        /// <summary>
        /// 14）00：位地址，即读取数据的首地址；（原本为000000，在读的时候仅前两个字节有效，因此为00）
        /// </summary>
        public byte RegisterOffset { get; set; }


        /// <summary>
        /// 15）0001：读取的数量。Word为单位
        /// </summary>
        public UInt16 PayloadLength { get; set; }

        /// <summary>
        /// payload
        /// </summary>
        public byte[] Payload { get; private set; }

        public FinsTcpDataFrameMessage SetPayload(byte[] payload)
        {
            if (payload == null) throw new ArgumentNullException("payload");

            return this.SetPayload(payload, 0, payload.Length);
        }


        public FinsTcpDataFrameMessage SetPayload(byte[] payload, int offset, int count)
        {
            if (payload == null || count == 0) throw new ArgumentNullException("payload");

            int loaclcount = count;
            if (count % 2 > 0)
                loaclcount++;

            this.PayloadLength = (UInt16)(loaclcount / 2);

            this.Payload = new byte[loaclcount];

            Buffer.BlockCopy(payload, offset, this.Payload, 0, count);

            return this;
        }


        public bool IsResponse()
        {
            if(this.ICF == 0xC0 && this.Command == CommandCode.DataFrame)
            {
                return true;
            } 

            return false;
        }

        /// <summary>
        /// 生成回复消息。
        /// </summary>
        /// <returns>The response.</returns>
        public FinsTcpDataFrameMessage ToResponse()
        {
            FinsTcpDataFrameMessage msg = (FinsTcpDataFrameMessage)this.Clone();
            msg.Payload = null;
            byte sa1 = msg.SA1;
            msg.SA1 = msg.DA1;
            msg.DA1 = sa1;
            msg.RegisterAddress = 0;
            msg.SetWriteAble(false);

            msg.SetAsResponse();

            return msg;
        }

        /// <summary>
        /// 切换源ip和目标ip
        /// </summary>
        /// <returns>The ip address.</returns>
        public FinsTcpDataFrameMessage SwitchIpAddress()
        {
            byte sa1 = this.SA1;
            this.SA1 = this.DA1;
            this.DA1 = sa1;

            return this;
        }
    }
}
