using System;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.Extensions;

namespace MapinfoWrapper.Geometries.Lines
{
	public class Line : Geometry, ILine
	{
		public Line(IVariable variable) : base(variable)
		{ }

		public decimal GetLength(string unit)
		{
		    string expression = base.Variable.GetExpression();
            string length = base.mapinfoinstance.Evaluate("ObjectLen({0},{1})".FormatWith(expression, unit.InQuotes()));
			return Convert.ToDecimal(length);
		}
	}
}
