namespace MapinfoWrapper.DataAccess.RowOperations.Enumerators
{
    using System.Collections;
    using System.Collections.Generic;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.Mapinfo;

    internal class RowList<TTabeDef> : IEnumerable<TTabeDef>
        where TTabeDef : BaseEntity, new()
    {
        private readonly Table table;
        private readonly IDataReader reader;
        private readonly EntityFactory entityfactory;

        public RowList(Table table, IDataReader reader, MapinfoSession MISession, EntityFactory factory)
        {
            this.table = table;
            this.reader = reader;
            this.MapinfoSession = MISession;
            this.entityfactory = factory;
        }

        public MapinfoSession MapinfoSession { get; private set; }

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
