namespace MapinfoWrapper.Geometries
{
    public interface IGeometry : IMapbasicObject
    {
        bool Contains(MIGeometry mapinfoObject);
        Coordinate Centroid
        {
            get;
        }
    }
}
