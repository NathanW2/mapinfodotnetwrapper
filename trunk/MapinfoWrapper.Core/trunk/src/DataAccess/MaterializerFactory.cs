namespace MapinfoWrapper.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MapinfoWrapper.Mapinfo;
    using MapinfoWrapper.DataAccess.RowOperations;

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
