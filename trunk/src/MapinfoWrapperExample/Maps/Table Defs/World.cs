using System;
using Wrapper.Extensions;
using Wrapper.TableOperations.Row;

namespace Wrapper.Example.Tables
{
    /// <summary>
    /// This is the entity object for the table worlds.TAB, this entity contains the
    /// mapping and conversions to and from Mapinfo's string to return right type.
    /// 
    /// <para>This class inherits from MappableRow which allows access to the obj column in the table, it also implements
    /// <see cref="T:IWorld"/> so that a change in the interface will force the change here also.</para>
    /// 
    /// <para>Properties in this class use the underlying base.GetValue method which takes a lambda expression which is used
    /// to force strong typing to avoid spelling mistakes in column names.</para>
    /// 
    /// <para>NOTE! Only a handfull of properties will be implemented just for examples sake.  You can get access to the columns still
    /// by doing something like World.GetValue(row => row.{ColumnName})</para>
    /// </summary>
    /// <remarks>NOTE! In future releases of the OLE wrapper, I will be inculding a tool that will generate this kind table entity from a
    /// supplied .TAB file automaticlly, it will also create the interface for the table that is used as a template.</remarks>
    [UsesWrapper]
    public class World : MappableRow<IWorld>, IWorld
    {

        #region IWorld Members

        public string Country
        {
            get
            {
                return (string)base.GetValue(row => row.Country);
            }
            set
            {
                base.SetValue(row => row.Country, value.InQuotes());
            }
        }

        public string Capital
        {
            get
            {
                return (string)base.GetValue(row => row.Capital);
            }
            set
            {
                base.SetValue(row => row.Capital, value.InQuotes());
            }
        }


        public int Numeric_code
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Numeric_code));
            }
            set
            {
                base.SetValue(row => row.Numeric_code, value);
            }
        }

        #region Only getters have been implemented Passed this point

        public string Continent
        {
            get
            {
                return (string)base.GetValue(row => row.Continent);
            }
            set
            {
                base.SetValue(row => row.Continent,value.InQuotes());
            }
        }

        public string FIPS
        {
            get
            {
                return (string)base.GetValue(row => row.FIPS);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ISO_2
        {
            get
            {
                return (string)base.GetValue(row => row.ISO_2);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ISO_3
        {
            get
            {
                return (string)base.GetValue(row => row.ISO_3);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_1994
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_1994));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal Pop_Grw_Rt
        {
            get
            {
                return Convert.ToDecimal(base.GetValue(row => row.Pop_Grw_Rt));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_Male
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_Male));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_Fem
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_Fem));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_0_14
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_0_14));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_15_64
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_15_64));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_65Plus
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_65Plus));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Male_0_14
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Male_0_14));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Male_15_64
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Male_15_64));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Male_65Plus
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Male_65Plus));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Fem_0_14
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Fem_0_14));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Fem_15_64
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Fem_15_64));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Fem_65Plus
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Fem_65Plus));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_Urban
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_Urban));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_Rural
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_Rural));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_Urb_Male
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_Urb_Male));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_Urb_Fem
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_Urb_Fem));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_Rur_Male
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_Rur_Male));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Pop_Rur_Fem
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.Pop_Rur_Fem));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal Arable_Pct
        {
            get
            {
                return Convert.ToDecimal(base.GetValue(row => row.Arable_Pct));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal Literacy
        {
            get
            {
                return Convert.ToDecimal(base.GetValue(row => row.Literacy));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal Inflat_Rate
        {
            get
            {
                return Convert.ToDecimal(base.GetValue(row => row.Inflat_Rate));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal Unempl_Rate
        {
            get
            {
                return Convert.ToDecimal(base.GetValue(row => row.Unempl_Rate));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal Indust_Growth
        {
            get
            {
                return Convert.ToDecimal(base.GetValue(row => row.Indust_Growth));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int ColorCode
        {
            get
            {
                return Convert.ToInt32(base.GetValue(row => row.ColorCode));
            }
            set
            {
                throw new NotImplementedException();
            }
        } 
        #endregion

        #endregion
    }
}
