using System.Globalization;

namespace Mapinfo.Wrapper.Core
{
    /// <summary>
    /// This class contains the culture information that can be used to parse string returned from MapInfo.
    /// </summary>
    public class CultureGlobals
    {
        /// <summary>
        /// The US culture info used when reading and sending information to and from Mapinfo.
        /// </summary>
        public static CultureInfo UscultureInfo = new CultureInfo("en-US");

        /// <summary>
        /// The US number format used to parse decimals from Mapinfo.
        /// </summary>
        public static NumberFormatInfo UsNumberFormat = UscultureInfo.NumberFormat;
    }
}
