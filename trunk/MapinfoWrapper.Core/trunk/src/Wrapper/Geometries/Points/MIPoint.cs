namespace MapinfoWrapper.Geometries.Points
{
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;

    /// <summary>
    /// Represents a point object in Mapinfo.  Provides real time access to properties
    /// and method for working with a Mapinfo point object.
    /// </summary>
    public class MIPoint : Geometry
    {
        internal MIPoint(MapinfoSession MISession, IVariable variable)
            : base(MISession, variable)
        { }
    }
}
