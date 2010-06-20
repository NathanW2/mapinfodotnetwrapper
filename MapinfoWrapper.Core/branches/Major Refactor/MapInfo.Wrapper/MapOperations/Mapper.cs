using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.MapOperations
{
    public class Mapper
    {
        private readonly MapinfoSession miSession;

        public Mapper(MapinfoSession MISession)
        {
            miSession = MISession;
        }

    }
}
