using MapinfoWrapper.Core;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.Core.IoC;
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
    public class GeometryFactory
    {
	    private readonly IMapinfoWrapper wrapper = IoC.Resolve<IMapinfoWrapper>();
	    private readonly IVariableFactory variablefactory = IoC.Resolve<IVariableFactory>();
    	
    	/// <summary>
    	/// Creates a new line in Mapinfo into the the supplied variable.
    	/// <para>If you need to store this object in a table, set the obj column of
    	/// the table as the return value of this function.</para>
    	/// </summary>
    	/// <param name="start">The coordinate of the start of the line.</param>
    	/// <param name="end">The coordinate of the end of the line.</param>
    	/// <param name="variable">The variable that will store the point object.</param>
    	/// <returns>A new line object, which can be used to get information about the object.</returns>
    	public ILine CreateLine(Coordinate start, Coordinate end)
    	{
            IVariable variable = variablefactory.CreateNewWithGUID(Variable.VariableType.Object);
            this.wrapper.RunCommand("Create Line Into Variable {0} ({1},{2})({3},{4})".FormatWith(variable.GetExpression(), start.X, start.Y, end.X, end.Y));
    		return new Line(variable);
    	}
    	
    	/// <summary>
        /// Creates a new point object in Mapinfo and returns a point object, which can be used to get information about
        /// the object or stored in a table.
        /// </summary>
        /// <param name="location">A coordinate that contains the X and Y to create the point.</param>
        /// <param name="variable">The variable that will be used to store the newly created object.</param>
        /// <returns>A new point object.</returns>
        public Point CreatePoint(Coordinate location)
        {
    	    IVariable variable = variablefactory.CreateNewWithGUID(Variable.VariableType.Object);
            this.wrapper.RunCommand("Create Point Into Variable {0} ({1},{2}) ".FormatWith(variable.GetExpression(), location.X, location.Y));
            return new Point(variable);
        }

        public Geometry GetGeometryFromObj(IVariable variable)
        {
            Guard.AgainstNull(variable, "variable");

            Geometry geo = new Geometry(variable);
            switch (geo.ObjectType)
            {
                case ObjectTypeEnum.OBJ_TYPE_ARC:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_ELLIPSE:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_LINE:
                    return new Lines.Line(variable);
                case ObjectTypeEnum.OBJ_TYPE_PLINE:
                    break;
                case ObjectTypeEnum.OBJ_TYPE_POINT:
                    return new Points.Point(variable);
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
                default:
                    return null;
            }
            return geo;
        }
    }
}
