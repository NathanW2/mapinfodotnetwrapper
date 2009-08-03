namespace MapinfoWrapper.Geometries.Lines
{
    using System;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;

    /// <summary>
    /// Represents a line object in Mapinfo.  Provides real time access to properties
    /// and method for working with a Mapinfo line object.
    /// </summary>
	public class MILine : Geometry, IMILine
	{
	    internal MILine(MapinfoSession MISession, IVariable variable)
            : base(MISession, variable)
	    { }

        /// <summary>
        /// Returns the length of the current line.
        /// </summary>
        /// <param name="unit">
        /// The unit that units will be returned in eg "m" for meters.
        /// <para>In future releases this will be changed to a enum type rather then a string.</para>
        /// </param>
        /// <returns>Returns a <see cref="decimal"/> for the length of the object.</returns>
		public decimal GetLength(string unit)
		{
		    string expression = base.Variable.GetExpression();
            string length = base.misession.Evaluate("ObjectLen({0},{1})".FormatWith(expression, unit.InQuotes()));
			return Convert.ToDecimal(length);
		}
	}
}
