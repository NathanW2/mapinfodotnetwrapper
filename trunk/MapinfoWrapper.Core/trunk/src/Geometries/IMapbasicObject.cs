using MapInfo.Wrapper.MapbasicOperations;

namespace MapInfo.Wrapper.Geometries
{
    public interface IMapbasicObject
    {
        ObjectTypeEnum ObjectType { get; }
        IVariable Variable { get; }
    }
}
