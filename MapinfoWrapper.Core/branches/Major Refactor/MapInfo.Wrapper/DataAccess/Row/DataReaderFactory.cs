using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.DataAccess.Row
{
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
