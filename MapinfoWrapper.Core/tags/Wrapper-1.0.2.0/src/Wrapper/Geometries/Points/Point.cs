using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Wrapper.Geometries;

namespace MapinfoWrapper.Geometries.Points
{
    public class Point : Geometry
    {
    	public Point(IVariable variable)
    		: base(variable) { }

        /// <summary>
        /// Creates a new point object in Mapinfo. 
        /// Returns a <see cref="T:MapinfoWrapper.Geometries.Points.Point"/> which can be inserted into a
        /// <see cref="T:MapinfoWrapper.DataAccess.Table"/>
        /// <para>This function will create a new object variable in Mapinfo with a modified GUID as its name.</para>
        /// </summary>
        /// <param name="location">A <see cref="T:MapinfoWrapper.Geometries.Coordinate"/> that contains coordinates at
        /// which to create the point.</param>
        /// <returns>A new point object.</returns>
        public static Point CreatePoint(Coordinate location)
        {
            IGeometryFactory geofactory = ServiceLocator.GetInstance<IFactoryBuilder>().BuildGeomtryFactory();
            return (Point)geofactory.CreatePoint(location);
        }
    }
}
