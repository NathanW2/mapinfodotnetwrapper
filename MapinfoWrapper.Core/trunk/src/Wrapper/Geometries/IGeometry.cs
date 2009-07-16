namespace MapinfoWrapper.Geometries
{
    public interface IGeometry : IMapbasicObject
    {
        bool Contains(Geometry mapinfoObject);
        Coordinate Centroid
        {
            get;
        }
    }
}
