using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.DataAccess.RowOperations;
using MapinfoWrapper.Wrapper.Geometries;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.DataAccess.LINQ;

namespace MapinfoWrapper.Core.IoC
{
    internal class FactoryBuilder : IFactoryBuilder
    {
        private readonly IMapinfoWrapper wrapper;

        public FactoryBuilder(IMapinfoWrapper mapinfoInstance)
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

        public ITableFactory BuildTableFactory()
        {
            ITableCommandRunner tablerunner = new TableCommandRunner(this.wrapper);
            IQueryProvider provider = new MapinfoQueryProvider(this.wrapper);
            ITableFactory tableFactory = new TableFactory(tablerunner, provider);
            return tableFactory;
        }

        public IDataReader BuildDataReader(string tableName)
        {
            DataReader reader = new DataReader(this.wrapper,tableName,this.BuildGeomtryFactory());
            return reader;
        }
    }
}
