using System.Linq;
using MapInfo.Wrapper.Core;
using MapInfo.Wrapper.Core.Extensions;
using MapInfo.Wrapper.Core.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using MapInfo.Wrapper.DataAccess.Entities;
using MapInfo.Wrapper.Mapinfo;


namespace MapInfo.Wrapper.DataAccess
{
    /// <summary>
    /// Represents a collection of tables from a mapInfoSession.
    /// </summary>
    public class TableCollection : IEnumerable<Table>
    {
        private readonly MapInfoSession mi_session;
        private readonly TableInfoWrapper tableinfo;
        private readonly MapbasicWrapper mapbasic;
        private readonly TableFactory tablefactory;

        /// <summary>
        /// The event fired when a new table is opened in Mapinfo.
        /// </summary>
        public event EventHandler<EventArgs<ITable>> TableOpened;

        public TableCollection(MapInfoSession miSession)
        {
            this.mi_session = miSession;
            this.tableinfo = new TableInfoWrapper(miSession);
            this.mapbasic = new MapbasicWrapper(miSession);
            this.tablefactory = new TableFactory(miSession);
        }

        private void OnTableOpened(ITable table)
        {
            if (TableOpened != null)
                TableOpened(this, new EventArgs<ITable>(table));
        }

        /// <summary>
        /// Opens a new table in Mapinfo and returns the opened table.
        /// </summary>
        /// <param name="tablePath">The path to the Mapinfo tab file to open.</param>
        /// <returns>An instance of <see cref="ITable"/></returns>
        public Table OpenTable(string tablePath)
        {
            Guard.AgainstNull(tablePath, "tablePath");
            Check.CorrectExtension(tablePath, ".tab");
            Check.FileExists(tablePath);

            string name = this.OpenTableAndGetName(tablePath);
            Table tab = this.tablefactory.GetTableFor(name);
            this.OnTableOpened(tab);
            return tab;
        }

        /// <summary>
        /// Opens a new table in Mapinfo, using the <typeparamref name="TEntity"/> as the entity type
        /// for the table and returns the opened table.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use a the entity for the table,
        /// this will allow strong typed access to the columns in the table and LINQ support.</typeparam>
        /// <param name="tablePath"></param>
        /// <returns>An instance of <see cref="ITable{TTableDef}"/></returns>
        public Table<TEntity> OpenTable<TEntity>(string tablePath)
            where TEntity : BaseEntity, new()
        {
            Guard.AgainstNullOrEmpty(tablePath, "tablePath");

            Check.CorrectExtension(tablePath, ".tab");
            Check.FileExists(tablePath);

        	string name = this.OpenTableAndGetName(tablePath);
            Table<TEntity> tab = this.tablefactory.GetTableFor<TEntity>(name);
            this.OnTableOpened(tab);
            return tab;
        }

        /// <summary>
        /// Opens the table in Mapinfo and returns the name of the table that was open.
        /// </summary>
        /// <param name="tablePath"></param>
        /// <returns></returns>
        private string OpenTableAndGetName(string tablePath)
        {
            this.mi_session.Do("Open Table {0}".FormatWith(tablePath.InQuotes()));
            string name = (String)this.tableinfo.GetTableInfo(0.ToString(), TableInfo.Name);
            return name;
        }

        /// <summary>
        /// Close a collection of tables in Mapinfo.
        /// </summary>
        /// <param name="tables">A <see cref="IEnumerable&lt;ITable&gt;"/> containing the tables that need to be closed.</param>
        public void CloseTables(IEnumerable<ITable> tables)
        {
            Guard.AgainstNull(tables, "tables");

            foreach (ITable table in tables)
            {
                table.Close();
            }
        }

        /// <summary>
        /// Closes all the 
        /// </summary>
        public void CloseAll()
        {
            this.mi_session.Do("Close All");
        }

        /// <summary>
        /// Returns a table object from the table collection using the supplied name.
        /// </summary>
        /// <param name="tableName">The name of the table to return from the list.</param>
        /// <returns>The table object return from the table collection.</returns>
        public Table this[string tableName]
        {
            get
            {
                // HACK! This really needs to check for active selection before just returning.
                if (tableName.ToUpper() == "SELECTION")
                    return this.tablefactory.GetTableFor("Selection");

                Table table = this.Where(tab => tab.Name == tableName)
                                                 .FirstOrDefault();

                // If we found the table on our first pass through then just return the table.
                if (table != null)
                {
                    return table;
                }

                return this[tableName];
            }
        }

        public Table GetTable(string tableName)
        {
            Guard.AgainstNullOrEmpty(tableName, "tableName");

            return this[tableName];
        }

        public Table<T> GetTable<T>(string tableName)
            where T : BaseEntity, new()
        {
            return this.GetTable(tableName).ToGenericTable<T>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Table> GetEnumerator()
        {
            // Get the number of tables.
            // Loop open tables and add to list.
            int numtables = mapbasic.GetNumberOfOpenTables();

            for (int i = 1; i <= numtables; i++)
            {
                string tableName = tableinfo.GetName(i);
                Table tab = this.tablefactory.GetTableFor(tableName);
                yield return tab;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
