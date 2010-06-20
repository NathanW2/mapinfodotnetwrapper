using Mapinfo.Wrapper.Core;
using Mapinfo.Wrapper.Core.Extensions;
using Mapinfo.Wrapper.Core.Wrappers;
using Mapinfo.Wrapper.DataAccess.Row.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.DataAccess
{
    /// <summary>
    /// Represents the collection of open tables in the supplied MapInfo session.
    /// </summary>
    public class TableCollection : IEnumerable<Table>
    {
        private readonly MapinfoSession miSession;
        private readonly TableInfoWrapper tableinfo;
        private readonly MapbasicWrapper mapbasic;
        private readonly TableFactory tablefactory;

        public delegate void TableEvent(ITable table);

        /// <summary>
        /// The event fired when a new table is opened in Mapinfo.
        /// </summary>
        public event TableEvent TableOpened;

        internal TableCollection(MapinfoSession MISession)
        {
            this.miSession = MISession;
            this.tableinfo = new TableInfoWrapper(MISession);
            this.mapbasic = new MapbasicWrapper(MISession);
            this.tablefactory = new TableFactory(miSession);
        }

        /// <summary>
        /// Opens a new table in MapInfo and returns the opened table.
        /// </summary>
        /// <param name="tablePath">The path to the Mapinfo tab file to open.</param>
        /// <returns>An instance of <see cref="ITable"/></returns>
        public Table Open(string tablePath)
        {
            return (Table)this.Open<BaseEntity>(tablePath);
        }

        /// <summary>
        /// Opens a new table in Mapinfo, using the <typeparamref name="TEntity"/> as the entity type
        /// for the table and returns the opened table.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use a the entity for the table,
        /// this will allow strong typed access to the columns in the table and LINQ support.</typeparam>
        /// <param name="tablePath"></param>
        /// <returns>An instance of <see cref="ITable{TTableDef}"/></returns>
        public Table<TEntity> Open<TEntity>(string tablePath)
            where TEntity : BaseEntity, new()
        {
            Guard.AgainstNullOrEmpty(tablePath, "tablePath");

            Check.CorrectExtension(tablePath, ".tab");
            Check.FileExists(tablePath);

        	string name = this.OpenTableAndGetName(tablePath);
            Table<TEntity> tab = this.tablefactory.GetTableFor<TEntity>(name);
            if (TableOpened != null)
            {
                TableOpened(tab);
            }
            return tab;
        }

        /// <summary>
        /// Opens the table in Mapinfo and returns the name of the table that was open.
        /// </summary>
        /// <param name="tablePath"></param>
        /// <returns></returns>
        private string OpenTableAndGetName(string tablePath)
        {
            this.miSession.Do("Open Table {0}".FormatWith(tablePath.InQuotes()));
            string name = (String)this.tableinfo.GetName(0);
            return name;
        }

        /// <summary>
        /// Closes all the tables in the current session of Mapinfo.
        /// </summary>
        public void CloseAll()
        {
            this.miSession.Do("Close All");
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

                return this.Where(tab => tab.Name == tableName)
                           .FirstOrDefault();
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
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Table> GetEnumerator()
        {
            // Get the number of tables.
            int numtables = mapbasic.GetNumberOfOpenTables();

            for (int i = 1; i <= numtables; i++)
            {
                string tableName = tableinfo.GetName(i);
                yield return this.tablefactory.GetTableFor(tableName);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
