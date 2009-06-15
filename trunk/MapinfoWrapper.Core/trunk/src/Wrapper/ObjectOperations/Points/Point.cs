using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.MapbasicOperations;
using Wrapper.Extensions;
using Wrapper.Core.IoC;

namespace Wrapper.ObjectOperations.Points
{
    public class Point : Geometry
    {
    	public Point(IVariableExtender variable)
    		: this(IoC.Resolve<IMapinfoWrapper>(),variable) { }
    	
        public Point(IMapinfoWrapper wrapper, IVariableExtender variable) 
            : base(wrapper,variable) { }
    }
}
