using System.Collections.Generic;
using System.Linq;

namespace MapinfoWrapper.DataAccess
{
    internal interface ITable<TTableDef> : ITable, IQueryable<TTableDef>
        where TTableDef : new()
    {
        IEnumerable<TTableDef> Rows { get; }
        void InsertRow(TTableDef row);
    }
}
