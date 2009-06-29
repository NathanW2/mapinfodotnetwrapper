using MapinfoWrapper.Geometries;

namespace MapinfoWrapper.DataAccess.RowOperations.Entities
{
    public class MappableEntity : BaseEntity
    {
        public IGeometry obj { get; set; }
    }
}
