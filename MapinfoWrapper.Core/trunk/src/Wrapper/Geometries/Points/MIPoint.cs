using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Geometries.Points
{
    public class MIPoint : Geometry
    {
        internal MIPoint(MapinfoSession MISession, IVariable variable)
            : base(MISession, variable)
        { }
    }
}
