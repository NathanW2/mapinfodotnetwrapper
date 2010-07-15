using MapInfo.Wrapper.Core.Extensions;
using MapInfo.Wrapper.DataAccess;
using MapInfo.Wrapper.Mapinfo;



namespace MapInfo.Wrapper.Core.Wrappers
{
    public class TableInfoWrapper
    {
        private readonly IMapInfoWrapper map_info;

        public TableInfoWrapper(IMapInfoWrapper miSession)
        {
            Guard.AgainstNull(miSession, "MISession");

            this.map_info = miSession;
        }

        public string GetTableInfo(string tableName,TableInfo attribute)
        {
            int enumvalue = (int)attribute;
            string command = "TableInfo({0},{1})".FormatWith(tableName, enumvalue);
            string value = map_info.Eval(command);
            return value;
        }

        public string GetName(string tableName)
        {
            return this.GetTableInfo(tableName,TableInfo.Name);
        }

        public string GetName(int tableNumber)
        {
            return this.GetName(tableNumber.ToString());
        }
        
    }
}
