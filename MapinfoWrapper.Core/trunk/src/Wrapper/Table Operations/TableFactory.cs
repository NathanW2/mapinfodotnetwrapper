using MapinfoWrapper.TableOperations.RowOperations.Entities;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Core.Internals;

namespace MapinfoWrapper.TableOperations
{
    /// <summary>
    /// A factory that is used to Create or Open tables in Mapinfo.
    /// </summary>
    public class TableFactory : ITableFactory
    {
        ITableCommandRunner tablerunner = IoC.Resolve<ITableCommandRunner>();

        /// <summary>
        /// Opens a new table in Mapinfo and returns the opened table.
        /// </summary>
        /// <param name="tablePath">The path to the Mapinfo tab file to open.</param>
        /// <returns>An instance of <see cref="T:MapinfoWrapper.TableOperations.ITable"/></returns>
        public ITable OpenTable(string tablePath)
        {
            string name = this.OpenTableAndGetName(tablePath);
            return new Table(name);
        }

        /// <summary>
        /// Opens a new table in Mapinfo, using the <typeparamref name="TEntity"/> as the entity type
        /// for the table and returns the opened table.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use a the entity for the table,
        /// this will allow strong typed access to the columns in the table and LINQ support.</typeparam>
        /// <param name="tablePath"></param>
        /// <returns>An instance of <see cref="T:MapinfoWrapper.TableOperations.ITable&lt;TEntity&gt;"/></returns>
        public ITable<TEntity> OpenTable<TEntity>(string tablePath)
            where TEntity : BaseEntity, new()
        {
        	string name = this.OpenTableAndGetName(tablePath);
            return new Table<TEntity>(name);
        }
        
        private string OpenTableAndGetName(string tablePath)
        {
			tablerunner.OpenTable(tablePath);
            string name = tablerunner.GetName(0);        	
        	return name;
        }
    }
}
