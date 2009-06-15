using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.MapbasicOperations;

namespace MapinfoWrapper.Geometries
{
    public interface IGeometry : IMapbasicObject
    {
        bool Contains(Geometry mapinfoObject);
        Coordinate Centroid
        {
            get;
        }
    }
}
