using MemoryMappEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleExcel;

namespace MemoryMappEditor
{
    class DataProxy
    {
        public List<MemoryNode> plcMemory;
        public List<MemoryNode> eapMemory;
        public List<FixedEvent> fixedEventList;
        public BaseHeader baseHeader;
        public Dictionary<int, int> Event2DataSizeMap = new Dictionary<int, int>();

        public DataProxy()
        {
            // plc data area
            plcMemory      = new List<MemoryNode>();
            // eap data area
            eapMemory      = new List<MemoryNode>();
            //Fixed Event
            fixedEventList = new List<FixedEvent>();

            InitEventDataSize();
        }
        private static DataProxy instance;
        public static DataProxy GetInstance()
        {
            if (instance == null)
                instance = new DataProxy();
            return instance;
        }
        public void InitEventDataSize()
        {
            Event2DataSizeMap.Clear();
            Event2DataSizeMap.Add((int)EventCode.Rigseter, ConstFile.Register_DataSize);
            Event2DataSizeMap.Add((int)EventCode.EapCommand, ConstFile.EapCommand_DataSize);
            Event2DataSizeMap.Add((int)EventCode.AlarmText, ConstFile.AlarmText_DataSize);
            Event2DataSizeMap.Add((int)EventCode.MotorSettings, ConstFile.MotorSetting_DataSize);
            Event2DataSizeMap.Add((int)EventCode.NormalSettings, ConstFile.NormalSettings_DataSize);
            Event2DataSizeMap.Add((int)EventCode.HeartBeat, ConstFile.HeartBeat_DataSize);
            Event2DataSizeMap.Add((int)EventCode.EventChangedFlag, ConstFile.EventChangedFlag_DataSize);
            Event2DataSizeMap.Add((int)EventCode.BreakPoint, ConstFile.BreakPoint_DataSize);
            Event2DataSizeMap.Add((int)EventCode.AlarmCode, ConstFile.AlarmCode_DataSize);
            Event2DataSizeMap.Add((int)EventCode.MachineStatus, ConstFile.MachineStatus_DataSize);
            Event2DataSizeMap.Add((int)EventCode.CheckIn, ConstFile.CheckIn_DataSize);
            Event2DataSizeMap.Add((int)EventCode.CheckOut, ConstFile.CheckOut_DataSize);
        }

        private MemoryNode plcMemoryNode;
        private MemoryNode eapMemoryNode;
        private MemoryNode subEvent;
        private int curPlcAddressOffset = ConstFile.PLC_EVENT_DATA_START;
        private int curEapAddressOffset = ConstFile.EAP_EVENT_DATA_START;

        // 初始化数据
        public void InitDataProxy()
        {
            //BaseHeader
            if(baseHeader == null)
                baseHeader = new BaseHeader("0","0","0",0,0,0,PriorityMode.Medium,PriorityMode.Medium,8,ConstFile.Flag,ConstFile.PLC_EVENT_DATA_START,ConstFile.Flag,ConstFile.EAP_EVENT_DATA_START,13);

            plcMemory.Clear();
            eapMemory.Clear();
            curPlcAddressOffset = baseHeader.PlcWriteAddressBegin; // ConstFile.PLC_EVENT_DATA_START;
            curEapAddressOffset = baseHeader.EapWriteAreaBegin;// ConstFile.EAP_EVENT_DATA_START;

            List<EventCode> events = new List<EventCode>();
            events.Add(EventCode.Rigseter);
            events.Add(EventCode.EapCommand);
            events.Add(EventCode.AlarmText);
            events.Add(EventCode.MotorSettings);
            events.Add(EventCode.NormalSettings);
            events.Add(EventCode.HeartBeat);
            events.Add(EventCode.EventChangedFlag);
            events.Add(EventCode.BreakPoint);
            events.Add(EventCode.AlarmCode);
            events.Add(EventCode.MachineStatus);
            events.Add(EventCode.CheckIn);
            events.Add(EventCode.CheckOut);
            // 初始化固定事件
            foreach(var ev in events)
            {
                AddEvent(ev);
            }
        }

        // 添加一级命令事件
        public void AddEvent(EventCode code)
        {
            int data_size = Event2DataSizeMap[(int)code];
            curPlcAddressOffset += 2;
            plcMemoryNode = new MemoryNode(curPlcAddressOffset, curPlcAddressOffset + data_size - 1, data_size,code.ToString());
            curPlcAddressOffset = curPlcAddressOffset + data_size;
            plcMemory.Add(plcMemoryNode);

            curEapAddressOffset += 3;
            eapMemoryNode = new MemoryNode(curEapAddressOffset, curEapAddressOffset + data_size - 1, data_size, code.ToString());
            curEapAddressOffset = curEapAddressOffset + data_size;
            eapMemory.Add(eapMemoryNode);
        }

        // 添加二级菜单
        public void AddSubEvent()
        {
            subEvent = new MemoryNode();
            // 构造二级菜单
        }

        //数据抖动 - 比如某个事件的datasize 变化了之后， 其及之后的内存都应该重新计算
        public void DataJitter()
        {
            //InitEventDataSize(); 
            InitDataProxy();
        }

        public void SetEvent2DataSizeMapByEventCode(int even_code, int data_size)
        { 
            if (Event2DataSizeMap.ContainsKey(even_code))
                Event2DataSizeMap[even_code] = data_size;
            else
                Event2DataSizeMap.Add(even_code, data_size);
        }
    }
}
