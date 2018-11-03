using Ksat.AppPlugIn.Common.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Communicate.SuperIoc
{
    [Guid("8CD0DF68-B546-433E-9326-4F7E520C56D6")]
    public enum CommunicateState : int
    {
        [EnumDescription("未知")]
        None = 0x00,
        [EnumDescription("通讯中断")]
        Interrupt = 0x01,
        [EnumDescription("通讯干扰")]
        Error = 0x02,
        [EnumDescription("通讯正常")]
        Communicate = 0x03
    }
}
