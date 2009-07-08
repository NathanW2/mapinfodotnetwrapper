using System;

namespace MapinfoWrapper.DataAccess.RowOperations.Entities
{
    public class BaseEntity
    {
        protected internal IDataReader reader;
        public int RowId { get; internal set; }

        public object Get(string columnName)
        {
            if (RowId == 0 || this.reader == null)
                throw new ArgumentException("The current row has not been inserted to a table so rows can not be read.");
            
            return this.reader.Get(columnName);
        }
    }
}
