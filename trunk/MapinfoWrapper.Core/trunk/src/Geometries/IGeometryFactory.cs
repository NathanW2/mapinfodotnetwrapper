using MapInfo.Wrapper.Geometries.Lines;
using MapInfo.Wrapper.Geometries.Points;

namespace MapInfo.Wrapper.Geometries
{
    internal interface IGeometryFactory
    {
        Line CreateLine(Coordinate start, Coordinate end);
        Point CreatePoint(Coordinate location);
    }
}
