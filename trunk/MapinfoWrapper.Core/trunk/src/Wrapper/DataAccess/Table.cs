using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapinfoWrapper.Core;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess.LINQ.SQLBuilders;
using MapinfoWrapper.DataAccess.RowOperations;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.DataAccess.RowOperations.Enumerators;
using MapinfoWrapper.Core.Internals;

namespace MapinfoWrapper.DataAccess
{
	/// <summary>
	/// A Mapinfo table object, allows access to properties for the table
	/// opened in Mapinfo. 
	/// </summary>
	/// <typeparam name="TEntity">The entity type to use with the table, as it row entity.</typeparam>
	public class Table<TEntity> : ITable<TEntity>
        where TEntity : BaseEntity ,new()
	{
		private readonly string internalname;
	    private readonly ITableCommandRunner tablecommandrunner;

        internal Table(ITableCommandRunner tableRunner, 
                       IQueryProvider queryProvider, 
                       string tableName)
        {
            this.tablecommandrunner = tableRunner;
            this.Provider = queryProvider;
            this.internalname = tableName;
        }
		
		/// <summary>
        /// Returns a <see cref="T:RowList&lt;TEntity&gt;"/> where each row is a new
        /// <typeparam name="TEntity"/>.
		/// </summary>
		public IEnumerable<TEntity> Rows
		{
			get
			{
				return new RowList<TEntity>(this.Name);
			}
		}

		/// <summary>
		/// Inserts a new row into the table.
		/// </summary>
		/// <param name="newRow"></param>
		public TEntity InsertRow(TEntity newRow)
		{
            Guard.AgainstNull(newRow, "newRow");
            SqlStringGenerator sqlstringgen = new SqlStringGenerator();

            string insertstring = sqlstringgen.GenerateInsertString(newRow, this.Name);

            this.tablecommandrunner.RunCommand(insertstring);
		    IDataReader reader = ServiceLocator.GetInstance<IFactoryBuilder>().BuildDataReader(this.Name);
            reader.FetchLast();
            TEntity row = new TEntity();
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

			this.tablecommandrunner.RunCommand("Delete From {0} Where Rowid = {1}".FormatWith(this.Name, index));
		}

		/// <summary>
		/// Returns a <see cref="T:TEntity"/> at the supplied index in the table.
		/// </summary>
		/// <param name="index">The index at which to get the <see cref="T:TEntity"/></param>
		/// <returns>An instace of <see cref="T:TEntity"/> for the supplied index.</returns>
		public TEntity this[int index]
		{
			get
			{
                TEntity row = new TEntity();
                row.RowId = index;
				return row;
			}

		}

		/// <summary>
		/// Deletes the selected row from the current table.
		/// </summary>
		/// <param name="entity">The row that will be deleted.</param>
		/// <exception cref="T:ArgumentNullException"/>
		public void DeleteRow(TEntity entity)
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
		/// Returns if the table is mappable or not.
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
	        this.SaveChanges(false);
		}

		/// <summary>
		/// Commits any pending changes for the current table.
		/// </summary>
		/// <param name="interactive">If set to true in the event of a conflict, MapInfo Professional displays the Conflict Resolution dialog box.
		/// <para>After a successful Commit Table Interactive statement, MapInfo Professional displays a dialog box allowing the user to refresh.</para>
		/// </param>
		public void SaveChanges(bool interactive)
		{
            string command = "Commit Table {0}".FormatWith(this.Name);

			if (interactive)
			    command += " Interactive";

            this.tablecommandrunner.RunCommand(command);
		}

		/// <summary>
		/// Discards the changes made to a Mapinfo table, this is the same as calling the revert command in Mapinfo.
		/// </summary>
		public void DiscardChanges()
		{
            this.tablecommandrunner.RunCommand("Rollback Table {0}".FormatWith(this.Name));
		}

		public IEnumerator<TEntity> GetEnumerator()
		{
			return ((IEnumerable<TEntity>)this.Provider.Execute(Expression)).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TEntity>)this.Provider.Execute(Expression)).GetEnumerator();
		}

		public Type ElementType
		{
			get { return typeof(TEntity); }
		}

		public Expression Expression
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
            this.tablecommandrunner.RunCommand("Close Table {0}".FormatWith(this.Name));
        }
	}
}
