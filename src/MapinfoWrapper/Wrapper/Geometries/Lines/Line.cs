using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Geometries.Lines
{
	public class Line : Geometry, ILine
	{
		public Line(IVariableExtender variable) : this(IoC.Resolve<IMapinfoWrapper>(), variable)
		{
		}

		public Line(IMapinfoWrapper wrapper, IVariableExtender variable) : base(wrapper, variable)
		{
		}

		public decimal GetLength(string unit)
		{
			string length = base.mapinfoinstance.Evaluate("ObjectLen({0},{1})".FormatWith(base.expression, unit.InQuotes()));
			return Convert.ToDecimal(length);
		}

	}
}
