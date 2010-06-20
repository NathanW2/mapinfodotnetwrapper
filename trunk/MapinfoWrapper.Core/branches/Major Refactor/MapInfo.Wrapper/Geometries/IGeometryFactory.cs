using Mapinfo.Wrapper.Geometries.Lines;
using Mapinfo.Wrapper.Geometries.Points;

namespace Mapinfo.Wrapper.Geometries
{
    internal interface IGeometryFactory
    {
        Line CreateLine(Coordinate start, Coordinate end);
        Point CreatePoint(Coordinate location);
    }
}
