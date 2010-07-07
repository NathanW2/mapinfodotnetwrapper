using System.Collections.Generic;

namespace MapinfoWrapper.DataAccess
{
    public interface IQueryResult<T> : IEnumerable<T>
    {
        string GetQueryString();
    }
}
