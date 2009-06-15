using System;
using System.Collections.Generic;
using System.Linq;
using Wrapper.MapbasicOperations;
using Wrapper.Extensions;
using Wrapper.Core.IoC;

namespace Wrapper.ObjectOperations.Lines
{
	public interface ILine
	{
		decimal GetLength(string unit);
	}
}
