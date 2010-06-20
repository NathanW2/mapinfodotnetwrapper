using Mapinfo.Wrapper.Core;
using Mapinfo.Wrapper.Core.Extensions;
using Mapinfo.Wrapper.DataAccess.Row.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.DataAccess.LINQ.SQL
{
    public abstract class Query
    {
        public abstract void Excute();
        public abstract string GetQueryString();
    }

    public class UpdateQuery : Query
    {
        private MapinfoSession mapinfo;

        public UpdateQuery(MapinfoSession session, string tableName)
        {
            Guard.AgainstNull(session, "session");
            Guard.AgainstNullOrEmpty(tableName, "tableName");

            this.mapinfo = session;
            this.TableName = tableName;
        }

        private Dictionary<string, object> mappings = new Dictionary<string, object>();
        public Dictionary<string, object> ColumnValueMapping
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
            if (this.ColumnValueMapping.Count <= 0)
                return "";

            StringBuilder updatestring = new StringBuilder("UPDATE {0} SET ".FormatWith(this.TableName));
            string wherestring = "";

            foreach (var item in this.ColumnValueMapping)
            {
                object resultvalue = String.Empty;
                object data = item.Value;

                switch (data.GetType().Name)
                {
                    case "String":
                        resultvalue = ((string)data).InQuotes();
                        break;
                    case "Int16":
                    case "Int32":
                        resultvalue = Convert.ToInt32(data);
                        break;
                    case "DateTime":
                        resultvalue = ((DateTime)data).ToString().InQuotes();
                        break;
                }

                if (string.Equals(item.Key, "rowid", StringComparison.InvariantCultureIgnoreCase))
                {
                    wherestring = " WHERE RowID = {0}".FormatWith(data);
                    continue;
                }

                updatestring.AppendFormat("{0} = {1},", item.Key, resultvalue);

            }
            return updatestring.ToString().TrimEnd(',') + wherestring;
        }
    }


    public class SqlStringGenerator
    {
        private MapinfoSession mapinfo;

        public SqlStringGenerator(MapinfoSession session)
        {
            this.mapinfo = session;
        }

        public Query GenerateUpdateQuery(BaseEntity entity, string tableName)
        {
            Guard.AgainstNull(entity, "entity");
            Guard.AgainstNullOrEmpty(tableName, "tableName");

            List<PropertyInfo> props = (from pro in entity.GetType().GetProperties()
                                        where !Attribute.IsDefined(pro, typeof(MapinfoIgnore))
                                        select pro).ToList();

            UpdateQuery query = new UpdateQuery(this.mapinfo, tableName);

            foreach (PropertyInfo property in props)
            {
                string name = property.Name;
                object value = property.GetValue(entity, null);

                if (value == null)
                    value = String.Empty;

                query.ColumnValueMapping.Add(name, value);
            }
            return query;
        }


        /// <summary>
        /// Generates a insert string that can be used by Mapinfo to insert the entity into the table.
        /// </summary>
        /// <param name="entity">The entity which will be used to generate the insert string.</param>
        /// <param name="tableName">The table that the enity will be inserted into.</param>
        /// <returns>A string containing the Mapbasic commands needed to insert the entity into the table.</returns>
        public string GenerateInsertString(BaseEntity entity, string tableName)
        {
            Guard.AgainstNull(entity, "entity");
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
