using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.DataAccess
{
    /// <summary>
    /// Represents the entity loading options for the supplied types.
    /// <para>This object should be used if you don't need to use weak typed column names with a entity as it will speed up
    /// the retrival time for that entity dramatically </para>
    /// </summary>
    public class EntityLoadOptions
    {
        List<Type> nobackingtypes = new List<Type>();

        public List<Type> HasNoBackingStore
        {
            get { return this.nobackingtypes; }
        }

        /// <summary>
        /// Adds the supplied <typeparamref name="TEntity"/> to the list objects to ignore loading the backing store for.
        /// 
        /// <para>Not loading the backing store will dramatically increase the time it takes for the wrapper to fill entities 
        /// with data from the row in the table. Only the properties in entity that represent columns in the table will be loaded.</para>
        /// 
        /// <para>If an object has no backing store you will not be be able to using the index property of the entity
        /// to get or set data for the entity.</para>
        /// 
        /// <para> For example a object with no backing store will not be able to do the following:
        /// <code>
        ///     myentity["ColumnName"]
        /// </code>
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public EntityLoadOptions NoBackingStore<TEntity>()
        {
            if (!this.nobackingtypes.Contains(typeof(TEntity)))
            {
                this.nobackingtypes.Add(typeof(TEntity));
            }
            return this;
        }
    }
}
