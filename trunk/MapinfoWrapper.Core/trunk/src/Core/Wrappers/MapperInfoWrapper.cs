using System;
using MapInfo.Wrapper.Mapinfo;
using MapInfo.Wrapper.MapOperations;


namespace MapInfo.Wrapper.Core.Wrappers
{
    public class MapperInfoWrapper
    {
        private IMapInfoWrapper map_info;

        public MapperInfoWrapper(IMapInfoWrapper miSession)
        {
            Guard.AgainstNull(miSession, "MISession");

            this.map_info = miSession;
        }

        public string MapperInfo(int windowID, MapperInfoTypes attribute)
        {
            return this.map_info.Eval(String.Format("MapperInfo({0},{1})", windowID, (int)attribute));
        }
    }
}
