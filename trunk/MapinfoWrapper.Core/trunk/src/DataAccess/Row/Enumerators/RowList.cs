using MapInfo.Wrapper.DataAccess;
using MapInfo.Wrapper.DataAccess.Entities;
using System.Collections;
using System.Collections.Generic;

namespace MapInfo.Wrapper.DataAccess.Row.Enumerators
{
    internal class RowList<TTabeDef> : IEnumerable<TTabeDef>
        where TTabeDef : BaseEntity, new()
    {
        private readonly IMapInfoDataReader reader;
        private readonly EntityMaterializer entityfactory;

        public RowList(IMapInfoDataReader reader, EntityMaterializer factory)
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
