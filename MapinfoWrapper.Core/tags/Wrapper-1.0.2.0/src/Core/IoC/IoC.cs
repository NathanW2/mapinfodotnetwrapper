using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MapinfoWrapper.Core.IoC
{
    public class IoC
    {
        public static IDependencyResolver innerresolver;

        [DebuggerStepThrough]
        public static T Resolve<T>()
        {
            Guard.AgainstNull(innerresolver, "resolver");
            return innerresolver.Resolve<T>();
        }

        [DebuggerStepThrough]
        public static void Register(Type type, object obj)
        {
            Guard.AgainstNull(innerresolver, "resolver");
            innerresolver.Register(type, obj);
        }


        [DebuggerStepThrough]
        public static void Initialize(IDependencyResolver resolver)
        {
            innerresolver = resolver;
        }
    }
}
