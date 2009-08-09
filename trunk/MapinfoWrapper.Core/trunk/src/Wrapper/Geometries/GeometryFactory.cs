namespace MapinfoWrapper.Geometries
{
    using MapinfoWrapper.Core;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Core.Internals;
    using MapinfoWrapper.DataAccess;
    using MapinfoWrapper.Geometries.Lines;
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;
    using MapinfoWrapper.Wrapper.Geometries;

	/// <summary>
	/// A geometry factory that can be used to create new geometry objects.
	/// </summary>
    internal class GeometryFactory : IGeometryFactory
    {
        private readonly MapinfoSession misession;
        private readonly IVariableFactory variablefactory;

	    public GeometryFactory(MapinfoSession MISession)
	    {
	        this.misession = MISession;
            this.variablefactory = new VariableFactory(MISession);
	    }

        internal GeometryFactory(MapinfoSession MISession, IVariableFactory variableFactory)
        {
            this.misession = MISession;
            this.variablefactory = variableFactory;
        }

        /// <summary>
        /// Creates a new line object.
        /// Returns a <see cref="Line"/> which can be inserted into a <see cref="Table"/>
        /// </summary>
        /// <param name="start">The <see cref="Coordinate"/> for the start of the line.</param>
        /// <param name="end">The <see cref="Coordinate"/> for the end of the line.</param></param>
        /// <returns>A new <see cref="MILine"/> object.</returns>
    	public Line CreateLine(Coordinate start, Coordinate end)
    	{
            return new Line(start, end);
    	}
    	
    	/// <summary>
        /// Creates a new point object in Mapinfo. 
        /// Returns a <see cref="Point"/> which can be inserted into a <see cref="Table"/>
        /// </summary>
        /// <param name="location">A <see cref="Coordinate"/> that contains coordinates at which to create the point.</param>
        /// <returns>A new <see cref="Point"/> object.</returns>
        public Point CreatePoint(Coordinate location)
        {
    	    return new Point(location);
        }

     }
}
