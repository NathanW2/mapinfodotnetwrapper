using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.LayerOperations
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
