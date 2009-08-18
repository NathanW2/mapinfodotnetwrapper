namespace MapinfoWrapper.DataAccess.RowOperations.Enumerators
{
    using System.Collections;
    using System.Collections.Generic;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.Mapinfo;

    public class RowList<TTabeDef> : IEnumerable<TTabeDef>
        where TTabeDef : BaseEntity, new()
    {
        private readonly Table table;
        private readonly IDataReader reader;

        public RowList(Table table, IDataReader reader, MapinfoSession MISession)
        {
            this.table = table;
            this.reader = reader;
            this.MapinfoSession = MISession;
        }

        public MapinfoSession MapinfoSession { get; private set; }

        public IEnumerator<TTabeDef> GetEnumerator()
        {
            EntityFactory entityfac = new EntityFactory(this.MapinfoSession, this.table);
            RowEnumerator<TTabeDef> rowenumerator = new RowEnumerator<TTabeDef>(this.reader,entityfac);
            rowenumerator.Reset();
            return rowenumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
