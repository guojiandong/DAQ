using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryMappEditor
{
    class ConstFile
    {
        //基础数据配置区
        public static int BASE_DATA_START = 8000;
        public static int BASE_DATA_LENGTH = 300;

        //PLC事件数据区
        public static int PLC_EVENT_DATA_START = 10000;
        public static int PLC_EVENT_DATA_LENGTH = 2000;

        //EAP回复信息存储区
        public static int EAP_EVENT_DATA_START = 20000;
        public static int EAP_EVENT_DATA_LENGTH = 2000;

        public static string Flag = "D0";

        public static int Register_DataSize = 1;
        public static int EapCommand_DataSize = 1;
        public static int AlarmText_DataSize = 30;
        public static int MotorSetting_DataSize = 300;
        public static int NormalSettings_DataSize = 100;
        public static int HeartBeat_DataSize = 4;
        public static int EventChangedFlag_DataSize = 4;
        public static int BreakPoint_DataSize = 20;
        public static int AlarmCode_DataSize = 20;
        public static int MachineStatus_DataSize = 1;
        public static int CheckIn_DataSize = 1;
        public static int CheckOut_DataSize = 1;
    }
}
