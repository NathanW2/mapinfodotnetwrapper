namespace MapInfo.Wrapper.CommandBuilders
{
    /// <summary>
    /// An interface for objects needing to build a command string to send to mapinfo.
    /// </summary>
    public interface ICommandStringBuilder
    {
        /// <summary>
        /// Retruns a concatinated command string build up in the correct order
        /// to be sent to mapinfo.
        /// </summary>
        /// <returns>Returns a correctly formated string to be executed in Mapinfo</returns>
        string BuildCommandString();
    }
}
