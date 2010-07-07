namespace MapinfoWrapper.Geometries
{
    using MapinfoWrapper.MapbasicOperations;

    public interface IMapbasicObject
    {
        ObjectTypeEnum ObjectType { get; }
        IVariable Variable { get; }
    }
}
