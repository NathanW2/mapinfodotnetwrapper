using System.Linq;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.DataAccess.RowOperations;
using MapinfoWrapper.Geometries;

namespace MapinfoWrapper.Core.IoC
{
    internal interface IObjectBuilder
    {
        IGeometryFactory BuildGeomtryFactory();
        IVariableFactory BuildVariableFactory();
        IDataReader BuildDataReader(string tableName);
        IQueryProvider BuildQueryProvider();

        ITable<TEntity> BuildTable<TEntity>(string name)
            where TEntity : BaseEntity, new();

        ITable BuildTable(string name);
    }
}
