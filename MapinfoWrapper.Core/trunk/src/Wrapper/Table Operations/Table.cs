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
using MapinfoWrapper.CommandBuilders;

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
		private string tablename;
		private IMapinfoWrapper wrapper;

		public Table(string tableName)
			: this(IoC.Resolve<IMapinfoWrapper>(), tableName)
		{ }

		/// <summary>
		/// Creates a new Table object used to gather information
		/// about a specifed table.
		/// </summary>
		/// <param name="wrapper">A wrapper object containing the running instace of mapinfo.</param>
		/// <param name="tablename">The name of the table for which to gather information about.</param>
		public Table(IMapinfoWrapper wrapper, string tablename)
		{
			this.wrapper = wrapper;
			this.tablename = tablename;
			this.Provider = new MapinfoQueryProvider(this.wrapper, this.Name);
		}
		
		/// <summary>
		/// Returns a <see cref="RowList&lt;TTableDef&gt;"/> using the TTableDef supplied with the
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
		/// Returns a <see cref="T:TTableDef"/> at the supplied index in the table.
		/// </summary>
		/// <param name="index">The index at which to get the <see cref="T:TTableDef"/></param>
		/// <returns>An instace of <see cref="T:TTableDef"/> for the supplied index.</returns>
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

		public static string ResolveTableNameFromNumber(IMapinfoWrapper wrapper, int tableNumber)
		{
			return Table.TableInfo(wrapper, tableNumber, TableInfoEnum.TAB_INFO_NAME);
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
                    name = this.TableInfo(TableInfoEnum.TAB_INFO_NAME);
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
				string returnValue = this.TableInfo(TableInfoEnum.TAB_INFO_TABFILE);

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
				return Convert.ToInt32(this.TableInfo(TableInfoEnum.TAB_INFO_NUM));
			}
		}

		/// <summary>
		/// Returns true if the table is mappable.
		/// If the table is mappable it may be passed to the <see cref="Map.MapTable"/> command.
		/// </summary>
		public bool IsMappable
		{
			get
			{
				string mapinforeturn = TableInfo(TableInfoEnum.TAB_INFO_MAPPABLE);
				return (mapinforeturn == "T");

			}
		}
		
		/// <summary>
		/// Runs a tableinfo command in mapinfo against the supplied table in the construtor.
		/// </summary>
		/// <param name="infoToReturn">A enum specifing the type of info to return.</param>
		/// <returns>A string containing the value returned from the evaluation of the Table commnd.</returns>
		public string TableInfo(TableInfoEnum infoToReturn)
		{
			return Table.TableInfo(this.wrapper,this.tablename, infoToReturn);
		}

		/// <summary>
		/// Returns the number of columns in the current table.
		/// </summary>
		/// <returns>The number of columns in the current table.</returns>
		public int GetNumberOfColumns()
		{
			return Convert.ToInt32(this.TableInfo(TableInfoEnum.TAB_INFO_NCOLS));
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

		/// <summary>
		/// Opens a table in mapinfo and returns a <see cref="Table"/> object, which can be used to get
		/// infomation about the table opened.
		/// </summary>
		/// <param name="wrapper">An instance of Mapinfo to open the table in.</param>
		/// <param name="commandBuidler">A command builder object used to set all open table command
		/// properties.</param>
		/// <returns>A <see cref="Table"/> object containing information about the table opened.</returns>
		public static Table OpenTable(IMapinfoWrapper wrapper, OpenTableCommandBuilder commandBuidler)
		{
			wrapper.RunCommand(commandBuidler.BuildCommandString());
			string name = Table.ResolveTableNameFromNumber(wrapper,0);
			return new Table(wrapper,name);
		}

		/// <summary>
		/// Opens a table in mapinfo and returns a <see cref="Table"/> object, which can be used to get
		/// infomation about the table opened.
		/// </summary>
		/// <param name="wrapper">An instance of Mapinfo to open the table in.</param>
		/// <param name="tablePath">The path of the table that needs to be opened.</param>
		/// <returns>A <see cref="Table"/> object containing information about the table opened.</returns>
		public static Table OpenTable(IMapinfoWrapper wrapper, string tablePath)
		{
			OpenTableCommandBuilder commandbuilder = new OpenTableCommandBuilder(tablePath);
			return OpenTable(wrapper, commandbuilder);
		}

		/// <summary>
		/// Opens a table in mapinfo and returns a Generic Table which can be used to get strong typed access to
		/// the table fields.
		/// </summary>
		/// <typeparam name="TTableDefinition">A strong typed table definition that will be used to access
		/// columns, the table definition should be exact match of the Mapinfo table
		/// structure. <see cref="TTableDefinition"/> for more info.</typeparam>
		/// <param name="wrapper">An instance of Mapinfo to open the table in.</param>
		/// <param name="tablePath">The path of the table which needs to be opened.</param>
		/// <returns>A generic <see cref="Table"/> which will give strong typed access to column names.</returns>
		public static Table<TTableDefinition> OpenTable<TTableDefinition>(IMapinfoWrapper wrapper, string tablePath)
                where TTableDefinition : BaseEntity, new()
        {
			OpenTableCommandBuilder commandbuilder = new OpenTableCommandBuilder(tablePath);
			wrapper.RunCommand(commandbuilder.BuildCommandString());
			string name = Table.ResolveTableNameFromNumber(wrapper,0);
			return new Table<TTableDefinition>(wrapper, name);
		}
        
        /// <summary>
		/// Runs a tableinfo command in mapinfo against the supplied table.
		/// </summary>
		/// <param name="tableNumber">The number of the table to run the tableinfo command against.</param>
		/// <param name="infoToReturn">A enum specifing the type of info to return.</param>
		/// <returns>A string containing the value returned from the evaluation of the Table commnd.</returns>
		public static string TableInfo(IMapinfoWrapper wrapper, int tableNumber, TableInfoEnum infoToReturn)
		{
			return Table.TableInfo(wrapper, tableNumber.ToString(), infoToReturn);
		}

		/// <summary>
		/// Runs a tableinfo command in mapinfo against the supplied table.
		/// </summary>
		/// <param name="tableName">The name of the table to run the tableinfo command against.</param>
		/// <param name="infoToReturn">A enum specifing the type of info to return.</param>
		/// <returns>A string containing the value returned from the evaluation of the Table commnd.</returns>
		public static string TableInfo(IMapinfoWrapper wrapper, string tableName, TableInfoEnum infoToReturn)
		{
			int enumValue = (int)infoToReturn;
			string command = String.Format("TableInfo({0},{1})", tableName, (int)enumValue);
			return wrapper.Evaluate(command);
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

    }
}
