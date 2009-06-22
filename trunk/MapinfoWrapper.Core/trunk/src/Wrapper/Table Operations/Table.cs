using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapinfoWrapper.Core;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.TableOperations.LINQ;
using MapinfoWrapper.TableOperations.LINQ.SQLBuilders;
using MapinfoWrapper.TableOperations.RowOperations;
using MapinfoWrapper.TableOperations.RowOperations.Entities;
using MapinfoWrapper.TableOperations.RowOperations.Enumerators;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Core.Internals;

namespace MapinfoWrapper.TableOperations
{
	/// <summary>
	/// A Mapinfo Table container which can be used to hold a referance
	/// and access properties on a already open table
	/// <para>A table definition can also be used to get strong typed access to column names.</para>
	/// <para>If strong typed access isn't need consider using <see cref="T:Table"/> instead.</para>
	/// </summary>
	/// <typeparam name="TTableDef">The table definition to use with the table to allow for strong typed access to the column names.</typeparam>
	public class Table<TTableDef> : ITable<TTableDef>
        where TTableDef : BaseEntity ,new()
	{
		private readonly string internalname;
		private readonly IMapinfoWrapper wrapper;
	    private readonly ITableCommandRunner tablecommandrunner = IoC.Resolve<ITableCommandRunner>();

		internal Table(string tableName)
			: this(IoC.Resolve<IMapinfoWrapper>(), tableName)
		{ }

		/// <summary>
		/// Creates a new Table object used to gather information
		/// about a specifed table.
		/// </summary>
		/// <param name="wrapper">A wrapper object containing the running instace of mapinfo.</param>
		/// <param name="tablename">The name of the table for which to gather information about.</param>
        internal Table(IMapinfoWrapper wrapper, string tablename)
		{
			this.wrapper = wrapper;
			this.internalname = tablename;
			this.Provider = new MapinfoQueryProvider(this.wrapper);
		}
		
		/// <summary>
		/// Returns a <see cref="RowList&lt;TEntity&gt;"/> using the TEntity supplied with the
		/// table which can provide strongly typed access to column names.
		/// </summary>
		public IEnumerable<TTableDef> Rows
		{
			get
			{
				return new RowList<TTableDef>(this.wrapper, this.Name);
			}
		}

		/// <summary>
		/// Inserts a new row into the table.
		/// </summary>
		/// <param name="newRow"></param>
		public TTableDef InsertRow(TTableDef newRow)
		{
            Guard.AgainstNull(newRow, "newRow");
            SqlStringGenerator sqlstringgen = new SqlStringGenerator();

            string insertstring = sqlstringgen.GenerateInsertString(newRow, this.Name);

            this.wrapper.RunCommand(insertstring);
            DataReader reader = new DataReader(this.Name);
            reader.FetchLast();
            TTableDef row = new TTableDef();
            row.RowId = Convert.ToInt32(reader.Get("rowId"));
            return row;
		}

        /// <summary>
		/// Deletes a row from the table at the specified index.
		/// </summary>
		/// <param name="index"></param>
		public void DeleteRowAt(int index)
		{
            Guard.AgainstIsZero(index,"index");

			this.wrapper.RunCommand("Delete From {0} Where Rowid = {1}".FormatWith(this.Name, index));
		}

		/// <summary>
		/// Returns a <see cref="T:TEntity"/> at the supplied index in the table.
		/// </summary>
		/// <param name="index">The index at which to get the <see cref="T:TEntity"/></param>
		/// <returns>An instace of <see cref="T:TEntity"/> for the supplied index.</returns>
		public TTableDef this[int index]
		{
			get
			{
                TTableDef row = new TTableDef();
                row.RowId = index;
				return row;
			}

		}

		/// <summary>
		/// Deletes the selected row from the current table.
		/// </summary>
		/// <param name="entity">The row that will be deleted.</param>
		/// <exception cref="T:ArgumentNullException"/>
		public void DeleteRow(TTableDef entity)
		{
			Guard.AgainstNull(entity, "entity");
            Guard.AgainstNotInserted(entity, "entity");

			this.DeleteRowAt(entity.RowId);
		}

        private string name;
		/// <summary>
		/// Returns the name of the current table.
		/// </summary>
        public virtual string Name
        {
            get
            {
                if (this.name == null)
                {
                    name = this.tablecommandrunner.GetName(this.internalname);
                }
                return name;
            }
        }

		/// <summary>
		/// Returns the path of the tab file for the underlying table,
		/// if the table is a query table it will retrun null.
		/// </summary>
		public System.IO.DirectoryInfo TablePath
		{
			get
			{
			    string returnValue = this.tablecommandrunner.GetPath(this.Name);

				if (string.IsNullOrEmpty(returnValue))
					return null;
				else
					return new System.IO.DirectoryInfo(returnValue);
			}
		}

		/// <summary>
		/// Returns the number of the current table.
		/// </summary>
		public int Number
		{
			get
			{
			    string value = (String)this.tablecommandrunner.RunTableInfo(this.internalname, TableInfo.Number);
			    return Convert.ToInt32(value);
			}
		}

		/// <summary>
		/// Returns true if the table is mappable.
		/// If the table is mappable it may be passed to the <see cref="T:Map.MapTable"/> command.
		/// </summary>
		public bool IsMappable
		{
			get
			{
			    string value = (string)this.tablecommandrunner.RunTableInfo(this.internalname, TableInfo.Mappable);
                return (value == "TEntity");
			}
		}

		/// <summary>
		/// Returns the number of columns in the current table.
		/// </summary>
		/// <returns>The number of columns in the current table.</returns>
		public int GetNumberOfColumns()
		{
            string value = (string)this.tablecommandrunner.RunTableInfo(this.internalname, TableInfo.Ncols);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Commits any pending changes for the current table.
		/// </summary>
		public void SaveChanges()
		{
			this.wrapper.RunCommand("Commit Table {0}".FormatWith(this.Name));
		}

		/// <summary>
		/// Commits any pending changes for the current table.
		/// </summary>
		/// <param name="interactive">If set to true in the event of a conflict, MapInfo Professional displays the Conflict Resolution dialog box.
		/// <para>After a successful Commit Table Interactive statement, MapInfo Professional displays a dialog box allowing the user to refresh.</para>
		/// </param>
		public void SaveChanges(bool interactive)
		{
			if (interactive)
				this.wrapper.RunCommand("Commit Table {0} Interactive".FormatWith(this.Name));
			else
				this.SaveChanges();
		}

		/// <summary>
		/// Discards the changes made to a Mapinfo table, this is the same as calling the revert command in Mapinfo.
		/// </summary>
		public void DiscardChanges()
		{
			this.wrapper.RunCommand("Rollback Table {0}".FormatWith(this.Name));
		}

		public IEnumerator<TTableDef> GetEnumerator()
		{
			return ((IEnumerable<TTableDef>)this.Provider.Execute(Expression)).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TTableDef>)this.Provider.Execute(Expression)).GetEnumerator();
		}

		public Type ElementType
		{
			get { return typeof(TTableDef); }
		}

		public System.Linq.Expressions.Expression Expression
		{
			get
			{
				return Expression.Constant(this);

			}
		}

		public IQueryProvider Provider { get; private set; }

        /// <summary>
        /// Close the current table in Mapinfo.
        /// </summary>
        public void Close()
        {
            this.wrapper.RunCommand("Close Table {0}".FormatWith(this.Name));
        }

        /// <summary>
        /// Close the supplied table in Mapinfo.
        /// </summary>
        /// <param name="table">The table that will be closed.</param>
        public static void CloseTable(ITable table)
        {
            table.Close();
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

	    private readonly static ITableFactory tabfactory = IoC.Resolve<ITableFactory>();

        /// <summary>
        /// Opens a new table in Mapinfo and returns the opened table.
        /// </summary>
        /// <param name="path">The path to the Mapinfo tab file to open.</param>
        /// <returns>An instance of <see cref="T:MapinfoWrapper.TableOperations.ITable"/></returns>
        public static ITable OpenTable(string path)
        {
            return tabfactory.OpenTable(path);   
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
            return tabfactory.OpenTable<TEntity>(path);
	    }
	}
}
