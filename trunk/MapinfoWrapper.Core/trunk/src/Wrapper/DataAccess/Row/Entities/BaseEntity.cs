namespace MapinfoWrapper.DataAccess.RowOperations.Entities
{
    public class BaseEntity
    {
        protected internal IDataReader reader;
        public int RowId { get; internal set; }

        public object Get(string columnName)
        {
            return this.reader.Get(columnName);
        }
    }
}
