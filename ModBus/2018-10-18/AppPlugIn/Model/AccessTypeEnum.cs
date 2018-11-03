using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ksat.AppPlugIn.Model
{
    [Guid("686F8B3D-1AB7-4A31-A061-709E50255DDA")]
    public enum AccessTypeEnum : UInt32
    {
        None = 0,


        Visiable = (1 << 0),


        Open = (1 << 1),


        Close = (1 << 2),
        
        //Write = (1 << 1),
    }
}
