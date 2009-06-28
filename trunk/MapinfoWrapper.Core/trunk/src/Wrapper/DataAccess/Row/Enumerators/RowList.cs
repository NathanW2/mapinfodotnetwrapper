using System.Collections.Generic;
using System.Collections;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.DataAccess.RowOperations.Enumerators
{
    public class RowList<TTabeDef> : IEnumerable<TTabeDef>
        where TTabeDef : BaseEntity, new()
    {
        private string tablename;

        public RowList(string tableName)
        {
            this.tablename = tableName;
        }

        public IEnumerator<TTabeDef> GetEnumerator()
        {
            IDataReader reader = ServiceLocator.GetInstance<IFactoryBuilder>().BuildDataReader(tablename);
            RowEnumerator<TTabeDef> rowenumerator = new RowEnumerator<TTabeDef>(reader);
            rowenumerator.Reset();
            return rowenumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
