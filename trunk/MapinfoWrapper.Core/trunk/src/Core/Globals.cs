using System.Globalization;

namespace MapinfoWrapper.Core
{
    public class Globals
    {
        /// <summary>
        /// The US culture info used when reading and sending information to and from Mapinfo.
        /// </summary>
        public static CultureInfo uscultureInfo = new CultureInfo("en-US");

        /// <summary>
        /// The US number format used to parse decimals from Mapinfo.
        /// </summary>
        public static NumberFormatInfo _usNumberFormat = uscultureInfo.NumberFormat;
    }
}
