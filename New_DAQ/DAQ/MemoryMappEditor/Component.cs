using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryMappEditor
{
    public enum EventCode
    {
        Rigseter = 0x2,
        EapCommand,
        AlarmText,
        MotorSettings,
        NormalSettings,
        HeartBeat,
        EventChangedFlag,
        BreakPoint,
        AlarmCode,
        MachineStatus,
        CheckIn,
        CheckOut
    }

    public class FixedEvent
    {
        private int version;
        public int Version                  //版本
        {
            get { return version; }
            set { version = value; }
        }

        private int eventcode;
        public int EventCode                //事件编码
        {
            get { return eventcode; }
            set { eventcode = value; }
        }

        private int datasize;
        public int DataSize                 //数据大小
        {
            get { return datasize; }
            set { datasize = value; }
        }

        private int stationId;
        public int StationID                //状态id
        {
            get { return stationId; }
            set { stationId = value; }
        }

        private int offset;               
        public int Offset                //偏移量 
        {
            get { return offset; }
            set { offset = value; }
        }              

        public FixedEvent(int _version, int _eventcode , int _datasize, int _stationId, int _offset)
        {
            Version = _version;
            EventCode = _eventcode;
            datasize = _datasize;
            StationID = _stationId;
            Offset = _offset;
        }
    }

    //马达轴参数
    public class MotorAxis
    {
        private string axisname;
        public string AxisName
        {
            get { return axisname; }
            set { axisname = value; }
        }     // 轴名称

        private int corrections;
        public int Corrections        // 修正位
        {
            get { return corrections; }
            set { corrections = value; }
        }   

        public int manualspeed;     
        public int ManualSpeed        // 手动速度
        {

            get { return manualspeed; }
            set { manualspeed = value; }
        }

        public int increment_offset;
        public int IncrementOffset    // 增量位移
        {

            get { return increment_offset; }
            set { increment_offset = value; }
        }

        public int increment_speed;
        public int IncrementSpeed     // 增量速度
        {

            get { return increment_speed; }
            set { increment_speed = value; }
        }

        public List<float> WorkingPositionSpeeds; // 工作位速度

        public MotorAxis(int _corrections, int _manualspeed, int _increment_offset, int _increment_speed, List<float> _WorkingPositionSpeeds)
        {
            Corrections = _corrections;
            ManualSpeed = _manualspeed;
            IncrementOffset = _increment_offset;
            IncrementSpeed = _increment_speed;
            WorkingPositionSpeeds = _WorkingPositionSpeeds;
        }
    }

    // 时间参数
    public class TimeArgs
    {
        public List<float> timeArgs; // 时间参数

        public TimeArgs(List<float> _timeArgs)
        {
            timeArgs = _timeArgs;
        }
    }

    // 系统参数
    public class SystemArgs
    {
        private int targetOutPut;  // 目标产量
        public int TargetOutPut
        {
            get { return targetOutPut; }
            set { targetOutPut = value; }
        }

        public List<int> reserved;  // 备用
    }

    // 机台自定义
    public class UserDefine
    {

    }

    public enum PriorityMode
    {
        Disable,
        ResultTime,  // 实时刷新
        Fastest,
        Medium,
        Normal,
        Slow,
    }

    public class BaseHeader
    {
        public string Version;
        public string Revision;
        public string ProjectName;
        public int LineID;
        public int SectionID;
        public int MachineID;
        public PriorityMode PollPriority;
        public PriorityMode HeartbeatPriority;
        public int Reserved;
        public string PlcWriteAddressFlag;
        public int PlcWriteAddressBegin;
        public string EapWriteAreaFlag;
        public int EapWriteAreaBegin;
        public int EventCount;

        public BaseHeader(string ver, string rev, string pro, int lineid, int sectionId,
            int machineId, PriorityMode poll, PriorityMode heartbeat, int reserved, string plcflag,
            int plcadrressbegin, string eapflag, int eapareabegin, int eventcount)
        {
            Version = ver;
            Revision = rev;
            ProjectName = pro;
            LineID = lineid;
            SectionID = sectionId;
            MachineID = machineId;
            PollPriority = poll;
            HeartbeatPriority = heartbeat;
            Reserved = reserved;
            PlcWriteAddressFlag = plcflag;
            PlcWriteAddressBegin = plcadrressbegin;
            EapWriteAreaFlag = eapflag;
            EapWriteAreaBegin = eapareabegin;
            EventCount = eventcount;
        }

        public BaseHeader()
        {
        }
    }

}
