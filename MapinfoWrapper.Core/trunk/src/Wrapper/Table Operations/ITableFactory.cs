using MapinfoWrapper.TableOperations.RowOperations.Entities;

namespace MapinfoWrapper.TableOperations
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
    }
}