using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MapinfoWrapper.Core.IoC
{
    internal class ServiceLocator
    {
        private static IDependencyResolver innerresolver;

        [DebuggerStepThrough]
        public static T GetInstance<T>()
        {
            Guard.AgainstNull(innerresolver, "resolver");
            return innerresolver.Resolve<T>();
        }

        public static IDependencyResolver Current { get { return innerresolver; } }


        [DebuggerStepThrough]
        public static void Initialize(IDependencyResolver resolver)
        {
            innerresolver = resolver;
        }
    }
}
