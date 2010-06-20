using Mapinfo.Wrapper.DataAccess.Row.Entities;
using System.Collections;
using System.Collections.Generic;

namespace Mapinfo.Wrapper.DataAccess.Row.Enumerators
{
    internal class RowList<TTabeDef> : IEnumerable<TTabeDef>
        where TTabeDef : BaseEntity, new()
    {
        private readonly IDataReader reader;
        private readonly EntityMaterializer entityfactory;

        public RowList(IDataReader reader, EntityMaterializer factory)
        {
            this.reader = reader;
            this.entityfactory = factory;
        }

        public IEnumerator<TTabeDef> GetEnumerator()
        {
            RowEnumerator<TTabeDef> rowenumerator = new RowEnumerator<TTabeDef>(this.reader, entityfactory);
            rowenumerator.Reset();
            return rowenumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
