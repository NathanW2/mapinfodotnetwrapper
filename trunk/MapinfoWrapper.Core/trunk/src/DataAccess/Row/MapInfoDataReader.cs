using System.Data;
using MapInfo.Wrapper.Core;
using MapInfo.Wrapper.Core.Extensions;
using System;
using MapInfo.Wrapper.Geometries;
using MapInfo.Wrapper.Mapinfo;


namespace MapInfo.Wrapper.DataAccess.Row
{
    /// <summary>
    /// Reads data from the suppiled Mapinfo table.
    /// </summary>
    // HACK! This object feels like it is doing a bit to much and needs to be refactored.
    public class MapInfoDataReader : IMapInfoDataReader
    {
        private readonly IMapInfoWrapper MapInfoSession;
        private readonly IGeometryFactory geometryfactory;

        public MapInfoDataReader(IMapInfoWrapper miSession, string tableName)
        {
            this.MapInfoSession = miSession;
            this.TableName = tableName;
            this.geometryfactory = new GeometryFactory(new MapInfoSession(miSession));
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
            this.MapInfoSession.Do("Fetch Rec {0} From {1}".FormatWith(recordIndex, this.TableName));
        }

        public void FetchLast()
        {
            this.MapInfoSession.Do("Fetch Last From {0}".FormatWith(this.TableName));
        }

        public void FetchNext()
        {
            this.MapInfoSession.Do("Fetch Next From {0}".FormatWith(this.TableName));
        }

        public void FetchFirst()
        {
            this.MapInfoSession.Do("Fetch First From {0}".FormatWith(this.TableName));
        }

        public bool EndOfTable()
        {
            return (MapInfoSession.Eval("EOT({0})".FormatWith(this.TableName)) == "T");
        }

        public object Get(string columnName)
        {
            string value = this.MapInfoSession.Eval("{0}.{1}".FormatWith(this.TableName, columnName));
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
                GeometryBuilder georeader = new GeometryBuilder(this.TableName, this.MapInfoSession);
                Geometry geo = georeader.CreateGeometry();
                return geo;
            }

            string columntypestring =
                this.MapInfoSession.Eval("ColumnInfo({0},{1},{2})".FormatWith(this.TableName, columnName, 3));
            int columntypeval = Convert.ToInt32(columntypestring);
            ColumnType columntype = (ColumnType) columntypeval;
            switch (columntype)
            {
                case ColumnType.CHAR:
                    return value;
                case ColumnType.DECIMAL:
                    return Convert.ToDecimal(value);
                case ColumnType.INTEGER:
                    return Convert.ToInt32(value);
                case ColumnType.SMALLINT:
                    return Convert.ToInt16(value);
                case ColumnType.DATE:
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
                case ColumnType.LOGICAL:
                    return (value == "T");
                case ColumnType.GRAPHIC:
                    break;
                case ColumnType.FLOAT:
                    return Convert.ToDouble(value);
                case ColumnType.TIME:
                    break;
                case ColumnType.DATETIME:
                    DateTime date;
                    bool parsed = DateTime.TryParseExact(value,
                                                         "yyyyMMddHHmmssfff",
                                                         null,
                                                         System.Globalization.DateTimeStyles.None,
                                                         out date);
                    if (parsed)
                        return date;
                    break;
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
