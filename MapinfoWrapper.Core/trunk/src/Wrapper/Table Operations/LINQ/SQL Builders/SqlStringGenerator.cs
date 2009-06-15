using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.Core.Extensions;

namespace MapinfoWrapper.TableOperations.LINQ.SQLBuilders
{
    public class SqlStringGenerator
    {
        public string GenerateInsertString<T>(T entity, string tableName)
        {
            Dictionary<string, object> mapping = new Dictionary<string, object>();

            StringBuilder sb = new StringBuilder("INSERT INTO {0}".FormatWith(tableName));
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                string Name = property.Name;

                if (string.Equals(Name, "rowid", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                object value = property.GetValue(entity,null);
                value = value ?? "";

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String:
                        value = ((string)value).InQuotes();
                        break;
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                        value = Convert.ToInt32(value);
                        break;
                    case TypeCode.DateTime:
                        value = ((DateTime)value).ToString().InQuotes();
                        break;
                    case TypeCode.Object:
                        DateTime? tempdate = value as DateTime?;
                        if (tempdate.HasValue)
                        {
                            value = tempdate.ToString().InQuotes();
                            break;
                        }

                        IGeometry obj = value as IGeometry;
                        if (obj != null)
                        {
                            value = obj.expression.ToString();
                            break;
                        }
                        break;
                    default:
                        break;
                }
                mapping.Add(Name, value);
            }

            sb.Append(GenerateValuesColumnMapping(mapping));
            return sb.ToString();
        }

        private string GenerateValuesColumnMapping(Dictionary<string, object> mappings)
        {
            StringBuilder columns = new StringBuilder(" (");
            StringBuilder values = new StringBuilder(" VALUES (");
            foreach (var item in mappings)
            {
                columns.AppendFormat("{0},", item.Key);
                object value = item.Value;
                values.AppendFormat("{0},", item.Value);
            }
            columns.Remove(columns.Length - 1, 1);
            values.Remove(values.Length - 1, 1);
            columns.Append(")");
            values.Append(")");
            return columns.ToString() + values.ToString();
        }

    }
}
