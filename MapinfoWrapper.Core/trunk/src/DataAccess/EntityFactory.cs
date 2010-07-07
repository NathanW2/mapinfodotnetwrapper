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
    class EntityMaterializer
    {
        private enum BackingStoreOptions
        {
            Load,
            NoStore
        }

        private readonly IDataReader datareader;
        private readonly string TableName;
        private Dictionary<Type, PropertyInfo[]> CachedMappings = new Dictionary<Type, PropertyInfo[]>();

        public EntityMaterializer(MapinfoSession MISession, string tableName, IDataReader dataReader)
        {
            this.MapifoSession = MISession;
            this.datareader = dataReader;
            this.TableName = tableName;
        }

        public MapinfoSession MapifoSession { get; private set; }

        private ITable Table { get; set; }

        public TEntity GenerateEntityForIndex<TEntity>(int index)
            where TEntity : BaseEntity, new()
        {
            if (this.datareader.CurrentRecord != index)
            {
                this.datareader.Fetch(index);
            }

            TEntity entity = new TEntity();
            //TODO We might need to look into getting the table a little better.
            this.Table = this.MapifoSession.Tables.GetTable<TEntity>(this.TableName);
            entity.Table = this.Table;

            BackingStoreOptions options = BackingStoreOptions.Load;
            //HACK We could proberly do this a bit better, maybe pass the loadoptions down rather then reaching for them.
            if (this.MapifoSession.LoadOptions.HasNoBackingStore.Contains(typeof(TEntity)))
            {
                options = BackingStoreOptions.NoStore;
            }
            return this.PopulateEntity(entity, options);
        }

        private TEntity PopulateEntity<TEntity>(TEntity obj, BackingStoreOptions options)
            where TEntity: BaseEntity, new()
        {
            List<ColumnMapping> ColumnDataMap = new List<ColumnMapping>();
            if (options == BackingStoreOptions.Load)
            {
                foreach (var column in this.Table.Columns)
                {
                    ColumnMapping mapping = new ColumnMapping();
                    mapping.ColumnName = column.Name;
                    mapping.Data = this.datareader.Get(column.Name);
                    ColumnDataMap.Add(mapping);
                }

                obj.BackingStore = ColumnDataMap;
            }

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
                object data = null;
                if (options == BackingStoreOptions.Load)
                {
                    data = ColumnDataMap.First(col => col.ColumnName.ToLower() == fi.Name.ToLower()).Data;
                }
                data = this.datareader.Get(fi.Name);
                fi.SetValue(obj, data, null);
            }

            obj.State = BaseEntity.EntityState.PossiblyModifed;

            return obj;
        }
    }
}
