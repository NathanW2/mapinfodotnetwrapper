using System;
using MapinfoWrapper.Geometries.Lines;
using MapinfoWrapper.Geometries.Points;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.MapbasicOperations;
namespace MapinfoWrapper.Wrapper.Geometries
{
    public interface IGeometryFactory
    {
        ILine CreateLine(Coordinate start, Coordinate end);
        Point CreatePoint(Coordinate location);
        Geometry GetGeometryFromVariable(IVariable variable);
    }
}
