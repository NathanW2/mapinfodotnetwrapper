using System;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Geometries.Lines
{
	public class MILine : Geometry, IMILine
	{
	    internal MILine(MapinfoSession MISession, IVariable variable)
            : base(MISession, variable)
	    { }

		public decimal GetLength(string unit)
		{
		    string expression = base.Variable.GetExpression();
            string length = base.misession.Evaluate("ObjectLen({0},{1})".FormatWith(expression, unit.InQuotes()));
			return Convert.ToDecimal(length);
		}
	}
}
