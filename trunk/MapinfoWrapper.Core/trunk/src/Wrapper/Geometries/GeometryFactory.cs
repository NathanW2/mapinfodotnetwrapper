using MapinfoWrapper.Core;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Geometries.Lines;
using MapinfoWrapper.Geometries.Points;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Geometries
{
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

        public GeometryFactory(MapinfoSession MISession, IVariableFactory variableFactory)
        {
            this.misession = MISession;
            this.variablefactory = variableFactory;
        }

        /// <summary>
        /// Creates a new line object in Mapinfo. 
        /// Returns a <see cref="T:MapinfoWrapper.Geometries.Lines.MILine"/> which can be inserted into a
        /// <see cref="T:MapinfoWrapper.DataAccess.Table"/>
        /// <para>This function will create a new object variable in Mapinfo with a modified GUID as its name.</para>
        /// </summary>
        /// <param name="start">The <see cref="T:MapinfoWrapper.Geometries.Coordinate"/> for the start of the line.</param>
        /// <param name="end">The <see cref="T:MapinfoWrapper.Geometries.Coordinate"/> for the end of the line.</param></param>
        /// <returns>A new <see cref="T:MapinfoWrapper.Geometries.Lines.IMILine"/>.</returns>
    	public IMILine CreateLine(Coordinate start, Coordinate end)
    	{
            IVariable variable = variablefactory.CreateNewWithGUID(Variable.VariableType.Object);
            this.misession.RunCommand("Create MILine Into Variable {0} ({1},{2})({3},{4})".FormatWith(variable.GetExpression(), start.X, start.Y, end.X, end.Y));
            return new MILine(this.misession, variable);
    	}
    	
    	/// <summary>
        /// Creates a new point object in Mapinfo. 
        /// Returns a <see cref="T:MapinfoWrapper.Geometries.Points.MIPoint"/> which can be inserted into a
        /// <see cref="T:MapinfoWrapper.DataAccess.Table"/>
        /// <para>This function will create a new object variable in Mapinfo with a modified GUID as its name.</para>
        /// </summary>
        /// <param name="location">A <see cref="T:MapinfoWrapper.Geometries.Coordinate"/> that contains coordinates at
        /// which to create the point.</param>
        /// <returns>A new point object.</returns>
        public MIPoint CreatePoint(Coordinate location)
        {
    	    IVariable variable = variablefactory.CreateNewWithGUID(Variable.VariableType.Object);
            this.misession.RunCommand("Create MIPoint Into Variable {0} ({1},{2}) ".FormatWith(variable.GetExpression(), location.X, location.Y));
    	    return new MIPoint(this.misession, variable);
        }

        /// <summary>
        /// Returns the a <see cref="T:MapinfoWrapper.Geometries.Geometry"/> for the 
        /// supplied <see cref="T:MapinfoWrapper.MapbasicOperations.IVariable"/>.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public Geometry GetGeometryFromVariable(IVariable variable)
        {
            Guard.AgainstNull(variable, "variable");

            Geometry geo = new Geometry(misession, variable);
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
