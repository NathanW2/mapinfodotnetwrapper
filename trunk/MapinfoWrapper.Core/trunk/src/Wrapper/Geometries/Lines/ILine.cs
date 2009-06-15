using System;
using System.Collections.Generic;
using System.Linq;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.IoC;

namespace MapinfoWrapper.Geometries.Lines
{
	public interface ILine
	{
		decimal GetLength(string unit);
	}
}
