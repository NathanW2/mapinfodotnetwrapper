using System.Collections.Generic;
using System.Linq;

namespace MapinfoWrapper.DataAccess
{
    public interface ITable<TTableDef> : ITable, IQueryable<TTableDef>
        where TTableDef : new()
    {
        IEnumerable<TTableDef> Rows { get; }
        TTableDef InsertRow(TTableDef row);
    }
}
