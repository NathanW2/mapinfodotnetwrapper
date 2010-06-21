using System;
using Mapinfo.Wrapper.Core.Extensions;
using System.Diagnostics;

namespace Mapinfo.Wrapper.Core
{
    public static class Guard
    {
    	[DebuggerStepThrough]
    	public static void AgainstNullOrEmpty(string argument, string name)
    	{
            if (String.IsNullOrEmpty(argument)) 
            {
                throw new ArgumentNullException(name, "{0} can not be null".FormatWith(name));
            }
    	}
    	
        [DebuggerStepThrough]
        public static void AgainstNull(object @object, string name)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(name, "{0} can not be null".FormatWith(name));
            }
        }

        [DebuggerStepThrough]
        public static void AgainstIsZero(int value,string name)
        {
            if (value == 0)
            {
                throw new ArgumentException("Value can not be zero", name);
            }
        }
    }
}
