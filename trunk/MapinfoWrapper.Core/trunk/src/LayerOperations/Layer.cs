using MapInfo.Wrapper.Mapinfo;

namespace MapInfo.Wrapper.LayerOperations
{
    /// <summary>
    /// A Layer object that gives you access to methods and properties for a single Mapinfo layer.
    /// 
    /// <para>This object isn't fully implemented yet and is only a stub for future improvments.</para>
    /// <para><b>This object may change name in future releases.</b></para>
    /// </summary>
    public class Layer
    {
        private readonly int layernumber;
        private readonly string layername;
        private readonly MapInfoSession wrapper;

        public Layer(MapInfoSession mapInfoInstance, int layerNumber)
        {
            this.layernumber = layerNumber;
            this.wrapper = mapInfoInstance;
        }

        public Layer(MapInfoSession mapInfoInstance, string layerName)
        {
            this.layername = layerName;
            this.wrapper = mapInfoInstance;
        }
    }
}
