using System;
using Ksat.AppPlugIn.Common.Attr;

namespace Ksat.SqliteHelper
{
    public class ColumnInfo
    {
        public ColumnInfo(string name)
        {
            this.Name = name;
            this.ColumnType = SqliteDataType.TEXT;
            this.DefaultValue = "";
            this.PrimaryKey = false;
        }

        //[Column("name")]
        public string Name { get; set; }

        //          [Column ("type")]
        public SqliteDataType ColumnType { get; set; }

        public int notnull { get; set; }

        public string DefaultValue { get; set; }

        public bool PrimaryKey { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
