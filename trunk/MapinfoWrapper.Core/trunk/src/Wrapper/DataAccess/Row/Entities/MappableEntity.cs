using MapinfoWrapper.Geometries;

namespace MapinfoWrapper.DataAccess.RowOperations.Entities
{
    /// <summary>
    /// Represents a mappable row in a table table.
    /// </summary>
    public class MappableEntity : BaseEntity
    {
        /// <summary>
        /// Returns a <see cref="IGeometry"/> for the current record.
        /// <para>This property represents the obj column for the current record in the attaced mapinfo
        /// table.</para>
        /// </summary>
        public IGeometry obj { get; set; }
    }
}
