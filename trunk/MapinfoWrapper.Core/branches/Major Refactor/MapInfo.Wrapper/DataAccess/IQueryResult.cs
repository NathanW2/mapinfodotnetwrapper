using System.Collections.Generic;

namespace Mapinfo.Wrapper.DataAccess
{
    public interface IQueryResult<T> : IEnumerable<T>
    {
        string GetQueryString();
    }
}
