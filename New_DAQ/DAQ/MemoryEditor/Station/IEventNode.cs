using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryEditor
{
    public interface IEventNode : IExtendProperty
    {
        #region Property
        /// <summary>
        /// 节点名称
        /// </summary>
        string EventName { get; set; }

        /// <summary>
        /// 内存开始位置
        /// </summary>
        int MemoryBegin { get; set; }

        /// <summary>
        /// 内存结束位置
        /// </summary>
        int MemoryEnd { get; set; }

        /// <summary>
        /// 数据大小  word
        /// </summary>
        int DataSize { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }

        #endregion
    }
}
