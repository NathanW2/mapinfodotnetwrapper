using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.DataAccess.RowOperations;

namespace MapinfoWrapper.DataAccess
{
    public interface ITable<TTableDef> : ITable, IQueryable<TTableDef>
        where TTableDef : new()
    {
        IEnumerable<TTableDef> Rows { get; }
        TTableDef InsertRow(TTableDef row);
        void DeleteRow(TTableDef entity);
    }
}
