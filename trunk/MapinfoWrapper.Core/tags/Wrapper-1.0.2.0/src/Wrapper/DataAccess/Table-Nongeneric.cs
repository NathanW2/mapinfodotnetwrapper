using System.Collections.Generic;
using System.Linq;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.Core.IoC;

namespace MapinfoWrapper.DataAccess
{
    /// <summary>
    /// A Mapinfo Table container which can be used when the
    /// table definition is not known. This will still give you strong access
    /// to the RowId column.
    /// </summary>
    public class Table : Table<BaseEntity>
    {
        private readonly static IFactoryBuilder depFactory = ServiceLocator.GetInstance<IFactoryBuilder>();
        private readonly static ITableFactory tableFactory = depFactory.BuildTableFactory();

        internal Table(ITableCommandRunner tableRunner, IQueryProvider queryProvider, string tableName)
            : base(tableRunner, queryProvider, tableName)
        { }

        /// <summary>
        /// Opens a new table in Mapinfo and returns the opened table.
        /// </summary>
        /// <param name="path">The path to the Mapinfo tab file to open.</param>
        /// <returns>An instance of <see cref="T:MapinfoWrapper.DataAccess.ITable"/></returns>
        public static ITable OpenTable(string path)
        {
            return tableFactory.OpenTable(path);
        }

        /// <summary>
        /// Opens a new table in Mapinfo, using the <typeparamref name="TEntity"/> as the entity type
        /// for the table and returns the opened table.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use a the entity for the table,
        /// this will allow strong typed access to the columns in the table and LINQ support.</typeparam>
        /// <param name="path"></param>
        /// <returns>An instance of <see cref="T:MapinfoWrapper.TableOperations.ITable&lt;TEntity&gt;"/></returns>
        public static ITable<TEntity> OpenTable<TEntity>(string path)
            where TEntity : BaseEntity, new()
        {
            return tableFactory.OpenTable<TEntity>(path);
        }

        /// <summary>
        /// Gets an already open table in Mapinfo.
        /// </summary>
        /// <typeparam name="TEntity">The entity object to use as the tables entity type.</typeparam>
        /// <param name="tableName">The name of the table to get from Mapinfo.</param>
        /// <returns></returns>
        public static ITable<TEntity> GetTable<TEntity>(string tableName)
            where TEntity : BaseEntity, new()
        {
            return tableFactory.BuildTable<TEntity>(tableName);
        }

        /// <summary>
        /// Close a collection of tables in Mapinfo.
        /// </summary>
        /// <param name="tables">A <see cref="T:IEnumerable&lt;ITable&gt;"/> containing the tables that need to be closed.</param>
        public static void CloseTables(IEnumerable<ITable> tables)
        {
            foreach (ITable table in tables)
            {
                Table.CloseTable(table);
            }
        }

        /// <summary>
        /// Close the supplied table in Mapinfo.
        /// </summary>
        /// <param name="table">The table that will be closed.</param>
        public static void CloseTable(ITable table)
        {
            table.Close();
        }
    }
}


