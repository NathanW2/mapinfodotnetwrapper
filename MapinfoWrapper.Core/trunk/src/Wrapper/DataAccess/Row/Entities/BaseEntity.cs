using System;

namespace MapinfoWrapper.DataAccess.RowOperations.Entities
{
    /// <summary>
    /// Represents a basic row in a Mapinfo table.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Returns the row id for the current record in the attached table.
        /// </summary>
        public int RowId { get; internal set; }

        /// <summary>
        /// Returns the value for the current record using the supplied column.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object Get(string columnName)
        {
            if (RowId == 0 || this.reader == null)
                throw new NotSupportedException("The current row has not been inserted to a table so rows can not be read.");

            return this.reader.Get(columnName);
        }

        /// <summary>
        /// Returns the <see cref="Table"/> that this record is contained in.  Returns null if the
        /// record is not inserted into a table
        /// </summary>
        [MapinfoIgnore]
        public Table AttachedTo { get; internal set; }

        /// <summary>
        /// Returns the <see cref="IDataReader"/> for the current record.  This is here to support
        /// non-strong typed records.
        /// </summary>
        protected internal IDataReader reader;

        /// <summary>
        /// Returns a <see cref="EntityState"/> representing the current state of the entity.
        /// </summary>
        [MapinfoIgnore]
        public EntityState State 
        {
            get
            {
                if (RowId == 0 || this.reader == null || AttachedTo == null)
                {
                    return EntityState.NotInserted;
                }
                return EntityState.Attached;
            }
        }

        public enum EntityState
        {
            Attached = 0,
            NotInserted = 1
        }
    }

    /// <summary>
    /// An attribute used to mark a property to be ignored when being loaded with data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property , Inherited = false, AllowMultiple = true)]
    internal sealed class MapinfoIgnore : Attribute
    {
        public MapinfoIgnore()
        {}
    }
}
