using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Common.Attr
{
    [System.AttributeUsage(System.AttributeTargets.Parameter
                                | AttributeTargets.GenericParameter
                                | AttributeTargets.Property,
                       AllowMultiple = true)  // multiuse attribute
    ]
    public class PrimaryKeyAttribute : System.Attribute
    {
    }
}
