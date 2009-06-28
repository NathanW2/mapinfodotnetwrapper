using MapinfoWrapper.DataAccess.RowOperations.Entities;

namespace Wrapper.Example.Tables
{
    public class WorldCapEntity : MappableEntity
    {
        public string Capital { get; set; }
        public string Country { get; set; }
        public decimal Cap_Pop { get; set; }
    }
}
