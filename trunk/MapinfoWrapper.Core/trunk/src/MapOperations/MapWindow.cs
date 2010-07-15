using System;
using MapInfo.Wrapper.Core;
using MapInfo.Wrapper.Core.Wrappers;
using MapInfo.Wrapper.Exceptions;
using MapInfo.Wrapper.Mapinfo;

namespace MapInfo.Wrapper.MapOperations
{
    public class MapWindow : Window
    {
        private MapperInfoWrapper infowrapper;

        public MapWindow(int windowID, MapInfoSession map_info)
            : base(windowID, map_info)
        {
            this.infowrapper = new MapperInfoWrapper(map_info);
        }

        /// <summary>
        /// Gets or sets the Center X point for the window.
        /// </summary>
        public double CenterX
        {
            get
            {
                string strvalue = base.map_info.Eval(String.Format("MapperInfo({0},3)", this.ID));
                double centerx;
                if (double.TryParse(strvalue, System.Globalization.NumberStyles.Number, Globals._usNumberFormat, out centerx))
                    return centerx;
                else
                    throw new MapbasicException(string.Format("Invalid return type from MapBasic. Expected double. Return value: {0}", strvalue));
            }
            set
            {
                this.UpdateCenters(value, this.CenterY);
            }
        }

        /// <summary>
        /// Gets or sets the Center Y point for the window.
        /// </summary>
        public double CenterY
        {
            get
            {
                string strvalue = this.map_info.Eval(String.Format("MapperInfo({0},4)", this.ID));
                double centery;
                if (double.TryParse(strvalue, System.Globalization.NumberStyles.Number, Globals._usNumberFormat, out centery))
                    return centery;
                else
                    throw new MapbasicException(string.Format("Invalid return type from MapBasic. Expected double. Return value: {0}", strvalue));
            }
            set
            {
                this.UpdateCenters(this.CenterX, value);
            }
        }

        /// <summary>
        /// Gets or sets the zoom width for the window.
        /// </summary>
        public double Zoom
        {
            get
            {
                string strvalue = this.map_info.Eval(String.Format("MapperInfo({0},1)", this.ID));
                double zoom;
                if (double.TryParse(strvalue, System.Globalization.NumberStyles.Number, Globals._usNumberFormat, out zoom))
                    return zoom;
                else
                    throw new MapbasicException(string.Format("Invalid return type from MapBasic. Expected double. Return value: {0}", strvalue));
            }
            set
            {
                this.UpdateZoom(value, this.ZoomUnit);
            }
        }

        /// <summary>
        /// Gets the zoom width for the window.
        /// </summary>
        public string ZoomUnit
        {
            get
            {
                return this.infowrapper.MapperInfo(this.ID, MapperInfoTypes.MAPPER_INFO_DISTUNITS);
            }
        }

        /// <summary>
        /// Gets the coordsys for the window.
        /// </summary>
        public string CoordSys
        {
            get
            {
                return this.infowrapper.MapperInfo(this.ID,MapperInfoTypes.MAPPER_INFO_COORDSYS_CLAUSE_WITH_BOUNDS);
            }
        }

        /// <summary>
        /// Updates the current zoom of the map.
        /// </summary>
        /// <param name="zoom">The zoom amount.</param>
        /// <param name="unit">The distance unit eg m,km etc</param>
        public void UpdateZoom(double zoom, string unit)
        {
            string command = string.Format("Set Map Window {0} Zoom {1} Units \"{2}\"", this.ID, zoom, unit);
            this.map_info.Do(command);
        }

        /// <summary>
        /// Updates the centres of the map at the same time.
        /// You can also update the map center using the X and Y properties.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateCenters(double x, double y)
        {
            this.map_info.Do("Set Map Window " + this.ID +
                                    " Center ( " + String.Format(Globals._usNumberFormat, "{0}", x) + " , " + String.Format(Globals._usNumberFormat, "{0}", y) + ")");
        }
    }
}
