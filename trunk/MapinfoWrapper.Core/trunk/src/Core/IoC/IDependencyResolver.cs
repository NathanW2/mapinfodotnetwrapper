using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.Core.IoC
{
    public interface IDependencyResolver
    {
        void Register(Type type, object obj);
        T Resolve<T>();
    }
}
