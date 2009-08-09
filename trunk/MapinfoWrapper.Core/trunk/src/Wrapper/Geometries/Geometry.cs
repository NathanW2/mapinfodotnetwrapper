namespace MapinfoWrapper.Geometries
{
    public abstract class Geometry
    {
        public abstract Coordinate Centroid();
        internal abstract string ToBasicCreateCommand();
    }
}