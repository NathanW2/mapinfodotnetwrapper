﻿namespace MapinfoWrapper.DataAccess.RowOperations
{
    using System;
    using System.Linq;
    using System.Reflection;
    using MapinfoWrapper.Core;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.Geometries;
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;
    using System.Collections.Generic;

    /// <summary>
    /// Reads data from the suppiled Mapinfo table.
    /// </summary>
    // HACK! This object feels like it is doing a bit to much and needs to be refactored.
    internal class DataReader : IDataReader
    {
        private readonly MapinfoSession MapinfoSession;
        private readonly IGeometryFactory geometryfactory;
        private int currentrecord;

        public DataReader(MapinfoSession MISession, string tableName)
        {
            this.MapinfoSession = MISession;
            this.TableName = tableName;
            this.geometryfactory = new GeometryFactory(MISession);
        }

        public string TableName {get; private set;}
        public int CurrentRecord
        {
            get 
            {
                return Convert.ToInt32(this.Get("RowID"));
            }
        }

        public void Fetch(int recordIndex)
        {
            this.MapinfoSession.RunCommand("Fetch Rec {0} From {1}".FormatWith(recordIndex, this.TableName));
        }

        public void FetchLast()
        {
            this.MapinfoSession.RunCommand("Fetch Last From {0}".FormatWith(this.TableName));
        }

        public void FetchNext()
        {
            this.MapinfoSession.RunCommand("Fetch Next From {0}".FormatWith(this.TableName));
        }

        public void FetchFirst()
        {
            this.MapinfoSession.RunCommand("Fetch First From {0}".FormatWith(this.TableName));
        }

        public bool EndOfTable()
        {
            return (MapinfoSession.Evaluate("EOT({0})".FormatWith(this.TableName)) == "T");
        }

        public object Get(string columnName)
        {
            string value = this.MapinfoSession.Evaluate("{0}.{1}".FormatWith(this.TableName, columnName));
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
                GeometryBuilder georeader = new GeometryBuilder(this.TableName,this.MapinfoSession);
                Geometry geo = georeader.CreateGeometry();
                return geo;
            }

            string columntypestring = this.MapinfoSession.Evaluate("ColumnInfo({0},{1},{2})".FormatWith(this.TableName, columnName, 3));
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
                    DateTime date2;
                    bool parsed2 = DateTime.TryParseExact(value,
                                                        "yyyyMMdd",
                                                        null,
                                                        System.Globalization.DateTimeStyles.None,
                                                        out date2);
                    if (parsed2)
                        return date2;
                    else
                        return null;
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

        public bool Read()
        {
            this.FetchNext();
            return this.EndOfTable();
        }
    }
}
