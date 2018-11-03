using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging
{
    /// <summary>
    /// Fins Tcp 格式
    ///                                 
    /// Flag        Length      Command     ErrorCode   ICF RSV GCT DNA DA1 DA2 SNA SA1 SA2 SID MRC SRC     data
    /// 46494E53    0000000C    00000002    00000000    80  00  02  00  02  00  00  36  00  FF  01  02
    /// Length : 从length后面的所有字符的字节数
    /// 1）80：ICF段，80标识要求有回复，81标识不要求有回复；
    /// 2）00：RSV段，默认为00；
    /// 3）02：GCT段，表示穿过的网络层数量，0层为02,1层为01，2层为00；
    /// 4）00：DNA段，目的网络地址；
    /// 5）18：DA1段，目的节点地址，默认是目的PLC的ip地址的最后位（上述PLC的ip地址为192.1.1.24，因此该段为0x18）；
    /// 6）00：DA2段，目的单元地址；
    /// 7）00：SNA段，源网络地址；
    /// 8）30：SA1段，源节点地址，即上位机ip地址的最后位（上述上位机的ip地址为192.1.1.48，因此该段为0x30）；
    /// 9）00：SA2段，源单元地址；
    /// 10）00：SID段；
    /// 11）0101：读写具体命令，0101表示读，0102表示写；
    /// 12）B1：相应区域和具体方式，B1表示WR区按字，B0表示CIO区按字，30   表示CIO区按位；
    /// 13）0064：寄存器地址；
    /// 14）00：位地址，即读取数据的首地址；（原本为000000，在读的时候仅前两个字节有效，因此为00）
    /// 15）0001：读取的数量。
    /// </summary>
    public abstract class AbstractFinsTcpMessage : AbstractMessageBase
    {
        public static readonly byte[] HEADER_FLAG = new byte[] { 0x46, 0x49, 0x4E, 0x53 };

        /// <summary>
        /// 命令类型
        /// </summary>
        public enum CommandCode : int
        {
            /// <summary>
            /// 请求握手
            /// </summary>
            HandShake_Req = 0,

            /// <summary>
            /// 握手回复
            /// </summary>
            HandShake_Resp = 0x01,

            /// <summary>
            /// 数据交互
            /// </summary>
            DataFrame = 0x02,
        }

        public enum ERROR_CODE : int
        {
            Success = 0,

            /// <summary>
            /// the header is not 'FINS'.
            /// </summary>
            HeaderError = 1,

            /// <summary>
            /// the data length is too long
            /// </summary>
            DataLengthError = 2,

            /// <summary>
            /// the command is not support
            /// </summary>
            CommandError = 3,

            /// <summary>
            /// all connections are in use 
            /// </summary>
            ConnectUnderUse = 0x20,

            /// <summary>
            /// the specified node is already connected
            /// </summary>
            RemoteConnected = 0x21,

            /// <summary>
            /// attempt to access a protected node from an unspecified ip address
            /// </summary>
            RemoteNotAllow = 0x22,

            /// <summary>
            /// the client fins node address is out of range 
            /// </summary>
            IpOutRange = 0x23,


        }


        ///// <summary>
        ///// 数据长度
        ///// </summary>
        //public UInt32 DataLength { get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public CommandCode Command { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public UInt32 ErrorCode { get; set; }


        public void CopyFrom(AbstractFinsTcpMessage other)
        {
            base.CopyFrom(other);

            this.Command = other.Command;
            this.ErrorCode = other.ErrorCode;
        }

        public AbstractFinsTcpMessage(AbstractFinsTcpMessage other) : base(other.SeqID)
        {
            this.CopyFrom(other);
        }


        public AbstractFinsTcpMessage(CommandCode cmd, int seqID = 0) : base(seqID)
        {
            this.Command = cmd;
        }

        public bool IsSuccess()
        {
            if (this.ErrorCode == (int)ERROR_CODE.Success)
                return true;

            return false;
        }

        public static AbstractFinsTcpMessage TryParse(byte[] buffer, int offset, int length, out int readlength)
        {
            readlength = 0;

            int data_length = Utils.NetworkBitConverter.ToInt32(buffer, offset);
            offset += 4;

            if (data_length <= 0 || length < (data_length  + 4))
            {
                Logging.Logger.Warn("AbstractFinsTcpMessage", 
                    "not engouh data count for buffer length:"+length+ " <  (msg_data_length  + 4):" + (data_length + 4)
                    + ", offset:"+ offset
                    + ", Fins buffer:"+ Utils.BinaryUtil.ByteToHex(buffer, offset-4, length));

                return null;
            }

            int cmd = Utils.NetworkBitConverter.ToInt32(buffer, offset);
            offset += 4;

            int errcode = Utils.NetworkBitConverter.ToInt32(buffer, offset);
            offset += 4;

            AbstractFinsTcpMessage msg = null;

            try
            {
                if (cmd == (int)CommandCode.DataFrame)
                {
                    msg = new Fins.FinsTcpDataFrameMessage(buffer, offset, data_length - 8);
                }
                else if (cmd == (int)CommandCode.HandShake_Req)
                {
                    msg = new Fins.FinsTcpHandShakeMessage(CommandCode.HandShake_Req, buffer, offset, data_length - 8);
                }
                else if (cmd == (int)CommandCode.HandShake_Resp)
                {
                    msg = new Fins.FinsTcpHandShakeMessage(CommandCode.HandShake_Resp, buffer, offset, data_length - 8);
                }

                if(msg != null)
                {
                    msg.Command = (CommandCode)cmd;
                    msg.ErrorCode = (UInt32)errcode;

                    readlength = 4 + data_length;
                }
            }
            catch
            {
                msg = null;
            }
            

            return msg;
        }

        public int FixHeaderFlag(byte[] buffer, int offset, int length)
        {
            Buffer.BlockCopy(HEADER_FLAG, 0, buffer, offset, 4);

            //offset += 4;

            //Buffer.BlockCopy(HEADER_FLAG, 0, buffer, offset, 4);

            return 4;
        }

        public int FixHeader(byte[] buffer, int datalength)
        {
            int offset = 0;
            Buffer.BlockCopy(HEADER_FLAG, 0, buffer, offset, 4);
            offset += 4;

            /// fill data length
            Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(datalength), 0, buffer, offset, 4);
            offset += 4;

            /// fill command
            int cmd = (int)this.Command;
            Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(cmd), 0, buffer, offset, 4);
            offset += 4;

            /// fill error code
            Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(this.ErrorCode), 0, buffer, offset, 4);
            offset += 4;
            

            return offset;
        }

        public void FixDataLength(byte[] buffer, int datalength)
        {
            /// fill data length
            Buffer.BlockCopy(Utils.NetworkBitConverter.GetBytes(datalength), 0, buffer, 4, 4);
        }
    }
}
