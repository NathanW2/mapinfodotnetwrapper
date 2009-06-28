using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.Core.IoC
{
    public interface IDependencyResolver
    {
        void Register<T>(T obj);
        T Resolve<T>();
    }
}
