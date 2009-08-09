namespace MapinfoWrapper.DataAccess.RowOperations.Entities
{
    using MapinfoWrapper.Geometries;

    /// <summary>
    /// Represents a mappable row in a table table.
    /// </summary>
    public class MappableEntity : BaseEntity
    {
        /// <summary>
        /// Returns a <see cref="Geometry"/> for the current record.
        /// <para>This property represents the obj column for the current record in the attaced mapinfo
        /// table.</para>
        /// </summary>
        public Geometry obj { get; set; }
    }
}
