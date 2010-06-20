namespace Mapinfo.Wrapper.DataAccess
{
    /// <summary>
    /// An enum containing different methods for opening VM GRD files.
    /// </summary>
    public enum GridHandleEnum
    {
        /// <summary>
        /// Treats all VM GRD files a Grid Layers when opened.
        /// </summary>
        VMGrid,
        /// <summary>
        /// Treats all VM GRD files as Raster Layers when opened.
        /// </summary>
        VMRaster,
        /// <summary>
        /// Treats all VM GRD files as Raster or Grid depending on existence of RasterStyle 6.1 tag in TAB file.
        /// </summary>
        VMDefault
    }
}
