using MapInfo.Wrapper.DataAccess;
using MapInfo.Wrapper.Geometries.Lines;
using MapInfo.Wrapper.Geometries.Points;
using MapInfo.Wrapper.MapbasicOperations;
using MapInfo.Wrapper.Mapinfo;


namespace MapInfo.Wrapper.Geometries
{
    /// <summary>
	/// A geometry factory that can be used to create new geometry objects.
	/// </summary>
    internal class GeometryFactory : IGeometryFactory
    {
        private readonly MapInfoSession misession;
        private readonly VariableFactory variablefactory;

	    public GeometryFactory(MapInfoSession MISession)
	    {
	        this.misession = MISession;
            this.variablefactory = new VariableFactory(MISession);
	    }

        internal GeometryFactory(MapInfoSession MISession, VariableFactory variableFactory)
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
