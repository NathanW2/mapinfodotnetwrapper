using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Core.Extensions;

namespace MapinfoWrapper.DataAccess.LINQ.SQL
{
    public class UpdateQuery : Query
    {
        private MapinfoSession mapinfo;

        public UpdateQuery(MapinfoSession session, string tableName)
        {
            Guard.AgainstNull(session, "session");

            this.mapinfo = session;
            this.TableName = tableName;
        }

        private List<ColumnWithData> mappings = new List<ColumnWithData>();
        public List<ColumnWithData> ColumnsAndData
        {
            get
            
            {
                return this.mappings;
            }
        }

        public string TableName { get; set; }

        public override void Excute()
        {
            string command = this.GetQueryString();
            this.mapinfo.Do(command);
        }

        public override string GetQueryString()
        {
            if (this.ColumnsAndData.Count <= 0)
                return "";

            StringBuilder updatestring = new StringBuilder("UPDATE {0} SET ".FormatWith(this.TableName));
            string wherestring = "";

            foreach (var item in this.ColumnsAndData)
            {
                object resultvalue = String.Empty;
                object data = item.Value;

                switch (item.Type)
                {
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.CHAR:
                        if (data == null)
                            data = String.Empty;
                        resultvalue = ((string)data).InQuotes();
                        break;
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.DECIMAL:
                        resultvalue = Convert.ToDecimal(data);
                        break;
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.INTEGER:
                        resultvalue = Convert.ToInt32(data);
                        break;
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.SMALLINT:
                        resultvalue = Convert.ToInt16(data);
                        break;
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.DATE:
                        if (data == null || ((DateTime)data) == DateTime.MinValue)
                            resultvalue = String.Empty.InQuotes();
                        else
                        {
                            DateTime date = (DateTime)data;
                            resultvalue = date.ToString("d").InQuotes();
                        }
                        break;
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.LOGICAL:
                        resultvalue = ((Boolean)data ? "T" : "F").InQuotes();
                        break;
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.FLOAT:
                        break;
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.TIME:
                        if (data == null || ((DateTime)data) == DateTime.MinValue)
                            resultvalue = String.Empty.InQuotes();
                        else
                            resultvalue = ((DateTime)data).TimeOfDay.ToString().InQuotes();
                        break;
                    case MapinfoWrapper.DataAccess.RowOperations.ColumnType.DATETIME:
                        if (data == null || ((DateTime)data) == DateTime.MinValue)
                            resultvalue = String.Empty.InQuotes();
                        else
                            resultvalue = ((DateTime)data).ToString().InQuotes();
                        break;
                    default:
                        break;
                }

                if (string.Equals(item.Name, "rowid", StringComparison.InvariantCultureIgnoreCase))
                {
                    wherestring = " WHERE RowID = {0}".FormatWith(data);
                    continue;
                }

                updatestring.AppendFormat("{0} = {1},", item.Name, resultvalue);

            }
            return updatestring.ToString().TrimEnd(',') + wherestring;
        }
    }
}
