namespace MapinfoWrapper.DataAccess
{
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.Mapinfo;
    using System.Linq;
    using MapinfoWrapper.DataAccess.Row;
    using MapinfoWrapper.DataAccess.LINQ;

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
