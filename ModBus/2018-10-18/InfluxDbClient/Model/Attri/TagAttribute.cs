using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.InfluxDbClient.Model.Attri
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple = true)]
    public class TagAttribute : Attribute
    {
        public TagAttribute()
        {
            Name = string.Empty;
        }

        public TagAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
