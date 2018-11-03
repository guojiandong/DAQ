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
    public class ColumnAttribute : System.Attribute
    {

        public string Name { get; set; }

        public int MaxLength { get; set; }
        //string 

        //public SqliteDataType ColumnType { get; set; }


        public ColumnAttribute() : this("")
        {
        }

        public ColumnAttribute(string name) : this(name, 0)
        {
        }

        public ColumnAttribute(string name, int len) //: this(name, type, false)
        {
            this.Name = name;

            this.MaxLength = len;
        }

        //public ColumnAttribute(string name, System.Data.DbType type, bool primarykey)
        //    : this(name, type, false, "")
        //{
        //}


        //public ColumnAttribute(string name, System.Data.DbType type, bool primarykey, string value)
        //{
        //    this.Name = name;

        //    this.MaxLength = type;

        //    this.PrimaryKey = primarykey;

        //    this.DefaultValue = value;
        //}
    }
}
