namespace MapinfoWrapper.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;

    internal interface ITable<TTableDef> : ITable, IQueryable<TTableDef>
        where TTableDef : new()
    {
        IEnumerable<TTableDef> Rows { get; }
        void InsertRow(TTableDef row);
    }
}
