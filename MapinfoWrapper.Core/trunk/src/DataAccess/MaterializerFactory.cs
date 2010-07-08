using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.DataAccess.RowOperations;

namespace MapinfoWrapper.DataAccess
{
    public class MaterializerFactory
    {
        private MapinfoSession misession;

        public MaterializerFactory(MapinfoSession miSession)
        {
            this.misession = miSession;
        }

        public EntityMaterializer CreateMaterializerFor(string tableName, IDataReader reader)
        {
            return new EntityMaterializer(this.misession, tableName, reader);
        }
    }
}
