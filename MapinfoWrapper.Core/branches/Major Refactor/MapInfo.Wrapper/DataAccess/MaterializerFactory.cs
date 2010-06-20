using Mapinfo.Wrapper.DataAccess.Row;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.DataAccess
{
    internal class MaterializerFactory
    {
        private MapinfoSession misession;

        public MaterializerFactory(MapinfoSession MISession)
        {
            this.misession = MISession;
        }

        public EntityMaterializer CreateMaterializerFor(string tableName, IDataReader reader)
        {
            return new EntityMaterializer(this.misession, tableName, reader);
        }
    }
}
