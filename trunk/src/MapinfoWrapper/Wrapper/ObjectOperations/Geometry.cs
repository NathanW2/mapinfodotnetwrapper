using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Extensions;
using Wrapper.Core.IoC;
using Wrapper.MapbasicOperations;

namespace Wrapper.ObjectOperations
{
	public class Geometry : MapbasicObject, IGeometry
	{
		public Geometry(IVariableExtender variable) : this(IoC.Resolve<IMapinfoWrapper>(), variable)
		{
		}

		public Geometry(IMapinfoWrapper wrapper, IVariableExtender variable) : base(wrapper, variable)
		{
		}

		public Coordinate Centroid {
			get {
				string x = base.mapinfoinstance.Evaluate("CentroidX({0})".FormatWith(base.expression));
				string y = base.mapinfoinstance.Evaluate("CentroidY({0})".FormatWith(base.expression));
				Decimal X = Decimal.Parse(x);
				Decimal Y = Decimal.Parse(y);
				return new Coordinate(X, Y);
			}
		}

		public bool Contains(Geometry mapinfoObject)
		{
			string command = "{0} Contains {1}".FormatWith(base.expression, mapinfoObject.expression);
			string returned = base.mapinfoinstance.Evaluate(command);
			return returned == "T";
		}

	}
}
