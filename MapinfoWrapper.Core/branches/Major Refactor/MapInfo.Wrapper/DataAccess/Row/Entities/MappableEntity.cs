using Mapinfo.Wrapper.Geometries;

namespace Mapinfo.Wrapper.DataAccess.Row.Entities
{
    /// <summary>
    /// Represents a mappable row in a table table.
    /// <para>You will need to inherit from this type if you need object support in your entities.</para>
    /// </summary>
    public class MappableEntity : BaseEntity
    {
        /// <summary>
        /// Gets and set the <see cref="Geometry"/> for the current record.
        /// <para>This property represents the obj column for the current record in the attaced mapinfo
        /// table.</para>
        /// </summary>
        public Geometry obj { get; set; }
    }
}
