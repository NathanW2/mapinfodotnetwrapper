﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Geometries.Points
{
    public class Point : Geometry
    {
    	public Point(IVariableExtender variable)
    		: this(IoC.Resolve<IMapinfoWrapper>(),variable) { }
    	
        public Point(IMapinfoWrapper wrapper, IVariableExtender variable) 
            : base(wrapper,variable) { }
    }
}
