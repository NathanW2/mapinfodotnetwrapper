namespace MapinfoWrapper.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;

    public interface ITable<TTableDef> : ITable, IEnumerable<TTableDef>
        where TTableDef : new()
    {

    }
}
