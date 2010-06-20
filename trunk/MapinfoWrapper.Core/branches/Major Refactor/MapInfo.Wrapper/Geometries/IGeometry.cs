namespace Mapinfo.Wrapper.Geometries
{
    public interface IGeometry : IMapbasicObject
    {
        bool Contains(IGeometry mapinfoObject);
        Coordinate Centroid
        {
            get;
        }
    }
}
