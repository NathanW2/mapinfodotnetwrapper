using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Extensions;
using Wrapper.Core.IoC;
using Wrapper.MapbasicOperations;

namespace Wrapper.ObjectOperations
{
	public interface IGeometry
	{
		bool Contains(Geometry mapinfoObject);
		Coordinate Centroid {
			get;
		}
	}
}
