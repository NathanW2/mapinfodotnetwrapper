using Mapinfo.Wrapper.Core.Extensions;
using Mapinfo.Wrapper.Mapinfo;
using Mapinfo.Wrapper.MapOperations;

namespace Mapinfo.Wrapper.LayerOperations
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
        private readonly MapWindow map;

        public Layer(MapinfoSession mapinfoInstance, int layerNumber, MapWindow mapwindow)
        {
            this.layernumber = layerNumber;
            this.wrapper = mapinfoInstance;
            this.map = mapwindow;
        }

        public string Name
        {
            get
            {
                return this.wrapper.Eval("LayerInfo({0},{1},{2})".FormatWith(this.map.ID, this.layernumber, 1));
            }
        }
    }
}
