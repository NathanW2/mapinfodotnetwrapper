namespace MapinfoWrapper.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using MapinfoWrapper.Core;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Core.InfoWrappers;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.Mapinfo;

    /// <summary>
    /// A factory that is used to CreateInto or Open tables in Mapinfo.
    /// </summary>
    public class TableCollection : IEnumerable<Table>
    {
        private readonly MapinfoSession miSession;
        private readonly TableInfoWrapper tableinfo;
        private readonly MapbasicWrapper mapbasic;
        private readonly List<Table> innertablelist;

        public delegate void TableEvent(Table table);

        /// <summary>
        /// The event fired when a new table is opened in Mapinfo.
        /// </summary>
        public event TableEvent TableOpened;

        internal TableCollection(MapinfoSession MISession)
        {
            this.miSession = MISession;
            this.tableinfo = new TableInfoWrapper(MISession);
            this.innertablelist = new List<Table>();
            this.mapbasic = new MapbasicWrapper(MISession);
        }

        /// <summary>
        /// Opens a new table in Mapinfo and returns the opened table.
        /// </summary>
        /// <param name="tablePath">The path to the Mapinfo tab file to open.</param>
        /// <returns>An instance of <see cref="MapinfoWrapper.DataAccess.ITable"/></returns>
        public Table OpenTable(string tablePath)
        {
            Guard.AgainstNull(tablePath, "tablePath");

            Check.CorrectExtension(tablePath, ".tab");
            Check.FileExists(tablePath);

            string name = this.OpenTableAndGetName(tablePath);
            Table tab = new Table(miSession, name);
            this.RefreshList();
            if (TableOpened != null)
            {
                TableOpened(tab);
            }
            return tab;
        }

        /// <summary>
        /// Opens a new table in Mapinfo, using the <typeparamref name="TEntity"/> as the entity type
        /// for the table and returns the opened table.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use a the entity for the table,
        /// this will allow strong typed access to the columns in the table and LINQ support.</typeparam>
        /// <param name="tablePath"></param>
        /// <returns>An instance of <see cref="MapinfoWrapper.DataAccess.ITable&lt;TEntity&gt;"/></returns>
        public Table<TEntity> OpenTable<TEntity>(string tablePath)
            where TEntity : BaseEntity, new()
        {
            Guard.AgainstNullOrEmpty(tablePath, "tablePath");

            Check.CorrectExtension(tablePath, ".tab");
            Check.FileExists(tablePath);

        	string name = this.OpenTableAndGetName(tablePath);
            Table<TEntity> tab = new Table<TEntity>(miSession,name);
            this.RefreshList();
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
            this.miSession.RunCommand("Open Table {0}".FormatWith(tablePath.InQuotes()));
            string name = (String)this.tableinfo.GetTableInfo(0.ToString(), TableInfo.Name);
            return name;
        }

        /// <summary>
        /// Close a collection of tables in Mapinfo.
        /// </summary>
        /// <param name="tables">A <see cref="IEnumerable&lt;ITable&gt;"/> containing the tables that need to be closed.</param>
        public void CloseTables(IEnumerable<Table> tables)
        {
            Guard.AgainstNull(tables, "tables");

            foreach (ITable table in tables)
            {
                table.Close();
            }
            this.CloseAll();
        }

        /// <summary>
        /// Closes all the 
        /// </summary>
        public void CloseAll()
        {
            this.miSession.RunCommand("Close All");
            this.RefreshList();
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
                    return new Table(miSession, "Selection");

                return this.innertablelist.Where(tab => tab.Name == tableName)
                                          .FirstOrDefault();
            }
        }


        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Table> GetEnumerator()
        {
            return this.innertablelist.GetEnumerator();
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

        #endregion

        /// <summary>
        /// Refeshss the list off tables.  This is called automaticlly from the OpenTables commands and OpenWorkspace.
        /// You should call this if the number of tables has changed from outside of the Mapinfo OLE Wrapper.
        /// </summary>
        public void RefreshList()
        {
            this.innertablelist.Clear();

            // Get the number of tables.
            // Loop open tables and add to list.
            int numtables = mapbasic.GetNumberOfOpenTables();

            if (numtables == 0) return;

            for (int i = 1; i <= numtables; i++)
            {
                string tableName = tableinfo.GetName(i);
                Table tab = new Table(miSession,tableName);
                this.innertablelist.Add(tab);
            }
        }
    }
}
