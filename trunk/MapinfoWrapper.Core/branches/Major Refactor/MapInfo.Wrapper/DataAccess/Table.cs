using Mapinfo.Wrapper.DataAccess.Row.Entities;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.DataAccess
{
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
