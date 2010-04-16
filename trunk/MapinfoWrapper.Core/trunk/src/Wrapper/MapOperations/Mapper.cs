namespace MapinfoWrapper.MapOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.DataAccess;
    using MapinfoWrapper.Mapinfo;

    public class Mapper
    {
        private readonly MapinfoSession miSession;

        public Mapper(MapinfoSession MISession)
        {
            miSession = MISession;
        }

    }
}
