namespace MapinfoWrapper.DataAccess
{
    using Core.Extensions;

    using Mapinfo;

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
            string value = this.session.Evaluate(command);
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