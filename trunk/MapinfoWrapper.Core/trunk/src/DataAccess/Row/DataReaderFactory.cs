namespace MapinfoWrapper.DataAccess.Row
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MapinfoWrapper.Mapinfo;
    using MapinfoWrapper.DataAccess.RowOperations;

    class DataReaderFactory
    {
        public DataReaderFactory(MapinfoSession MISession)
        {
            this.MapinfoSession = MISession;
        }

        public MapinfoSession MapinfoSession { get; private set; }

        public IDataReader GetReaderFor(string tableName)
        {
            return new DataReader(this.MapinfoSession, tableName);
        }
    }
}
