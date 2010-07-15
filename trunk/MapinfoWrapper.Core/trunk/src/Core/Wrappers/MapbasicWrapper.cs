using MapInfo.Wrapper.Core.Extensions;
using MapInfo.Wrapper.Exceptions;
using MapInfo.Wrapper.Mapinfo;


namespace MapInfo.Wrapper.Core.Wrappers
{
    public class MapbasicWrapper
    {
        private readonly IMapInfoWrapper session;

        public MapbasicWrapper(IMapInfoWrapper miSession)
        {
            this.session = miSession;
        }

        public int GetNumberOfOpenTables()
        {
            string command = "NumTables()";
            string value = this.session.Eval(command);
            int count;

            if (int.TryParse(value,out count))
            {
                return count;
            }
            else
            {
                throw new MapbasicException("Return type from NumTables() was invailed.  Expected interger but was {0}".FormatWith(value));
            }
        }
    }
}