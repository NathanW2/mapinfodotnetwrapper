using MapInfo.Wrapper.Mapinfo;



namespace MapInfo.Wrapper.DataAccess.Row
{
    class DataReaderFactory
    {
        public DataReaderFactory(MapInfoSession MISession)
        {
            this.MapInfoSession = MISession;
        }

        public MapInfoSession MapInfoSession { get; private set; }

        public IMapInfoDataReader GetReaderFor(string tableName)
        {
            return new MapInfoDataReader(this.MapInfoSession, tableName);
        }
    }
}
