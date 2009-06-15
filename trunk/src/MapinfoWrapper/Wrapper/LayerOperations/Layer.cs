using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.LayerOperations
{
    /// <summary>
    /// A wrapper object around a Mapinfo layer, contains methods used to get information about the specifed
    /// layer.
    /// </summary>
    public class Layer
    {
        int layernumber;
        string layername;
        IMapinfoWrapper wrapper;

        private Layer(IMapinfoWrapper mapinfoInstance,int layerNumber)
        {
            this.layernumber = layerNumber;
            this.wrapper = mapinfoInstance;
        }

        private Layer(IMapinfoWrapper mapinfoInstance, string layerName)
        {
            this.layername = layerName;
            this.wrapper = mapinfoInstance;
        }

        /// <summary>
        /// Creates a new <see cref="T:Layer"/> using the layer number.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <param name="layerNumber">The number of the layer.</param>
        /// <returns>An instance of <see cref="T:Layer"/>.</returns>
        public static Layer GetLayerFromNumber(IMapinfoWrapper wrapper,int layerNumber)
        {
            return new Layer(wrapper,layerNumber);
        }

        /// <summary>
        /// Creates a new <see cref="T:Layer"/> using the layer name.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <param name="layerName">The name of the layer.</param>
        /// <returns>An instance of <see cref="T:Layer"/>.</returns>
        public static Layer GetLayerFromName(IMapinfoWrapper wrapper,string layerName)
        {
            return new Layer(wrapper,layerName);
        }
    }
}
