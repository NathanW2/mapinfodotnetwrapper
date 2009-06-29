using System;
using MapinfoWrapper.Core;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Wrapper.Geometries;

namespace MapinfoWrapper.DataAccess.RowOperations
{
    internal class DataReader : IDataReader
    {
        private readonly IMapinfoWrapper wrapper;
        private readonly string tableName;
        private readonly IGeometryFactory geometryfactory;

        public DataReader(IMapinfoWrapper wrapper,string tableName, IGeometryFactory geometryFactory)
        {
            this.wrapper = wrapper;
            this.tableName = tableName;
            this.geometryfactory = geometryFactory;
        }

        public string GetName(int index)
        {
            return this.wrapper.Evaluate("ColumnInfo({0},COL{1},1".FormatWith(this.tableName, index.ToString().InQuotes()));
        }

        public void Fetch(int recordIndex)
        {
            this.wrapper.RunCommand("Fetch Rec {0} From {1}".FormatWith(recordIndex, this.tableName));
        }

        public void FetchLast()
        {
            this.wrapper.RunCommand("Fetch Last From {0}".FormatWith(this.tableName));
        }

        public void FetchNext()
        {
            this.wrapper.RunCommand("Fetch Next From {0}".FormatWith(this.tableName));
        }

        public void FetchFirst()
        {
            this.wrapper.RunCommand("Fetch First From {0}".FormatWith(this.tableName));
        }

        public bool EndOfTable()
        {
            return (wrapper.Evaluate("EOT({0})".FormatWith(this.tableName)) == "T");
        }

        public int GetColumnCount()
        {
            string value = this.wrapper.Evaluate("TableInfo({0},4)".FormatWith(this.tableName));
            return Convert.ToInt32(value);
        }

        public object Get(string columnName)
        {
            string value = this.wrapper.Evaluate("{0}.{1}".FormatWith(this.tableName, columnName));
            return CastToColumnType(columnName, value);
        }

        private object CastToColumnType(string columnName, string value)
        {
            Guard.AgainstNull(columnName, "Column Name");

            if (string.Equals(columnName, "rowid", StringComparison.InvariantCultureIgnoreCase))
            {
                return Convert.ToInt32(value);
            }

            if (string.Equals(columnName, "obj", StringComparison.InvariantCultureIgnoreCase))
            {
                int index = Convert.ToInt32(this.Get("rowid"));
                TableObjVariable objvariable = new TableObjVariable(this, index);
                return geometryfactory.GetGeometryFromVariable(objvariable);
            }

            string columntypestring = this.wrapper.Evaluate("ColumnInfo({0},{1},{2})".FormatWith(this.tableName, columnName, 3));
            int columntypeval = Convert.ToInt32(columntypestring);
            ColumnTypes columntype = (ColumnTypes)columntypeval;
            switch (columntype)
            {
                case ColumnTypes.CHAR:
                    return value;
                case ColumnTypes.DECIMAL:
                    return Convert.ToDecimal(value);
                case ColumnTypes.INTEGER:
                    return Convert.ToInt32(value);
                case ColumnTypes.SMALLINT:
                    return Convert.ToInt16(value);
                case ColumnTypes.DATE:
                    break;
                case ColumnTypes.LOGICAL:
                    return (value == "T");
                case ColumnTypes.GRAPHIC:
                    break;
                case ColumnTypes.FLOAT:
                    return Convert.ToDouble(value);
                case ColumnTypes.TIME:
                    break;
                case ColumnTypes.DATETIME:
                    DateTime date;
                    bool parsed = DateTime.TryParseExact(value,
                                                        "yyyyMMddHHmmssfff",
                                                        null,
                                                        System.Globalization.DateTimeStyles.None,
                                                        out date);
                    if (parsed)
                        return date;
                    else
                        return null;
                default:
                    return null;
            }
            return null;
        }


        public string GetTableAndRowString(string columnName)
        {
            return "{0}.{1}".FormatWith(this.tableName, columnName);
        }
    }
}
