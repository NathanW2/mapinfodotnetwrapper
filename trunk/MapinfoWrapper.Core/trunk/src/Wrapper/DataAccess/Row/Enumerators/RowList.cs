namespace MapinfoWrapper.DataAccess.RowOperations.Enumerators
{
    using System.Collections;
    using System.Collections.Generic;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;

    public class RowList<TTabeDef> : IEnumerable<TTabeDef>
        where TTabeDef : BaseEntity, new()
    {
        private readonly string tablename;
        private readonly IDataReader reader;

        public RowList(string tableName, IDataReader reader)
        {
            this.tablename = tableName;
            this.reader = reader;
        }

        public IEnumerator<TTabeDef> GetEnumerator()
        {
            RowEnumerator<TTabeDef> rowenumerator = new RowEnumerator<TTabeDef>(this.reader);
            rowenumerator.Reset();
            return rowenumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
