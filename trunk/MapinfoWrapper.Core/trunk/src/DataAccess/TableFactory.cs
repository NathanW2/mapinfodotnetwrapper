using MapInfo.Wrapper.DataAccess.Entities;
using MapInfo.Wrapper.Mapinfo;


namespace MapInfo.Wrapper.DataAccess
{

    /// <summary>
    /// A factory used to create new <see cref="Table"/> objects.
    /// </summary>
    internal class TableFactory
    {
        private readonly MapInfoSession misession;

        public TableFactory(MapInfoSession miSession)
        {
            this.misession = miSession;
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
