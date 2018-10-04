using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAQ
{
    public enum DataType
    {
        _16_uint = 0,
        _16_int,
        _32_uint,
        _32_int,
        _int64,
    }

    public enum ChangeMode
    {
        Create = 0,
        Change = 1,
    }

    public enum ComponentType
    {
        TextComponent = 1,
        BtnComponent = 2,
    }

    public enum OperatorType
    {
        Auto = 0,
        Manual = 1,
    }

    public enum PressType
    {
        Auto_Up = 0,
        Keep = 1,
    }

    public class Component
    {
        public string Id ;   
        public string note;   // 注釋 針對btn類型
        public string offset; //偏移 ：針對Text類型
        public string isEnable_Input; // 是否輸入
        public int componentType;   // 控件類型
        public OperatorType operatorType; // 操作類型
        public DataType data_Type;   // 數據類型

        //btn 類型專用
        public string in_word_offset;  // 讀出 字偏移
        public string in_bit_offset;  // 寫入 位偏移

        public string out_word_offset;// 讀出字偏移
        public string out_bit_offset; // 讀出位偏移

        public PressType pressType = PressType.Auto_Up; // 按钮按下后的状态


        /*
        public Component(string _id, string _offset, string _note, bool _isEnable_Input, ComponentType _componentType, OperatorType _operatorType, DataType _data_Type)
        {
            offset = _offset;
            note = _note;
            isEnable_Input = _isEnable_Input;
            componentType = _componentType;
            operatorType = _operatorType;
            data_Type = _data_Type;
        }
        
        public string Id {get{return Id;}set{Id = value;}}
        public string note { get { return note; } set { note = value; } }
        public string offset { get { return offset; } set { offset = value; } }
        public string isEnable_Input { get { return isEnable_Input; } set { isEnable_Input = value; } }
        public ComponentType componentType { get { return componentType; } set { componentType = value; } }
        public OperatorType operatorType { get { return operatorType; } set { operatorType = value; } }
        public DataType data_Type { get { return data_Type; } set { data_Type = value; } }  
          */
    }


    public class MemoryState
    {
        public int index;
        public int Index                //序号
        {
            get { return index; }
            set { index = value; }
        }

        public int used_count;
        public int Used_count                //序号
        {
            get { return used_count; }
            set { used_count = value; }
        }

        public int com_type;
        public int Com_type            //控件类型
        {
            get { return com_type; }
            set { com_type = value; }
        }

        public bool state;            //使用状态
        public bool State              
        { 
            get { return state; }
            set { state = value; }
        }
        public string bit_used_str;
        public string Bit_used_str    //使用的bit位标识  比如： "1,2,4" :标识 该字的 1,2,4 bit位已经被使用了
        {
            get { return bit_used_str; }
            set { bit_used_str = value; }
        }

        public MemoryState(int index, int type, bool state, int used_count ,string use_str)
        {
            this.index = index;
            this.com_type = type;
            this.state = state;
            this.bit_used_str = use_str;
            
        }
    }
    
}
