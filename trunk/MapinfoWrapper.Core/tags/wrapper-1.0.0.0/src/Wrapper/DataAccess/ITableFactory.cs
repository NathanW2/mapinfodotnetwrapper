using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.DataAccess.RowOperations.Entities;

namespace MapinfoWrapper.DataAccess
{
    internal interface ITableFactory
    {
        /// <summary>
        /// Opens the supplied table in Mapinfo
        /// </summary>
        /// <param name="tablePath"></param>
        /// <returns></returns>
        ITable OpenTable(string tablePath);

        ITable<T> OpenTable<T>(string tablePath)
            where T : BaseEntity, new();

        ITable BuildTable(string tableName);

        ITable<TEntity> BuildTable<TEntity>(string tableName)
            where TEntity : BaseEntity, new();
    }
}