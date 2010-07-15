using MapInfo.Wrapper.DataAccess.Row;
using MapInfo.Wrapper.Mapinfo;
namespace MapInfo.Wrapper.DataAccess
{
    public class MaterializerFactory
    {
        private MapInfoSession misession;

        public MaterializerFactory(MapInfoSession miSession)
        {
            this.misession = miSession;
        }

        public EntityMaterializer CreateMaterializerFor(ITable table, IMapInfoDataReader reader)
        {
            return new EntityMaterializer(this.misession, table, reader);
        }
    }
}
