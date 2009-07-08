using System;
using System.Collections.Generic;
using MapinfoWrapper.Core;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.DataAccess
{
    /// <summary>
    /// A factory that is used to Create or Open tables in Mapinfo.
    /// </summary>
    public class TableManager : ITableManager
    {
        private readonly MapinfoSession miSession;
        internal readonly IObjectBuilder builder;

        public TableManager(MapinfoSession MISession)
        {
            this.miSession = MISession;
            this.builder = new ObjectBuilder(MISession);
        }

        internal TableManager(MapinfoSession MISession,IObjectBuilder builder)
        {
            this.miSession = MISession;
            this.builder = builder;
        }

        /// <summary>
        /// Opens a new table in Mapinfo and returns the opened table.
        /// </summary>
        /// <param name="tablePath">The path to the Mapinfo tab file to open.</param>
        /// <returns>An instance of <see cref="MapinfoWrapper.DataAccess.ITable"/></returns>
        public Table OpenTable(string tablePath)
        {
            string name = this.OpenTableAndGetName(tablePath);
            return this.GetTable(name);
        }

        /// <summary>
        /// Opens a new table in Mapinfo, using the <typeparamref name="TEntity"/> as the entity type
        /// for the table and returns the opened table.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use a the entity for the table,
        /// this will allow strong typed access to the columns in the table and LINQ support.</typeparam>
        /// <param name="tablePath"></param>
        /// <returns>An instance of <see cref="MapinfoWrapper.TableOperations.ITable&lt;TEntity&gt;"/></returns>
        public Table<TEntity> OpenTable<TEntity>(string tablePath)
            where TEntity : BaseEntity, new()
        {
        	string name = this.OpenTableAndGetName(tablePath);
            return this.GetTable<TEntity>(name);
        }

        /// <summary>
        /// Gets an already open table in Mapinfo.
        /// </summary>
        /// <typeparam name="TEntity">The entity object to use as the tables entity type.</typeparam>
        /// <param name="tableName">The name of the table to get from Mapinfo.</param>
        /// <returns></returns>
        public Table<TEntity> GetTable<TEntity>(string tableName) 
            where TEntity : BaseEntity, new()
        {
            Guard.AgainstNullOrEmpty(tableName, "tableName");
            // TODO Add logic here to handle if table isn't open.
            return (Table<TEntity>)this.builder.BuildTable<TEntity>(tableName);
        }

        public Table GetTable(string tableName)
        {
            return this.GetTable<BaseEntity>(tableName);
        }

        /// <summary>
        /// Close a collection of tables in Mapinfo.
        /// </summary>
        /// <param name="tables">A <see cref="IEnumerable&lt;ITable&gt;"/> containing the tables that need to be closed.</param>
        public static void CloseTables(IEnumerable<Table> tables)
        {
            foreach (ITable table in tables)
            {
                table.Close();
            }
        }

        private string OpenTableAndGetName(string tablePath)
        {
            this.miSession.RunCommand("Open Table {0}".FormatWith(tablePath.InQuotes()));
            string name = (String)this.miSession.RunTableInfo(0.ToString(), TableInfo.Name);        	
        	return name;
        }
    }
}
