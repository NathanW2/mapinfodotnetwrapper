namespace MapinfoWrapper.DataAccess
{
    using Core.Extensions;
    using Mapinfo;
    using MapinfoWrapper.Exceptions;

    internal class MapbasicWrapper
    {
        private readonly IMapinfoWrapper session;
        public MapbasicWrapper(IMapinfoWrapper Session)
        {
            this.session = Session;
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