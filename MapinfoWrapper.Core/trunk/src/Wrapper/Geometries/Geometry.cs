namespace MapinfoWrapper.Geometries
{
    public abstract class Geometry
    {
        public abstract Coordinate Centroid();
        public abstract string ToBasicCreateCommand();
        public abstract string ToExtendedCreateString(string variableName);
        public abstract string ToExtendedCreateString(int windowID);
    }
}