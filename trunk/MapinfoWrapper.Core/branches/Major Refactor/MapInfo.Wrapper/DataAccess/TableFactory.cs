using Mapinfo.Wrapper.DataAccess.Row.Entities;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.DataAccess
{
    /// <summary>
    /// A factory used to create new <see cref="Table"/> objects.
    /// </summary>
    internal class TableFactory
    {
        private readonly MapinfoSession misession;

        public TableFactory(MapinfoSession MISession)
        {
            this.misession = MISession;
        }

        public Table<TEntity> GetTableFor<TEntity>(string tableName)
            where TEntity : BaseEntity, new()
        {
            return new Table<TEntity>(this.misession, tableName);
        }

        public Table GetTableFor(string tableName)
        {
            return new Table(this.misession, tableName);
        }
    }
}
