using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Exceptions;

namespace MapinfoWrapper.Core.Wrappers
{
    public class MapbasicWrapper
    {
        private readonly IMapinfoWrapper session;

        public MapbasicWrapper(IMapinfoWrapper miSession)
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