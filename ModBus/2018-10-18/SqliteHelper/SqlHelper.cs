using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.SqliteHelper
{
    public static class SqlHelper
    {
        public static SqliteDataType GetSqliteDataType(Type t)
        {
            if (t.Equals(typeof(string))
                || t.Equals(typeof(bool)) || t.Equals(typeof(Boolean)))
            {
                return SqliteDataType.TEXT;
            }

            if (t.Equals(typeof(int)) || t.Equals(typeof(Int32)) || t.Equals(typeof(UInt32))
                || t.Equals(typeof(Int16)) || t.Equals(typeof(UInt16))
                || t.Equals(typeof(short)) || t.Equals(typeof(ushort))
                )
            {
                return SqliteDataType.INTEGER;
            }

            if (t.Equals(typeof(Int64)) || t.Equals(typeof(UInt64))
                || t.Equals(typeof(long)) || t.Equals(typeof(ulong)))
            {
                return SqliteDataType.BIGINT;
            }

            if (t.Equals(typeof(char)) || t.Equals(typeof(Char))
                || t.Equals(typeof(byte)) || t.Equals(typeof(Byte))
                )
            {
                return SqliteDataType.TINYINT;
            }

            if (t.Equals(typeof(float)) || t.Equals(typeof(double)) || t.Equals(typeof(Double))
                || t.Equals(typeof(decimal)) || t.Equals(typeof(Decimal)) || t.Equals(typeof(Single)))
            {
                return SqliteDataType.REAL;
            }

            return SqliteDataType.TEXT;
        }

        public static string GetSqliteDataTypeString(Type t)
        {
            return GetSqliteDataType(GetSqliteDataType(t));
        }

        public static string GetSqliteDataType(SqliteDataType data_type)
        {
            switch (data_type)
            {
                case SqliteDataType.INTEGER:
                case SqliteDataType.TINYINT:
                case SqliteDataType.MEDIUMINT:
                case SqliteDataType.BIGINT:
                case SqliteDataType.REAL:
                case SqliteDataType.TEXT:
                case SqliteDataType.BLOB:
                case SqliteDataType.NUMERIC:
                    return data_type.ToString();
                default:
                    return "TEXT";
            }
        }
    }
}
