namespace MapinfoWrapper.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using MapinfoWrapper.Core;
    using MapinfoWrapper.DataAccess.LINQ;
    using MapinfoWrapper.DataAccess.LINQ.SQLBuilders;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.DataAccess.RowOperations.Enumerators;
    using MapinfoWrapper.Mapinfo;

	/// <summary>
	/// A Mapinfo table object, allows access to properties for the table
	/// opened in Mapinfo. 
	/// </summary>
	/// <typeparam name="TEntity">The entity type to use with the table, as it row entity.</typeparam>
	public class Table<TEntity> : Table, ITable<TEntity>
        where TEntity : BaseEntity ,new()
	{
	    internal Table(MapinfoSession MISession, string tableName)
            : base(MISession, tableName)
	    { }

	    /// <summary>
        /// Returns a <typeparamref name="TEntity"/> at the supplied index in the table.
        /// </summary>
        /// <param name="index">The index at which to get the <typeparamref name="TEntity"/></param>
        /// <returns>An instace of <typeparamref name="TEntity"/> for the supplied index.</returns>
        new public TEntity this[int index]
        {
            get
            {
                TEntity entity = base.EntityFactory.GenerateEntityForIndex<TEntity>(index);
                return entity;
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
	            return new RowList<TEntity>(this, this.reader, this.MapinfoSession,this.EntityFactory);
	        }
	    }

        public IEnumerator<TEntity> GetEnumerator()
		{
            var queryabletable = (IQueryable)this;
            return ((IEnumerable<TEntity>)queryabletable.Provider.Execute(queryabletable.Expression))
                                                                 .GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
            return this.GetEnumerator();
		}

        Type IQueryable.ElementType
		{
			get { return typeof(TEntity); }
		}

        Expression IQueryable.Expression
		{
			get
			{
				return Expression.Constant(this);

			}
		}

	    private IQueryProvider provider;
        IQueryProvider IQueryable.Provider
	    {
	        get 
            { 
                if (provider == null)
                {
                    provider = new MapinfoQueryProvider(this.MapinfoSession, this.EntityFactory);
                }
                return provider;
            }
	    }

	}
}
