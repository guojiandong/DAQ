using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Utils;
using Ksat.AppPlugIn.Communicate.FastIocp.Base.Messaging;

namespace Ksat.AppPlugIn.Utils
{
    class ModBusTcpHelper
    {
        public static readonly byte[] HEADER_FLAG = new byte[] { 0x90, 0x91, 0x00, 0x00 };
        #region Build Command

        /// <summary>
        /// 生成读取线圈的指令 0x01
        /// </summary>
        /// <param name="address">地址 "01 02" </param>
        /// <param name="length">长度 "00 02"</param>
        public static byte[] BuildReadCoilCommand(string hex_address, string hex_length)
        {
            byte[] addressByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_address);
            byte addr_Start_Hi = addressByte[0];
            byte addr_Start_Lo = addressByte[1];

            byte[] lengthByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_length);
            byte length_Hi = lengthByte[0];
            byte length_Lo = lengthByte[1];

            // 生成最终tcp byte[]
            byte[] buffer = new byte[] { 0x90, 0x91, 0x00, 0x00, 0x00, 0x06, 0xff, 0x01, addr_Start_Hi, addr_Start_Lo, length_Hi, length_Lo };
            return buffer;
        }


        /// <summary>
        /// 生成读取离散信息的指令 0x02
        /// </summary>
        /// <param name="address">地址"01 02"</param>
        /// <param name="length">长度"00 02"</param>
        public static byte[] BuildReadDiscreteCommand(string hex_address, string hex_length)
        {
            byte[] addressByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_address);
            byte addr_Start_Hi = addressByte[0];
            byte addr_Start_Lo = addressByte[1];

            byte[] lengthByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_length);
            byte length_Hi = lengthByte[0];
            byte length_Lo = lengthByte[1];

            // 生成最终tcp byte[]
            byte[] buffer = new byte[] { 0x90, 0x91, 0x00, 0x00, 0x00, 0x06, 0xff, 0x02, addr_Start_Hi, addr_Start_Lo, 0x00, 0x01 };
            return buffer;
        }

        /// <summary>
        /// 生成读取寄存器的指令 0x03
        /// </summary>
        /// <param name="address">地址"01 02"</param>
        /// <param name="length">长度"00 02"</param>
        public static byte[] BuildReadRegisterCommand(string hex_address, string hex_length)
        {
            byte[] addressByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_address);
            byte addr_Start_Hi = addressByte[0];
            byte addr_Start_Lo = addressByte[1];

            byte[] lengthByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_length);
            byte length_Hi = lengthByte[0];
            byte length_Lo = lengthByte[1];

            // 生成最终tcp byte[]
            // 00 00 00 00 00 06 FF 03 00 00 00 01      功能码03，读取寄存器，地址address ，长度length
            byte[] buffer = new byte[] { 0x90, 0x91, 0x00, 0x00, 0x00, 0x06, 0xff, 0x03, addr_Start_Hi, addr_Start_Lo, length_Hi, length_Lo };
            return buffer;
        }


        /// <summary>
        /// 生成写入单线圈的指令 0x05
        /// </summary>
        /// <param name="address">地址"01 02"</param>
        /// <param name="isOn"></param>
        public static byte[] BuildWriteOneCoilCommand(string hex_address, bool isOn)
        {
            byte[] addressByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_address);
            byte addr_Start_Hi = addressByte[0];
            byte addr_Start_Lo = addressByte[1];

            byte state_Hi;
            byte state_Lo;
            if (isOn)
            {
                state_Hi = 0xff;
                state_Lo = 0x00;
            }
            else
            {
                state_Hi = 0x00;
                state_Lo = 0x00;
            }
            // 生成最终tcp byte[]
           // 00 00 00 00 00 06 FF 05 00 00 FF 00   前面的含义都是一致的，我们就分析 05 00 00 FF 00
           //05 是功能码， 00 00 是我们指定的地址，如果我们想写地址1000为通，那么就为 03 E8，至于FF 00是规定的数据，如果你想地址线圈通，就填这个值，想指定线圈为断，就填 00 00 ，其他任何的值都对结果无效。
            byte[] buffer = new byte[] { 0x90, 0x91, 0x00, 0x00, 0x00, 0x06, 0xff, 0x05, addr_Start_Hi, addr_Start_Lo, state_Hi, state_Lo };
            return buffer;
        }

        /// <summary>
        /// 生成写入多个线圈的指令 0x0f
        /// </summary>
        /// <param name="address">地址"00 02"</param>
        /// <param name="hex_length">需要写线圈的个数</param>
        /// <param name="bin_data">长度"1001010110011"</param>
        public static byte[] BuildWriteMultiCoilCommand(string hex_address, string coil_count_hex, string bin_data)
        {
            byte[] addressByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_address);
            byte addr_Start_Hi = addressByte[0];
            byte addr_Start_Lo = addressByte[1];

            byte[] lengthByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(coil_count_hex);
            byte length_Hi = lengthByte[0];
            byte length_Lo = lengthByte[1];

            int coil_count_length = Int32.Parse(coil_count_hex.Replace(" ", ""), System.Globalization.NumberStyles.HexNumber);
            int real_data_length = (int)Math.Ceiling((double)coil_count_length / 8);

            Int16 message_int_length = (Int16)(7 + real_data_length);
            byte[] message_byte = BitConverter.GetBytes(message_int_length);
            byte[] data = new byte[] { 0x90, 0x91, 0x00, 0x00, message_byte[1], message_byte[0], 0xff, 0x0f, addr_Start_Hi, addr_Start_Lo, length_Hi, length_Lo,(byte)real_data_length };

            byte[] binary_data = MobBusUtil.GetByteArr(bin_data,8);// 一个线圈 占一个byte
            byte[] buffer = new byte[12 + 1 +  real_data_length];
            Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
            Buffer.BlockCopy(binary_data, 0 , buffer, data.Length, binary_data.Length);
            return buffer;
        }


        /// <summary>
        /// 生成写入单个寄存器的报文 0x06
        /// </summary>
        /// <param name="address">地址"01 02"</param>
        /// <param name="values">内容"00 02"</param>
        public static byte[] BuildWriteOneRegisterCommand(string hex_address, byte[] values)
        {
            byte[] addressByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_address);
            byte addr_Start_Hi = addressByte[0];
            byte addr_Start_Lo = addressByte[1];
            byte[] data = new byte[] { 0x90, 0x91, 0x00, 0x00, 0x00, 0x06, 0xff, 0x06, addr_Start_Hi, addr_Start_Lo, values[1], values[0] };
            return data;
        }

        /// <summary>
        /// 生成批量写入寄存器的报文信息 0x10
        /// </summary>
        /// <param name="address">地址"00 02"</param>
        /// <param name="reigster_count">寄存器个数"00 02"</param> 在地址addrss处 写入hex_length个寄存器的值values
        public static byte[] BuildWriteMultiRegisterCommand(string hex_address, string reigster_count_hex, byte[] values)
        {
            byte[] addressByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(hex_address);
            byte addr_Start_Hi = addressByte[0];
            byte addr_Start_Lo = addressByte[1];

            byte[] registerCountByte = Ksat.AppPlugIn.Utils.BinaryUtil.HexToByte(reigster_count_hex);
            byte registerCount_Hi = registerCountByte[0];
            byte registerCount_Lo = registerCountByte[1];

            int register_count_length = Int32.Parse(reigster_count_hex.Replace(" ", ""), System.Globalization.NumberStyles.HexNumber); // 一个寄存器占两个byte
            Int16 message_int_length = (Int16)(7 + register_count_length * 2);
            byte[] message_byte = BitConverter.GetBytes(message_int_length);  // https://blog.csdn.net/xxgxgx/article/details/46755097

            byte[] buffer = new byte[12 + 1 + register_count_length * 2];
            //FixHeader(ref buffer);
            byte[] data = new byte[] { 0x90, 0x91, 0x00, 0x00, message_byte[1], message_byte[0], 0xff, 0x10, addr_Start_Hi, addr_Start_Lo, registerCount_Hi, registerCount_Lo,(byte)register_count_length };
            Buffer.BlockCopy(data, 4, buffer, 0, data.Length);
            Buffer.BlockCopy(values, 0, buffer, data.Length, values.Length);
            return buffer;
        }
        #endregion

        public static void FixHeader(ref byte[] buffer)
        {
            Buffer.BlockCopy(HEADER_FLAG, 0, buffer, 0, 4);
        }
    }
}
