using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.LayerOperations
{
    /// <summary>
    /// A wrapper object around a Mapinfo layer, contains methods used to get information about the specifed
    /// layer.
    /// </summary>
    public class Layer
    {
        private readonly int layernumber;
        private readonly string layername;
        IMapinfoWrapper wrapper;

        internal Layer(string layerName)
            : this(null, layerName)
        {}

        internal Layer(int layerNumber)
            : this(null, layerNumber)
        { }

        internal Layer(IMapinfoWrapper mapinfoInstance, int layerNumber)
        {
            this.layernumber = layerNumber;
            this.wrapper = mapinfoInstance;
        }

        internal Layer(IMapinfoWrapper mapinfoInstance, string layerName)
        {
            this.layername = layerName;
            this.wrapper = mapinfoInstance ?? ServiceLocator.GetInstance<IMapinfoWrapper>();
        }

        /// <summary>
        /// Creates a new <see cref="T:Layer"/> using the layer number.
        /// </summary>
        /// <param name="layerNumber">The number of the layer.</param>
        /// <returns>An instance of <see cref="T:Layer"/>.</returns>
        public static Layer GetLayerFromNumber(int layerNumber)
        {
            return new Layer(layerNumber);
        }

        /// <summary>
        /// Creates a new <see cref="T:Layer"/> using the layer name.
        /// </summary>
        /// <param name="layerName">The name of the layer.</param>
        /// <returns>An instance of <see cref="T:Layer"/>.</returns>
        public static Layer GetLayerFromName(string layerName)
        {
            return new Layer(layerName);
        }
    }
}
