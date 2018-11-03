using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.LogMySql.Model
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple = true)]
    public class ColumnIgnoreAttribute : Attribute
    {
    }
}
