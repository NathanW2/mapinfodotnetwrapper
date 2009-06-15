using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MapinfoWrapper.Core.IoC
{
    public class DependencyResolver : IDependencyResolver
    {
        private IDictionary<Type, object> typelookup = new Dictionary<Type, object>();

        #region IDependencyResolver Members
        [DebuggerStepThrough]
        public void Register(Type type, object obj)
        {
            Guard.AgainstNull(type, "type");
            Guard.AgainstNull(obj, "obj");

            this.typelookup.Add(type, obj);
        }

        [DebuggerStepThrough]
        public T Resolve<T>()
        {
            return (T)this.typelookup[typeof(T)];
        }

        #endregion
    }
}
