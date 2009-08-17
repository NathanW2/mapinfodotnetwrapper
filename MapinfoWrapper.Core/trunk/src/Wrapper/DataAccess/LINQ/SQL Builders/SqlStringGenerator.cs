namespace MapinfoWrapper.DataAccess.LINQ.SQLBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Geometries;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;

    internal class SqlStringGenerator
    {
        public string GenerateInsertString(BaseEntity entity, string tableName)
        {
            Dictionary<string, object> mapping = new Dictionary<string, object>();
            PropertyInfo[] props = entity.GetType().GetProperties();

            StringBuilder sb = new StringBuilder("INSERT INTO {0}".FormatWith(tableName));

            foreach (PropertyInfo property in props)
            {
                string Name = property.Name;

                if (string.Equals(Name, "rowid", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                object value = property.GetValue(entity,null);

                if (string.Equals(Name, "obj", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (value == null) break;

                    string createstring = ((Geometry) value).ToBasicCreateCommand();
                    mapping.Add(Name,createstring);
                    break;
                }

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
