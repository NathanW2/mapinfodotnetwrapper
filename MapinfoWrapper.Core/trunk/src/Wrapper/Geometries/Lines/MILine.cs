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
	public class MILine : MIGeometry, IMILine
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

    public class MIGeometry : IGeometry
    {
        protected MapinfoSession misession;
        public MIGeometry(MapinfoSession MISession,IVariable variable)
        {

        }

        public bool Contains(IGeometry mapinfoObject)
        {
            throw new NotImplementedException();
        }

        public Coordinate Centroid
        {
            get { throw new NotImplementedException(); }
        }

        public ObjectTypeEnum ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public IVariable Variable
        {
            get { throw new NotImplementedException(); }
        }
    }
}
