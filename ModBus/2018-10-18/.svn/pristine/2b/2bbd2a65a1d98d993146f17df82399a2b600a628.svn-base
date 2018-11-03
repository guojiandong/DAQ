using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.InfluxDbClient.Model.Attri
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple = true)]
    public class FieldAttribute : Attribute
    {
        public FieldAttribute()
        {
            Name = string.Empty;
        }

        public FieldAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
