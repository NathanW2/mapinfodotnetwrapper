using MapinfoWrapper.DataAccess;
using MapinfoWrapper.Wrapper.Geometries;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.DataAccess.RowOperations;
namespace MapinfoWrapper.Core.IoC
{
    internal interface IFactoryBuilder
    {
        IGeometryFactory BuildGeomtryFactory();
        IVariableFactory BuildVariableFactory();
        ITableFactory BuildTableFactory();
        IDataReader BuildDataReader(string tableName);
    }
}
