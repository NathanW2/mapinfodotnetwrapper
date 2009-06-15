using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Wrapper.TableOperations.Row
{
    public interface IMapinfoRow<T> : IMapinfoRow
    {
        object GetValue<TProperty>(Expression<Func<T,TProperty>> columnNames);
    }
}
