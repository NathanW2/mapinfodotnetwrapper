using System;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.MapOperations;

namespace MapinfoWrapper.Core.Wrappers
{
    public class MapperInfoWrapper
    {
        private IMapinfoWrapper mapinfo;

        public MapperInfoWrapper(IMapinfoWrapper miSession)
        {
            Guard.AgainstNull(miSession, "MISession");

            this.mapinfo = miSession;
        }

        public string MapperInfo(int windowID, MapperInfoTypes attribute)
        {
            return this.mapinfo.Eval(String.Format("MapperInfo({0},{1})", windowID, (int)attribute));
        }
    }
}
