using System.Collections.Generic;

namespace MapInfo.Wrapper.DataAccess
{
    public interface ITable<TTableDef> : ITable, IEnumerable<TTableDef>
        where TTableDef : new()
    {

    }
}
