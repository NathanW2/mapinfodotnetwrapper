using System;
using System.Linq;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.DataAccess.RowOperations;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.DataAccess.LINQ;

namespace MapinfoWrapper.Core.IoC
{
    // HACK This feels like a bit of a code smell need to
    // look into better ways to build objects rather then a large 
    internal class ObjectBuilder : IObjectBuilder
    {
        private readonly MapinfoSession wrapper;

        public ObjectBuilder(MapinfoSession mapinfoInstance)
        {
            this.wrapper = mapinfoInstance;
        }

        public IGeometryFactory BuildGeomtryFactory()
        {
            return new GeometryFactory(this.wrapper, this.BuildVariableFactory());
        }

        public IVariableFactory BuildVariableFactory()
        {
            return new VariableFactory(this.wrapper);
        }

        public IDataReader BuildDataReader(string tableName)
        {
            DataReader reader = new DataReader(this.wrapper,tableName,this.BuildGeomtryFactory());
            return reader;
        }

        public IQueryProvider BuildQueryProvider()
        {
            return new MapinfoQueryProvider(this.wrapper);
        }
        
        [Obsolete]
        public ITable<TEntity> BuildTable<TEntity>(string name)
            where TEntity : BaseEntity, new()
        {
            throw new NotImplementedException("This feature needs to be removed");
            //IQueryProvider provider = this.BuildQueryProvider();
            //IDataReader reader = this.BuildDataReader(name);
            //return new Table<TEntity>(this.wrapper, provider, reader, name);
        }

        public ITable BuildTable(string name)
        {
            return (Table) this.BuildTable<BaseEntity>(name);
        }
    }
}
