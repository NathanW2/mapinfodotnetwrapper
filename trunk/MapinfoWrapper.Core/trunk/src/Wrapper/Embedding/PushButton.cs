namespace MapinfoWrapper.Embedding
{
    using Core.Extensions;

    /// <summary>
    /// Represents a Mapinfo push button.  Can be added to <see cref="ButtonPad"/> buttons collection.
    /// </summary>
    public class PushButton : MIButton
    {
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