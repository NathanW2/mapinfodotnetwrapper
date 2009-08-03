namespace MapinfoWrapper.Geometries
{
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;

    public class Geometry : MapbasicObject, IGeometry
	{
        internal Geometry(MapinfoSession MISession, IVariable variable)
            : base(MISession, variable)
        { }

        public Coordinate Centroid {
			get {
			    string expression = base.Variable.GetExpression();
                string x = base.misession.Evaluate("CentroidX({0})".FormatWith(expression));
                string y = base.misession.Evaluate("CentroidY({0})".FormatWith(expression));
                double X = double.Parse(x);
                double Y = double.Parse(y);
				return new Coordinate(X, Y);
			}
		}

		public bool Contains(Geometry mapinfoObject)
		{
            string expression = base.Variable.GetExpression();
		    string object2expression = mapinfoObject.Variable.GetExpression();

            string command = "{0} Contains {1}".FormatWith(expression, object2expression);
            string returned = base.misession.Evaluate(command);
			return (returned == "T");
		}

	}
}
