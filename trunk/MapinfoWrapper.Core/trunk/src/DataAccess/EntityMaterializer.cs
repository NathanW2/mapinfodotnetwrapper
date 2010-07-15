using System;
using System.Collections.Generic;
using System.Linq;
using MapInfo.Wrapper.DataAccess.Entities;
using MapInfo.Wrapper.DataAccess.Row;
using MapInfo.Wrapper.Mapinfo;
using System.Reflection;

namespace MapInfo.Wrapper.DataAccess
{
    class ColumnMapping
    {
        public string ColumnName { get; set; }
        public object Data { get; set; }
    }

    /// <summary>
    /// Materializer used to create and populate entity objects.
    /// </summary>
    public class EntityMaterializer
    {
        private enum BackingStoreOptions
        {
            Load,
            NoStore
        }

        private readonly IMapInfoDataReader datareader;
        private readonly string table_name;
        private Dictionary<Type, PropertyInfo[]> CachedMappings = new Dictionary<Type, PropertyInfo[]>();

        public EntityMaterializer(MapInfoSession miSession, ITable table, IMapInfoDataReader mapInfoDataReader)
        {
            this.MapifoSession = miSession;
            this.datareader = mapInfoDataReader;
            this.Table = table;
        }

        public MapInfoSession MapifoSession { get; private set; }

        private ITable Table { get; set; }

        public TEntity GenerateEntityForIndex<TEntity>(int index)
            where TEntity : BaseEntity, new()
        {
            if (this.datareader.CurrentRecord != index)
            {
                this.datareader.Fetch(index);
            }

            TEntity entity = new TEntity();
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
            List<ColumnMapping> columndatamap = new List<ColumnMapping>();
            if (options == BackingStoreOptions.Load)
            {
                foreach (var column in this.Table.Columns)
                {
                    ColumnMapping mapping = new ColumnMapping();
                    mapping.ColumnName = column.Name;
                    mapping.Data = this.datareader.Get(column.Name);
                    columndatamap.Add(mapping);
                }

                obj.BackingStore = columndatamap;
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
                    data = columndatamap.First(col => col.ColumnName.ToLower() == fi.Name.ToLower()).Data;
                }
                data = this.datareader.Get(fi.Name);
                fi.SetValue(obj, data, null);
            }

            obj.State = BaseEntity.EntityState.PossiblyModifed;

            return obj;
        }
    }
}
