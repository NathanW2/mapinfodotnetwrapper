namespace MapinfoWrapper.Geometries
{
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;

    public class MIPolyline : MIGeometry
    {
        internal MIPolyline(MapinfoSession MISession, IVariable variable)
            : base(MISession, variable)
        { }
    }
}
