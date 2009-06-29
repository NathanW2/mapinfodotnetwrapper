using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Geometries
{
    public class Geometry : MapbasicObject, IGeometry
	{
		public Geometry(IVariable variable)
            : base(variable)
		{ }


        public Coordinate Centroid {
			get {
			    string expression = base.Variable.GetExpression();
                string x = base.mapinfoinstance.Evaluate("CentroidX({0})".FormatWith(expression));
                string y = base.mapinfoinstance.Evaluate("CentroidY({0})".FormatWith(expression));
				Decimal X = Decimal.Parse(x);
				Decimal Y = Decimal.Parse(y);
				return new Coordinate(X, Y);
			}
		}

		public bool Contains(Geometry mapinfoObject)
		{
            string expression = base.Variable.GetExpression();
		    string object2expression = mapinfoObject.Variable.GetExpression();

            string command = "{0} Contains {1}".FormatWith(expression, object2expression);
			string returned = base.mapinfoinstance.Evaluate(command);
			return (returned == "T");
		}

	}
}
