using MapinfoWrapper.Core;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Geometries.Lines;
using MapinfoWrapper.Geometries.Points;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Wrapper.Geometries;

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
        /// Creates a new line object in Mapinfo. 
        /// Returns a <see cref="TEntity:MapinfoWrapper.Geometries.Lines.Line"/> which can be inserted into a
        /// <see cref="TEntity:MapinfoWrapper.TableOperations.Table"/>
        /// <para>This function will create a new object variable in Mapinfo with a modified GUID as its name.</para>
        /// </summary>
        /// <param name="start">The <see cref="TEntity:MapinfoWrapper.Geometries.Coordinate"/> for the start of the line.</param>
        /// <param name="end">The <see cref="TEntity:MapinfoWrapper.Geometries.Coordinate"/> for the end of the line.</param></param>
        /// <returns>A new <see cref="TEntity:MapinfoWrapper.Geometries.Lines.ILine"/>.</returns>
    	public ILine CreateLine(Coordinate start, Coordinate end)
    	{
            IVariable variable = variablefactory.CreateNewWithGUID(Variable.VariableType.Object);
            this.wrapper.RunCommand("Create Line Into Variable {0} ({1},{2})({3},{4})".FormatWith(variable.GetExpression(), start.X, start.Y, end.X, end.Y));
    		return new Line(variable);
    	}
    	
    	/// <summary>
        /// Creates a new point object in Mapinfo. 
        /// Returns a <see cref="TEntity:MapinfoWrapper.Geometries.Points.Point"/> which can be inserted into a
        /// <see cref="TEntity:MapinfoWrapper.TableOperations.Table"/>
        /// <para>This function will create a new object variable in Mapinfo with a modified GUID as its name.</para>
        /// </summary>
        /// <param name="location">A <see cref="TEntity:MapinfoWrapper.Geometries.Coordinate"/> that contains coordinates at
        /// which to create the point.</param>
        /// <returns>A new point object.</returns>
        public Point CreatePoint(Coordinate location)
        {
    	    IVariable variable = variablefactory.CreateNewWithGUID(Variable.VariableType.Object);
            this.wrapper.RunCommand("Create Point Into Variable {0} ({1},{2}) ".FormatWith(variable.GetExpression(), location.X, location.Y));
            return new Point(variable);
        }

        /// <summary>
        /// Returns the a <see cref="TEntity:MapinfoWrapper.Geometries.Geometry"/> for the 
        /// supplied <see cref="TEntity:MapinfoWrapper.MapbasicOperations.IVariable"/>.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public Geometry GetGeometryFromVariable(IVariable variable)
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
                    return new Line(variable);
                case ObjectTypeEnum.OBJ_TYPE_PLINE:
                    return new Polyline(variable);
                case ObjectTypeEnum.OBJ_TYPE_POINT:
                    return new Point(variable);
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
                    break;
            }
            return geo;
        }
    }
}
