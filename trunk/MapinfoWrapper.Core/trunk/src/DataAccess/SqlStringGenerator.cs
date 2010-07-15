using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MapInfo.Wrapper.Core;
using MapInfo.Wrapper.Core.Extensions;
using MapInfo.Wrapper.DataAccess.Entities;
using MapInfo.Wrapper.DataAccess.LINQ.SQL;
using MapInfo.Wrapper.DataAccess.Row;
using MapInfo.Wrapper.Geometries;
using MapInfo.Wrapper.Mapinfo;
using System.Linq;

namespace MapInfo.Wrapper.DataAccess
{
    /// <summary>
    /// A class that can be used to generate SQL strings from wrapper entities.
    /// </summary>
    public class SqlStringGenerator
    {
        private IMapInfoWrapper mapinfo;

        /// <summary>
        /// Creates a new instance of <see cref="SqlStringGenerator"/> that can be used to generate SQL commands from entities.
        /// </summary>
        /// <param name="session"></param>
        public SqlStringGenerator(IMapInfoWrapper session)
        {
            this.mapinfo = session;
        }

        /// <summary>
        /// Generates a <see cref="UpdateQuery"/> for the supplied entity, using the data in each of the entities
        /// properties with 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public UpdateQuery GenerateUpdateQuery(BaseEntity entity, string tableName)
        {
            Guard.AgainstNull(entity, "entity");

            IEnumerable<PropertyInfo> props = GetVaildProperties(entity);

            StringBuilder updatestring = new StringBuilder("UPDATE {0} SET ".FormatWith(entity.Table.Name));
            string wherestring = " WHERE RowID = {0}".FormatWith(entity.RowId); ;

            UpdateQuery query = new UpdateQuery(this.mapinfo, entity.Table.Name);

            ColumnWithData col;

            foreach (PropertyInfo property in props)
            {
                string name = property.Name;
                object columnvalue = property.GetValue(entity, null);
                MapInfoColumnAttribute attribute = (MapInfoColumnAttribute)property.GetCustomAttributes(typeof(MapInfoColumnAttribute), true).FirstOrDefault();
                ColumnType columnType = attribute.Type;

                object resultvalue = null;
                resultvalue = GetConvertedValue(columnType, columnvalue);
                updatestring.AppendFormat("{0} = {1},", name, resultvalue);
            }
            return new UpdateQuery(this.mapinfo, updatestring.ToString().TrimEnd(',') + wherestring);
        }

        private object GetConvertedValue(ColumnType columnType, object columnvalue)
        {
            switch (columnType)
            {
                case ColumnType.CHAR:
                    if (columnvalue == null)
                        columnvalue = String.Empty;
                    return ((string)columnvalue).InQuotes();
                case ColumnType.DECIMAL:
                    return Convert.ToDecimal(columnvalue);
                case ColumnType.INTEGER:
                    return Convert.ToInt32(columnvalue);
                case ColumnType.SMALLINT:
                    return Convert.ToInt16(columnvalue);
                case ColumnType.DATE:
                    if (columnvalue == null || ((DateTime)columnvalue) == DateTime.MinValue)
                        return String.Empty.InQuotes();
                    else
                    {
                        DateTime date = (DateTime)columnvalue;
                        return date.ToString("d").InQuotes();
                    }
                case ColumnType.LOGICAL:
                    return ((Boolean)columnvalue ? "T" : "F").InQuotes();
                case ColumnType.FLOAT:
                    break;
                case ColumnType.TIME:
                    if (columnvalue == null || ((DateTime)columnvalue) == DateTime.MinValue)
                        return String.Empty.InQuotes();
                    else
                        return ((DateTime)columnvalue).TimeOfDay.ToString().InQuotes();
                case ColumnType.DATETIME:
                    if (columnvalue == null || ((DateTime)columnvalue) == DateTime.MinValue)
                        return String.Empty.InQuotes();
                    else
                        return ((DateTime)columnvalue).ToString().InQuotes();
                    break;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns a <see cref="IEnumerable{T}"/> of <see cref="PropertyInfo"/> for all the properties in the
        /// entity that are vaild, vaild properties have the <see cref="MapInfoColumnAttribute"/> defind and are not special
        /// columns like RowID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetVaildProperties(BaseEntity entity)
        {
            return (from pro in entity.GetType().GetProperties()
                    where Attribute.IsDefined(pro, typeof (MapInfoColumnAttribute))
                          && pro.Name != "RowId"
                    select pro);
        }


        /// <summary>
        /// Generates a insert string that can be used by Mapinfo to insert the entity into the table.
        /// </summary>
        /// <param name="entity">The entity which will be used to generate the insert string.</param>
        /// <param name="tableName">The table that the enity will be inserted into.</param>
        /// <returns>A string containing the Mapbasic commands needed to insert the entity into the table.</returns>
        public string GenerateInsertString(BaseEntity entity, string tableName)
        {
            throw new NotImplementedException("This method doesn't work correctly yet");
            Guard.AgainstNull(entity, "entity");
            Guard.AgainstNullOrEmpty(tableName, "tableName");

            Dictionary<string, object> mapping = new Dictionary<string, object>();
            IEnumerable<PropertyInfo> props = GetVaildProperties(entity);

            String objdeclareString = "";
            String undimobjectvariablecommand = "";
            String objectcreateString = "";

            StringBuilder sb = new StringBuilder("INSERT INTO {0}".FormatWith(tableName));

            foreach (PropertyInfo property in props)
            {
                string Name = property.Name;

                object value = property.GetValue(entity, null);

                if (string.Equals(Name, "obj", StringComparison.InvariantCultureIgnoreCase))
                {
                    // If there is no object we can just move to the next property.
                    if (value == null) break; ;

                    objdeclareString = "Dim InsertObjectVariable as Object";
                    undimobjectvariablecommand = "UnDim InsertObjectVariable";

                    // Add the create statment for the object to the create string.
                    objectcreateString = ((Geometry)value).ToExtendedCreateString("InsertObjectVariable");

                    // Adds the mapping to assign obj the value of InsertObjectVariable.
                    mapping.Add(Name, "InsertObjectVariable");
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

            // Create the final string in the following format:
            // {Variable Declare InsertObjectVariable string}
            // {Assign InsertObjectVariable string}
            // {Insert statement string}
            // {Undim InsertObjectVariable string}
            string finalstring = "{0} \n\r {1} \n\r {2} \n\r {3}".FormatWith(objdeclareString, objectcreateString, sb.ToString(), undimobjectvariablecommand);
            return finalstring;
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
