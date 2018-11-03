using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ksat.AppPlugIn.Model
{
    [Guid("4775C7DB-5201-44B4-BA18-FDA91A9D4B08")]
    public enum RoleTypeEnum : UInt32
    {
        /// <summary>
        /// 来宾
        /// </summary>
        Guest = 0,

        /// <summary>
        /// 操作员
        /// </summary>
        Operator,

        /// <summary>
        /// 维修人员
        /// </summary>
        Repairer,

        /// <summary>
        /// 管理员
        /// </summary>
        Administrator,

        /// <summary>
        /// 工程师
        /// </summary>
        Engineer,
    }
}
