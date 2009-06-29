using System;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Wrapper.Geometries;
using MapinfoWrapper.Core.IoC;

namespace MapinfoWrapper.Geometries.Lines
{
	public class Line : Geometry, ILine
	{
		public Line(IVariable variable) : base(variable)
		{ }

        /// <summary>
        /// Creates a new line object in Mapinfo. 
        /// Returns a <see cref="T:MapinfoWrapper.Geometries.Lines.Line"/> which can be inserted into a
        /// <see cref="T:MapinfoWrapper.DataAccess.Table"/>
        /// <para>This function will create a new object variable in Mapinfo with a modified GUID as its name.</para>
        /// </summary>
        /// <param name="start">The <see cref="T:MapinfoWrapper.Geometries.Coordinate"/> for the start of the line.</param>
        /// <param name="end">The <see cref="T:MapinfoWrapper.Geometries.Coordinate"/> for the end of the line.</param></param>
        /// <returns>A new <see cref="T:MapinfoWrapper.Geometries.Lines.ILine"/>.</returns>
        public static Line CreateLine(Coordinate start, Coordinate end)
        {
            IGeometryFactory geofactory = ServiceLocator.GetInstance<IFactoryBuilder>().BuildGeomtryFactory();
            return (Line)geofactory.CreateLine(start, end);
        }

		public decimal GetLength(string unit)
		{
		    string expression = base.Variable.GetExpression();
            string length = base.mapinfoinstance.Evaluate("ObjectLen({0},{1})".FormatWith(expression, unit.InQuotes()));
			return Convert.ToDecimal(length);
		}
	}
}
