using MapinfoWrapper.DataAccess.RowOperations.Entities;

namespace MapinfoWrapper.DataAccess
{
    internal interface ITableManager
    {
        /// <summary>
        /// Opens a new table in Mapinfo and returns the opened table.
        /// </summary>
        /// <param name="tablePath">The path to the Mapinfo tab file to open.</param>
        /// <returns>An instance of <see cref="MapinfoWrapper.DataAccess.ITable"/></returns>
        Table OpenTable(string tablePath);

        /// <summary>
        /// Opens a new table in Mapinfo, using the <typeparamref name="TEntity"/> as the entity type
        /// for the table and returns the opened table.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use a the entity for the table,
        /// this will allow strong typed access to the columns in the table and LINQ support.</typeparam>
        /// <param name="tablePath"></param>
        /// <returns>An instance of <see cref="MapinfoWrapper.TableOperations.ITable&lt;TEntity&gt;"/></returns>
        Table<TEntity> OpenTable<TEntity>(string tablePath)
            where TEntity : BaseEntity, new();

        /// <summary>
        /// Gets an already open table in Mapinfo.
        /// </summary>
        /// <typeparam name="TEntity">The entity object to use as the tables entity type.</typeparam>
        /// <param name="tableName">The name of the table to get from Mapinfo.</param>
        /// <returns></returns>
        Table<TEntity> GetTable<TEntity>(string tableName) 
            where TEntity : BaseEntity, new();

        Table GetTable(string tableName);
    }
}