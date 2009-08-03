namespace MapinfoWrapper.Geometries
{
    using MapinfoWrapper.Core;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Core.Internals;
    using MapinfoWrapper.DataAccess;
    using MapinfoWrapper.Geometries.Lines;
    using MapinfoWrapper.Geometries.Points;
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;

	/// <summary>
	/// A geometry factory that can be used to create new geometry objects.
	/// </summary>
    public class GeometryFactory : IGeometryFactory
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
        /// Creates a new line object in Mapinfo. 
        /// Returns a <see cref="MILine"/> which can be inserted into a
        /// <see cref="MapinfoWrapper.DataAccess.Table"/>
        /// <para>This function will create a new object variable in Mapinfo with a modified GUID as its name.</para>
        /// </summary>
        /// <param name="start">The <see cref="Coordinate"/> for the start of the line.</param>
        /// <param name="end">The <see cref="Coordinate"/> for the end of the line.</param></param>
        /// <returns>A new <see cref="MILine"/> object.</returns>
    	public MILine CreateLine(Coordinate start, Coordinate end)
    	{
            IVariable variable = variablefactory.CreateNewWithGUID(Variable.VariableType.Object);
            this.misession.RunCommand("Create Line Into Variable {0} ({1},{2})({3},{4})".FormatWith(variable.GetExpression(), start.X, start.Y, end.X, end.Y));
            return new MILine(this.misession, variable);
    	}
    	
    	/// <summary>
        /// Creates a new point object in Mapinfo. 
        /// Returns a <see cref="MIPoint"/> which can be inserted into a
        /// <see cref="Table"/>
        /// <para>This function will create a new object variable in Mapinfo with a modified GUID as its name.</para>
        /// </summary>
        /// <param name="location">A <see cref="Coordinate"/> that contains coordinates at
        /// which to create the point.</param>
        /// <returns>A new <see cref="MIPoint"/> object.</returns>
        public MIPoint CreatePoint(Coordinate location)
        {
    	    IVariable variable = variablefactory.CreateNewWithGUID(Variable.VariableType.Object);
            this.misession.RunCommand("Create Point Into Variable {0} ({1},{2}) ".FormatWith(variable.GetExpression(), location.X, location.Y));
    	    return new MIPoint(this.misession, variable);
        }

        /// <summary>
        /// Returns the a <see cref="MIGeometry"/> for the supplied <see cref="IVariable"/>.  This function
        /// is really only for internal use.  See <see cref="Variable"/> for notes about Mapbasic variables
        /// in the MapinfoWrapper API.
        /// </summary>
        /// <param name="variable">An object variable</param>
        /// <returns></returns>
        public MIGeometry GetGeometryFromVariable(IVariable variable)
        {
            Guard.AgainstNull(variable, "variable");

            MIGeometry geo = new MIGeometry(misession, variable);
            switch (geo.ObjectType)
            {
                case ObjectTypeEnum.OBJ_TYPE_ARC:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_ELLIPSE:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_LINE:
                    return new MILine(misession, variable);
                case ObjectTypeEnum.OBJ_TYPE_PLINE:
                    return new MIPolyline(misession, variable);
                case ObjectTypeEnum.OBJ_TYPE_POINT:
                    return new MIPoint(misession, variable);
                case ObjectTypeEnum.OBJ_TYPE_FRAME:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_REGION:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_RECT:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_ROUNDRECT:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_TEXT:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_MPOINT:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_COLLECTION:
                    break;
                case ObjectTypeEnum.NoObject:
                    return null;
            }
            return geo;
        }
    }
}
