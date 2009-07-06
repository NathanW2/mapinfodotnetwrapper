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
        private readonly MapinfoSession wrapper;

        public Layer(MapinfoSession mapinfoInstance, int layerNumber)
        {
            this.layernumber = layerNumber;
            this.wrapper = mapinfoInstance;
        }

        public Layer(MapinfoSession mapinfoInstance, string layerName)
        {
            this.layername = layerName;
            this.wrapper = mapinfoInstance;
        }
    }
}
