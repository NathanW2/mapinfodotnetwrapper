using MapinfoWrapper.Geometries.Lines;
using MapinfoWrapper.Geometries.Points;
using MapinfoWrapper.MapbasicOperations;

namespace MapinfoWrapper.Geometries
{
    internal interface IGeometryFactory
    {
        MILine CreateLine(Coordinate start, Coordinate end);
        MIPoint CreatePoint(Coordinate location);
        Geometry GetGeometryFromVariable(IVariable variable);
    }
}
