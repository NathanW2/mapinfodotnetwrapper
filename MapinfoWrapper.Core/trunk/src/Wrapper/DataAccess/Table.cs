using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapinfoWrapper.Core;
using MapinfoWrapper.DataAccess.LINQ.SQLBuilders;
using MapinfoWrapper.DataAccess.RowOperations;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.DataAccess.RowOperations.Enumerators;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.DataAccess
{
	/// <summary>
	/// A Mapinfo table object, allows access to properties for the table
	/// opened in Mapinfo. 
	/// </summary>
	/// <typeparam name="TEntity">The entity type to use with the table, as it row entity.</typeparam>
	public class Table<TEntity> : Table, ITable<TEntity>
        where TEntity : BaseEntity ,new()
	{

        internal Table(MapinfoSession MISession,
                       IQueryProvider queryProvider,
                       IDataReader reader,
                       string tableName)
            : base(MISession, reader, tableName)
        {
            this.Provider = queryProvider;
        }

        /// <summary>
        /// Returns a <typeparamref name="TEntity"/> at the supplied index in the table.
        /// </summary>
        /// <param name="index">The index at which to get the <typeparamref name="TEntity"/></param>
        /// <returns>An instace of <typeparamref name="TEntity"/> for the supplied index.</returns>
        new public virtual TEntity this[int index]
        {
            get
            {
                TEntity entity = new TEntity();
                entity.reader = this.reader;
                base.reader.Fetch(index);
                return base.reader.PopulateEntity(entity);
            }
        }
		
        /// <summary>
        /// Rows a collection of rows from the table, using <typeparam name="TEntity" /> as
        /// the row collection type.
        /// </summary>
	    new public IEnumerable<TEntity> Rows
	    {
	        get
	        {
	            return new RowList<TEntity>(this.Name, this.reader);
	        }
	    }

		/// <summary>
		/// Inserts a new row into the table.
		/// </summary>
		/// <param name="newRow"></param>
		public TEntity InsertRow(TEntity newRow)
		{
            Guard.AgainstNull(newRow, "newRow");

            // NOTE! This might be able to be moved down into subclass.
            SqlStringGenerator sqlstringgen = new SqlStringGenerator();

            string insertstring = sqlstringgen.GenerateInsertString(newRow, this.Name);

            base.miSession.RunCommand(insertstring);
		    this.reader.FetchLast();
            TEntity row = new TEntity();
		    row = this.reader.PopulateEntity(row);
            return row;
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
	}
}
