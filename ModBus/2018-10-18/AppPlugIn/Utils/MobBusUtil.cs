using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Ksat.AppPlugIn.Utils
{
    class MobBusUtil
    {
        public static byte[] GetByteArr(string strs, int len)
        {
            string[] splitStr = SplitByLen(strs, 8); // 分割字符串
            string[] fillStr = FillZero(splitStr, 8);
            string[] reverseStr = ReverseArr(fillStr);
            List<string> result = Binary2Hex(reverseStr);
            byte[] newByteArr = Array.ConvertAll<string, byte>(result.ToArray(), (s) => byte.Parse(s, NumberStyles.HexNumber));
            return newByteArr;
        }

        public static List<string> Binary2Hex(string[] strArr)
        {
            List<string> result = new List<string>();
            for (int j = 0; j < strArr.Length; j++)
            {
                string newstr = string.Format("{0:X2}", Convert.ToInt32(strArr[j], 2));
                result.Add(newstr);

            }
            return result;
        }

        /// <summary>
        /// 按照长度拆分字符串
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static String[] SplitByLen(string strs, int len)
        {
            double i = strs.Length;
            string[] myarray = new string[int.Parse(Math.Ceiling(i / len).ToString())];
            for (int j = 0; j < myarray.Length; j++)
            {
                len = len <= strs.Length ? len : strs.Length;
                myarray[j] = strs.Substring(0, len);
                strs = strs.Substring(len, strs.Length - len);
            }
            return myarray;
        }


        public static String[] FillZero(string[] strs, int length)
        {
            string[] newArr = new string[strs.Length];
            for (int j = 0; j < strs.Length; j++)
            {
                int len = strs[j].Length;
                if (len < length)
                {
                    StringBuilder newStr = new StringBuilder();
                    newStr.Append(strs[j]);
                    int delat = length - len;
                    for (int k = 0; k < delat; k++)
                    {
                        newStr.Append('0');
                    }
                    newArr[j] = newStr.ToString();
                }
                else
                    newArr[j] = strs[j];
            }
            return newArr;
        }

        public static string[] ReverseArr(string[] strArr)
        {
            string[] newArr = new string[strArr.Length];
            for (int j = 0; j < strArr.Length; j++)
            {
                newArr[j] = Reverse(strArr[j]);
            }
            return newArr;
        }

        public static string Reverse(string str)
        {
            if (String.IsNullOrEmpty(str))
                throw new ArgumentException("字符串为空！");
            StringBuilder sb = new StringBuilder(str.Length);
            for (int i = str.Length - 1; i >= 0; i--)
            {
                sb.Append(str[i]);
            }
            return sb.ToString();
        }
    }
}
