using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Utils
{
    public sealed class BinaryUtil
    {
        /// <summary>
        /// 把字节数组转换成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteToHex(byte[] data, int begin = 0, int count = 0xFFFF)
        {
            if (data == null || data.Length == 0) return "";

#if true
            StringBuilder str = new StringBuilder();
            int index = 0, end = begin + count;
            foreach(byte b in data)
            {
                if (index++ < begin)
                    continue;

                if (index > end)
                    break;

                str.Append(b.ToString("X2")).Append(" ");
            }

            return str.ToString();
#else
            string[] hexs = Array.ConvertAll<byte, string>(data, (b) => b.ToString("X2") );
            Array.Resize<string>()
            return String.Join(" ", hexs);
#endif
        }

        /// <summary>
        /// 把字节数组转换成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string WordToHex(ushort[] data, int begin = 0, int end = 0xFFFF)
        {
            if (data == null || data.Length == 0) return "";
#if true
            StringBuilder str = new StringBuilder();
            int index = 0;
            foreach (ushort b in data)
            {
                if (index++ < begin)
                    continue;

                if (index > end)
                    break;

                str.Append(b.ToString("X4")).Append(" ");
            }

            return str.ToString();
#else
            string[] hexs = Array.ConvertAll<ushort, string>(data, (b) => b.ToString("X4"));
            return String.Join(" ", hexs);
#endif
        }

        /// <summary>
        /// 把字符串数组转换成字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToByte(string hexString)
        {
            string[] hexSplit = hexString.Split(' ');
            return Array.ConvertAll<string, byte>(hexSplit, (s) => byte.Parse(s, NumberStyles.HexNumber));
        }

        /// <summary>
        /// 截取字节
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startindex"></param>
        /// <param name="length"></param>
        /// <param name="isreverseorder"></param>
        /// <returns></returns>
        public static byte[] SubBytes(byte[] data, int startindex, int length, bool isreverseorder)
        {
            byte[] tmp = new byte[length];
            Array.Copy(data, startindex, tmp, 0, tmp.Length);
            if (isreverseorder) tmp = Reverse(tmp);
            return tmp;
        }

        /// <summary>
        /// 翻转字节
        /// </summary>
        /// <param name="reverse"></param>
        /// <returns></returns>
        private static byte[] Reverse(byte[] reverse)
        {
            Array.Reverse(reverse);
            return reverse;
        }

        /// <summary>
        /// 数字转换成十六进制的BCD
        /// </summary>
        /// <param name="dig"></param>
        /// <returns></returns>
        public static byte Dig2HexBCD(int ivalue)
        {
            //Debug.Assert (iDigNumber>=0 && iDigNumber<=8);
            int dwRet = 0;
            int dwTmp = 0;
            for (int i = 0; i < 8; i++)
            {
                dwTmp = ivalue % 10;
                ivalue /= 10;
                dwTmp <<= i * 4;
                dwRet |= dwTmp;
            }
            return (byte)dwRet;
        }

        /// <summary>
        /// 十六进制BCD转换成数字
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int HexBCD2Dig(byte hex)
        {
            return (int)Convert.ToByte(Convert.ToString(hex, 16));
        }

        /// <summary>
        /// 转成二进制字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ByteToBinary(byte b, int length = 8)
        {
            return System.Convert.ToString(b, 2).PadLeft(length, '0');
        }

        /// <summary>
        /// 转成二进制字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string WordToBinary(ushort b, int length = 16)
        {
            return System.Convert.ToString(b, 2).PadLeft(length, '0');
        }

        /// <summary>
        /// 转成二进制字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string IntToBinary(int b, int length = 32)
        {
            return System.Convert.ToString(b, 2).PadLeft(length, '0');
        }
    }
}
