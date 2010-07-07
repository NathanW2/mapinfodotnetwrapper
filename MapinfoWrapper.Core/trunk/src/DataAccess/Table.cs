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
    /// Provides access to tables we you may not know/or care about the entity type.
    /// <para>This object is just a short hand version of 
    /// <code>
    ///     Table&lt;BaseEntity&gt;
    /// </code>
    /// </para>
    /// <para>This object inherits from Table{TEntity}</para>
    /// </summary>
    public class Table : Table<BaseEntity>
	{
	    internal Table(MapinfoSession MISession, string tableName)
            : base(MISession, tableName)
	    { }
	}
}
