using MapinfoWrapper.DataAccess.RowOperations.Entities;

namespace Wrapper.Example.Tables
{
    /// <summary>
    /// This is the entity object for the table worlds.TAB, this entity contains properties that 
    /// match all the columns in the worlds table and allows for strong typed access to the column data
    /// in Mapinfo.  An entity represents one row in a given table.
    /// 
    /// <para>There is no binding an entity and it's table. For instance you could have World.Tab and Worlds2.Tab
    /// as long as the column names match you can use it as the tables entity type.</para>
    /// 
    /// <para>This class inherits from <see cref="T:MapinfoWrapper.TableOperations.RowOperations.Entities.MappableEntity"/>
    /// which allows access to the obj column and rowid column in the table.</para>
    /// </summary>
    /// <remarks>NOTE! I am in the process of writing a tool that will generate these types of entities
    /// from a Mapinfo Tab file.</remarks>
    [UsesWrapper]
    public class World : MappableEntity, IWorld
    {
        public string Country { get; set; }
        public string Capital { get; set; }
        public string Continent { get; set; }
        public int Numeric_code { get; set; }
        public string FIPS { get; set; }
        public string ISO_2 { get; set; }
        public string ISO_3 { get; set; }
        public int Pop_1994 { get; set; }
        public decimal Pop_Grw_Rt { get; set; }
        public int Pop_Male { get; set; }
        public int Pop_Fem { get; set; }
        public int Pop_0_14 { get; set; }
        public int Pop_15_64 { get; set; }
        public int Pop_65Plus { get; set; }
        public int Male_0_14 { get; set; }
        public int Male_15_64 { get; set; }
        public int Male_65Plus { get; set; }
        public int Fem_0_14 { get; set; }
        public int Fem_15_64 { get; set; }
        public int Fem_65Plus { get; set; }
        public int Pop_Urban { get; set; }
        public int Pop_Rural { get; set; }
        public int Pop_Urb_Male { get; set; }
        public int Pop_Urb_Fem { get; set; }
        public int Pop_Rur_Male { get; set; }
        public int Pop_Rur_Fem { get; set; }
        public decimal Arable_Pct { get; set; }
        public decimal Literacy { get; set; }
        public decimal Inflat_Rate { get; set; }
        public decimal Unempl_Rate { get; set; }
        public decimal Indust_Growth { get; set; }
        public int ColorCode { get; set; }
    }
}
