using Ksat.AppPlugIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc.Com
{
    public class ComUtils
    {
        private static readonly string WinComPrefix = "COM";
        private static readonly string LinuxUsbComPrefix = "/dev/ttyUSB";
        private static readonly string LinuxSysComPrefix = "/dev/ttyS";

        /// <summary>
        /// 标识linux下串口类型
        /// </summary>
        public enum LinuxComType
        {
            /// <summary>
            /// 标识串口为USB类型
            /// </summary>
            Usb,
            /// <summary>
            /// 标识串口为System系统类型
            /// </summary>
            System
        }

        /// <summary>
        /// 配置Linux串口信息
        /// </summary>
        public class LinuxCom
        {
            /// <summary>
            /// Linux下串口号
            /// </summary>
            public int LinuxPort { set; get; }

            /// <summary>
            /// Linux下串口类型
            /// </summary>
            public LinuxComType LinuxComType { set; get; }
        }

        public static string PortToString(int port)
        {
            string prefix = String.Empty;
            OperatingSystemType plat = Common.OperatingSystem.GetOperatingSystemType();
            if (plat == OperatingSystemType.Windows)
            {
                prefix = WinComPrefix;
            }
            else if (plat == OperatingSystemType.Linux)
            {
                prefix = LinuxSysComPrefix;
#if false

                LinuxCom linuxCom = GlobalConfigTool.GlobalConfig.LinuxComList.FirstOrDefault(l => l.LinuxPort == port);
                if (linuxCom != null)
                {
                    if (linuxCom.LinuxComType == LinuxComType.Usb)
                    {
                        prefix = LinuxUsbComPrefix;
                    }
                    else if (linuxCom.LinuxComType == LinuxComType.System)
                    {
                        prefix = LinuxSysComPrefix;
                    }

                }
#endif
            }
            return String.Format("{0}{1}", prefix, port.ToString());
        }

        public static int PortToInt(string portString)
        {
            string prefix = String.Empty;
            OperatingSystemType plat = Common.OperatingSystem.GetOperatingSystemType();
            if (plat == OperatingSystemType.Windows)
            {
                prefix = WinComPrefix;
            }
            else if (plat == OperatingSystemType.Linux)
            {
                prefix = LinuxSysComPrefix;
#if false
            LinuxCom linuxCom = GlobalConfigTool.GlobalConfig.LinuxComList.FirstOrDefault(l => portString.Contains(l.LinuxPort.ToString()));
                if (linuxCom != null)
                {
                    if (linuxCom.LinuxComType == LinuxComType.Usb)
                    {
                        prefix = LinuxUsbComPrefix;
                    }
                    else if (linuxCom.LinuxComType == LinuxComType.System)
                    {
                        prefix = LinuxSysComPrefix;
                    }
                }
#endif
            }

            if (prefix.Length > 0)
            {
                return int.Parse(portString.Substring(prefix.Length));
            }
            else
            {
                throw new IndexOutOfRangeException("串口字符串无法转换");
            }
        }
    }
}
