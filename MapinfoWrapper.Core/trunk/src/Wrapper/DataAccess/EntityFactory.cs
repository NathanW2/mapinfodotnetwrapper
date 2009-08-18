namespace MapinfoWrapper.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MapinfoWrapper.Mapinfo;
    using MapinfoWrapper.DataAccess.RowOperations;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using System.Reflection;

    class ColumnMapping
    {
        public string ColumnName { get; set; }
        public object Data { get; set; }
    }

    /// <summary>
    /// Factory used to create and populate entity objects.
    /// </summary>
    class EntityFactory
    {
        private readonly DataReader datareader;
        private Dictionary<Type, PropertyInfo[]> CachedMappings = new Dictionary<Type, PropertyInfo[]>();

        public EntityFactory(MapinfoSession MISession, Table table)
        {
            this.MapifoSession = MISession;
            this.Table = table;
            this.datareader = new DataReader(MISession, table.Name);
        }

        public MapinfoSession MapifoSession { get; private set; }
        public Table Table { get; private set; }
        
        public TEntity GenerateEntityForIndex<TEntity>(int index)
            where TEntity : BaseEntity, new()
        {
            string name = this.Table.Name;
            if (this.datareader.CurrentRecord != index)
            {
                this.datareader.Fetch(index);
            }

            TEntity entity = new TEntity();
            entity.Table = this.Table;
            return this.PopulateEntity(entity);
        }

        public TEntity PopulateEntity<TEntity>(TEntity obj)
            where TEntity: BaseEntity, new()
        {
            List<ColumnMapping> ColumnDataMap = new List<ColumnMapping>();
            foreach (var column in this.Table.Columns)
            {
               ColumnMapping mapping = new ColumnMapping();
               mapping.ColumnName = column.Name;
               mapping.Data = this.datareader.Get(column.Name);
               ColumnDataMap.Add(mapping);
            }

            obj.BackingStore = ColumnDataMap;

            PropertyInfo[] properties;
            var found = this.CachedMappings.TryGetValue(typeof(TEntity), out properties);

            if (!found)
            {
                properties = (from prop in typeof(TEntity).GetProperties()
                              where !prop.GetCustomAttributes(false).Any(att => att.GetType() == typeof(MapinfoIgnore))
                              select prop).ToArray();

                this.CachedMappings.Add(typeof(TEntity), properties);
            }

            for (int i = 0, n = properties.Length; i < n; i++)
            {
                PropertyInfo fi = properties[i];
                object data = ColumnDataMap.First(col => col.ColumnName.ToLower() == fi.Name.ToLower()).Data;
                fi.SetValue(obj, data, null);
            }

            return obj;
        }
    }
}
