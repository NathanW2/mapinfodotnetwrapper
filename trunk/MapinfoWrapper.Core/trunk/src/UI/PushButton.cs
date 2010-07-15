using MapInfo.Wrapper.Core.Extensions;

namespace MapInfo.Wrapper.UI
{
    /// <summary>
    /// Represents a Mapinfo push button.  Can be added to <see cref="ButtonPad"/> buttons collection, which can then be creating in 
    /// Mapinfo.
    /// </summary>
    public class PushButton : MIButton
    {
        public PushButton(int ID)
        {
            this.ID = ID;
        }

        /// <summary>
        /// Returns the formated Mapbasic string for the current button.
        /// </summary>
        /// <returns></returns>
        public override string ToButtonString()
        {
            return @"PushButton 
                        {0} 
                        Enable".FormatWith(this.IDFormated);
        }
    }
}