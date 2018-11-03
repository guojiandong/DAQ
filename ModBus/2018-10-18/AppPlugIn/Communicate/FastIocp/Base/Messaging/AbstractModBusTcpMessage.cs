using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging;
using Ksat.AppPlugIn.Utils;

namespace Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging
{
    public abstract class AbstractModBusTcpMessage : AbstractMessageBase
    {
        public static readonly byte[] HEADER_FLAG = ModBusTcpHelper.HEADER_FLAG;

        public enum ERROR_CODE : int
        {
            Success = 0,

            /// <summary>
            /// 非法的功能.
            /// </summary>
            IllegalFunc = 1,

            /// <summary>
            /// 非法的数据地址
            /// </summary>
             ILLegalDataAddress= 2,

            /// <summary>
            /// 非法的数据值
            /// </summary>
            ILLegalDataValue = 3,

            /// <summary>
            /// 非法的响应长度
            /// </summary>
            ILLegalDataLength = 4,

            /// <summary>
            /// 确认
            /// </summary>
            Confirm = 5,

            /// <summary>
            /// 从站设备忙
            /// </summary>
            slaveEquipBusy = 6,

            /// <summary>
            /// 存储器奇偶校验错误
            /// </summary>
            OddCheckError = 8,

            /// <summary>
            /// 网管通路不可用
            /// </summary>
            UnReached = 10,

            /// <summary>
            /// 网关目标设备响应失败
            /// </summary>
            TargetResponseFailed = 11,

        }
        public byte ErrorCode { get; set; } = 0;

        public void CopyFrom(AbstractModBusTcpMessage other)
        {
            base.CopyFrom(other);
        }

        public AbstractModBusTcpMessage(AbstractModBusTcpMessage other) : base(other.SeqID)
        {
            this.CopyFrom(other);
        }

        public AbstractModBusTcpMessage(int seqID = 0) : base(seqID)
        {
        }

        public bool IsSuccess()
        {
            if (this.ErrorCode == (int)ERROR_CODE.Success)
                return true;


            return false;
        }

        public static AbstractModBusTcpMessage TryParse(byte[] buffer, int offset, int length, out int readlength)
        {
            AbstractModBusTcpMessage msg = null;
            readlength = 0;
            int oldOffset = offset;
            offset += 3; // jump the 报文长度 2-byte  单元标识 1-byte

            int bit8 = (buffer[offset] & 128) == 128 ? 1:0;  // offset = 7 releative
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
                catch
                {
                    msg = null;
                }
            }
            return msg;
        }
    }
}
