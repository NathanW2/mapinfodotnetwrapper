namespace Wrapper.Example.Tables
{
    /// <summary>
    /// Interface for the world table.
    /// </summary>
    public interface IWorld
    {
        string Country { get; set; }
        string Capital { get; set; }
        string Continent { get; set; }
        int Numeric_code { get; set; }
        string FIPS { get; set; }
        string ISO_2 { get; set; }
        string ISO_3 { get; set; }
        int Pop_1994 { get; set; }
        decimal Pop_Grw_Rt { get; set; }
        int Pop_Male { get; set; }
        int Pop_Fem { get; set; }
        int Pop_0_14 { get; set; }
        int Pop_15_64 { get; set; }
        int Pop_65Plus { get; set; }
        int Male_0_14 { get; set; }
        int Male_15_64 { get; set; }
        int Male_65Plus { get; set; }
        int Fem_0_14 { get; set; }
        int Fem_15_64 { get; set; }
        int Fem_65Plus { get; set; }
        int Pop_Urban { get; set; }
        int Pop_Rural { get; set; }
        int Pop_Urb_Male { get; set; }
        int Pop_Urb_Fem { get; set; }
        int Pop_Rur_Male { get; set; }
        int Pop_Rur_Fem { get; set; }
        decimal Arable_Pct { get; set; }
        decimal Literacy { get; set; }
        decimal Inflat_Rate { get; set; }
        decimal Unempl_Rate { get; set; }
        decimal Indust_Growth { get; set; }
        int ColorCode { get; set; }
    }
}
