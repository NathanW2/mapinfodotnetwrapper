﻿using Mapinfo.Wrapper.Core;
using MapInfo.Wrapper.Core.Exceptions;
using Mapinfo.Wrapper.Core.Extensions;
using Mapinfo.Wrapper.Exceptions;
using Mapinfo.Wrapper.LayerOperations;
using System;
using System.Collections.Generic;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.MapOperations
{
    public class MapWindow : Window
    {
        public MapWindow(int windowID, MapinfoSession mapinfo)
            : base(windowID, mapinfo)
        { }

        /// <summary>
        /// Gets or sets the Center X point for the window.
        /// </summary>
        public double CenterX
        {
            get
            {
                string strvalue = base.mapinfo.Eval(String.Format("MapperInfo({0},3)", this.ID));
                double centerx;
                if (double.TryParse(strvalue, System.Globalization.NumberStyles.Number, CultureGlobals.UsNumberFormat, out centerx))
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
                string strvalue = this.mapinfo.Eval(String.Format("MapperInfo({0},4)", this.ID));
                double centery;
                if (double.TryParse(strvalue, System.Globalization.NumberStyles.Number, CultureGlobals.UsNumberFormat, out centery))
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
                string strvalue = this.mapinfo.Eval(String.Format("MapperInfo({0},1)", this.ID));
                double zoom;
                if (double.TryParse(strvalue, System.Globalization.NumberStyles.Number, CultureGlobals.UsNumberFormat, out zoom))
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
                return this.mapinfo.Eval(String.Format("MapperInfo({0},12)", this.ID));
            }
        }

        /// <summary>
        /// Gets the coordsys for the window.
        /// </summary>
        public string CoordSys
        {
            get
            {
                return this.mapinfo.Eval(String.Format("MapperInfo({0},{1})", this.ID, (int)MapperInfoTypes.Coordsys_clause_with_bounds));
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
            this.mapinfo.Do(command);
        }

        /// <summary>
        /// Updates the centres of the map at the same time.
        /// You can also update the map center using the X and Y properties.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateCenters(double x, double y)
        {
            this.mapinfo.Do("Set Map Window " + this.ID +
                                    " Center ( " + String.Format(CultureGlobals.UsNumberFormat, "{0}", x) + " , " + String.Format(CultureGlobals.UsNumberFormat, "{0}", y) + ")");
        }

        public IList<Layer> Layers
        {
            get
            {
                List<Layer> layers = new List<Layer>();

                string slayers = this.mapinfo.Eval("MapperInfo({0},{1})".FormatWith(this.ID, (int)MapperInfoTypes.Layers));
                int numlayers = Convert.ToInt32(slayers);

                for (int i = 0; i < numlayers + 1; i++)
                {
                    layers.Add(new Layer(this.mapinfo, i, this));
                }
                return layers;
            }
        }
    }
}
