namespace MapinfoWrapper.DataAccess.LINQ.SQLBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Geometries;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.Core;

    public class SqlStringGenerator
    {
        /// <summary>
        /// Generates a insert string that can be used by Mapinfo to insert the entity into the table.
        /// </summary>
        /// <param name="entity">The entity which will be used to generate the insert string.</param>
        /// <param name="tableName">The table that the enity will be inserted into.</param>
        /// <returns>A string containing the Mapbasic commands needed to insert the entity into the table.</returns>
        public string GenerateInsertString(BaseEntity entity, string tableName)
        {
            Guard.AgainstNull(entity,"entity");
            Guard.AgainstNullOrEmpty(tableName, "tableName");

            Dictionary<string, object> mapping = new Dictionary<string, object>();
            PropertyInfo[] props = entity.GetType().GetProperties();

            String objdeclareString = "";
            String undimobjectvariablecommand = "";
            String objectcreateString = "";

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
            string finalstring =  "{0} \n\r {1} \n\r {2} \n\r {3}".FormatWith(objdeclareString, objectcreateString, sb.ToString(), undimobjectvariablecommand);
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
