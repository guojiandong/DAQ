using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAQ
{
    [Serializable]
    public enum DataType
    {
        _16_BCD = 0,
        _32_BCD = 1,
        _16_Hex = 2,
        _32_Hex = 3,
        _16_Binary = 4,
        _32_Binary = 5,
        _16_Unsigned = 6,
        _16_Signed = 7,
        _32_Unsigned = 8,
        _32_Signed = 9,
        _32_Float = 10,
    }

    [Serializable]
    public enum ChangeMode
    {
        Create = 0,
        Change = 1,
    }

    [Serializable]
    public enum ComponentType
    {
        TextComponent = 1,
        BtnComponent = 2,
    }

    [Serializable]
    public enum OperatorType
    {
        _On = 0,
        _Off = 1,
        _Switch = 2,
        _Reversion = 3,
    }

    [Serializable]
    public enum PressType
    {
        Auto_Up = 0,
        Keep = 1,
    }

    [Serializable]
    public class Component
    {
        [XmlElement("id")]              // 预留字段-空闲
        public string id { get; set; }

        [XmlElement("note")]
        public string note { get; set; } // 注釋 針對btn類型

        [XmlElement("offset")]           
        public string offset { get; set; }  //偏移 ：針對Text類型

        [XmlElement("isEnable_Input")]
        public string isEnable_Input { get; set; }  // 是否輸入

        [XmlElement("componentType")]
        public int componentType { get; set; }   // 控件類型

        [XmlElement("operatorType")]
        public OperatorType operatorType { get; set; } //操作类型

        [XmlElement("data_Type")] 
        public DataType data_Type { get; set; }   //数据类型

        //btn 類型專用
        [XmlElement("in_word_offset")]
        public string in_word_offset { get; set; }  // 讀出 字偏移

        [XmlElement("in_bit_offset")]
        public string in_bit_offset { get; set; }  // 寫入 位偏移

        [XmlElement("out_word_offset")]
        public string out_word_offset { get; set; } // 讀出字偏移

        [XmlElement("out_bit_offset")]
        public string out_bit_offset { get; set; } // 讀出位偏移

        [XmlElement("pressType")]
        public PressType pressType { get; set; }  // 按钮按下后的状态

        [XmlElement("ponit")]
        public int point { get; set; }  // 按钮按下后的状态

        public Component()
        {
            note = "";
            offset = "0";
            isEnable_Input = "false";
            componentType = 1;
            operatorType = OperatorType._On;
            data_Type = DataType._16_BCD;
            in_word_offset = "0";
            in_bit_offset = "0";
            out_word_offset = "0";
            out_bit_offset = "0";
            pressType = PressType.Auto_Up;
            point = 0;
        }
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
