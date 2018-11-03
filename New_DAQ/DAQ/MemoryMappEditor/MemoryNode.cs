using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleExcel;
using SimpleExcel.Attributes;
using SimpleExcel.Styles;

namespace MemoryMappEditor
{
    [Row(
    EvenRowColor = ExcelColor.Aqua,
    OddRowColor = ExcelColor.CornflowerBule,
    HeaderBackColor = ExcelColor.Maroon,
    HeaderFontColor = ExcelColor.White,
    HeaderHeight = 20,
    HeaderHorAlign = HorizontalAlign.Center,
    HeaderVerAlign = VerticalAlign.Center)]
    class MemoryNode
    {
        [Column(
           BackColor = ExcelColor.Brown,
           FontColor = ExcelColor.White,
           FontSize = 14,
           FontFamily = "黑体",
           HorAlign = HorizontalAlign.Center,
           VerAlign = VerticalAlign.Center,
           Name = "开始地址")]
        public int MemoryBegin { get; set; }

        [Column(
           BackColor = ExcelColor.Brown,
           FontColor = ExcelColor.White,
           FontSize = 14,
           FontFamily = "黑体",
           HorAlign = HorizontalAlign.Center,
           VerAlign = VerticalAlign.Center,
           Name = "结束地址")]
        public int MemoryEnd { get; set; }

        [Column(
            FontColor = ExcelColor.Red,
            HorAlign = HorizontalAlign.Left,
            VerAlign = VerticalAlign.Center,
            Name = "大小")]
        public int DataSize { get; set; }

        [Column(
            FontColor = ExcelColor.Red,
            HorAlign = HorizontalAlign.Left,
            VerAlign = VerticalAlign.Center,
            Name = "描述")]
        public string Description { get; set; }

        public MemoryNode()
        {

        }

        public MemoryNode(int _MemoryBegin, int _MemoryEnd , int _DataSize, string _Description)
        {
            MemoryBegin = _MemoryBegin;
            MemoryEnd = _MemoryEnd;
            DataSize = _DataSize;
            Description = _Description;
        }
    }
}
