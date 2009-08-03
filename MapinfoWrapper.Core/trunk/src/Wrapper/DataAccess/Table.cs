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
                EntityBuilder builder = new EntityBuilder(this.miSession, this);
                TEntity entity = builder.GenerateEntityForIndex<TEntity>(index);
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
	            return new RowList<TEntity>(this.Name, this.reader);
	        }
	    }

		/// <summary>
		/// Adds the supplied <typeparamref name="TEntity"/> to the 
		/// internal list to be inserted.
		/// </summary>
		/// <param name="newRow"></param>
		public void InsertRow(TEntity newRow)
		{
            Guard.AgainstNull(newRow, "newRow");

		    Debug.Assert(base.EntitesToBeInserted != null);

		    base.EntitesToBeInserted.Add(newRow);
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

	    private IQueryProvider provider;
	    public IQueryProvider Provider
	    {
	        get 
            { 
                if (provider == null)
                {
                    provider = new MapinfoQueryProvider(this.miSession);
                }
                return provider;
            }
	    }

        /// <summary>
        /// Commits any pending inserts and deletes to the table in Mapinfo.  
        /// This method does NOT save the underlying Mapinfo table.  
        /// To save any changes that have been made from calling this method you will need
        /// to call SaveChanges.
        /// </summary>
        public void CommitPendingChanges()
        {
            foreach (var entity in base.EntitesToBeInserted)
            {
                TEntity e = (TEntity) entity;
                // NOTE! This might be able to be moved down into subclass.
                SqlStringGenerator sqlstringgen = new SqlStringGenerator();
                string insertstring = sqlstringgen.GenerateInsertString(e, this.Name);
                
                // Insert the entity into the table.
                base.miSession.RunCommand(insertstring);

                e.reader = this.reader;
                e.AttachedTo = this;
            }
            // Remove all the entites from the pending insert list.
            base.EntitesToBeInserted.Clear();
        }
	}
}
