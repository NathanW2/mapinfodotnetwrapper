namespace MapinfoWrapper.Geometries
{
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Wrapper.Geometries;

    internal interface IGeometryFactory
    {
        Line CreateLine(Coordinate start, Coordinate end);
        Point CreatePoint(Coordinate location);
    }
}
