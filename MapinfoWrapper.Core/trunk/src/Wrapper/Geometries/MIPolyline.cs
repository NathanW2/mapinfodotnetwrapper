using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Geometries
{
    public class MIPolyline : Geometry
    {
        internal MIPolyline(MapinfoSession MISession, IVariable variable)
            : base(MISession, variable)
        { }
    }
}
