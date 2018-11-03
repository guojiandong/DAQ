using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Common.Attr
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct,
                       AllowMultiple = true)  // multiuse attribute
    ]
    public class TableNameAttribute : System.Attribute
    {
        public string Name { get; set; }

        public TableNameAttribute(string n)
        {
            this.Name = n;
        }
    }
}
