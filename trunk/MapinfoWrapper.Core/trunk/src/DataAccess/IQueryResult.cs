using System.Collections.Generic;

namespace MapInfo.Wrapper.DataAccess
{
    public interface IQueryResult<T> : IEnumerable<T>
    {
        string GetQueryString();
    }
}
