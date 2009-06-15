using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.MapbasicOperations;
using Wrapper.Extensions;
using Wrapper.Core.IoC;

namespace Wrapper.ObjectOperations.Lines
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
