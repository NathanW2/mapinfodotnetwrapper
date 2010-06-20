using System.Collections.Generic;

namespace Mapinfo.Wrapper.DataAccess
{
    public interface ITable<TTableDef> : ITable, IEnumerable<TTableDef>
        where TTableDef : new()
    {

    }
}
