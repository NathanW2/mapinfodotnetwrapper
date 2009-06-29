using System.Text.RegularExpressions;

namespace MapinfoWrapper.CommandBuilders
{
    public abstract class CommandStringBuilder : ICommandStringBuilder
    {
        /// <summary>
        /// Returns the template used to create the final string.
        /// </summary>
        /// <remarks>Templates should use { & } to show tokens that need to be replaced</remarks>
        /// <example>Open Table {TableName}
        /// <para>Where {TableName} is the token that will be replaced.</para></example>
        public abstract string Template { get; protected set; }
     
        #region ICommandStringBuilder Members

        /// <summary>
        /// Creates a correctly formatted Mapbasic command string from the Template property which can be used to send to MapInfo.
        /// </summary>
        /// <returns>A correctly formatted Mapbasic command string.</returns>
        public string BuildCommandString()
        {
            string returnstring;
            returnstring = Regex.Replace(this.Template, "{.+?}", "");
            returnstring = Regex.Replace(returnstring, @"\s+", " ").Trim();
            return returnstring;
        }

        #endregion

        protected string ReplaceTokenWithValue(string token, string replaceWith)
        {
            if (this.Template.Contains(token))
            {
                string returnstring = this.Template.Replace(token, replaceWith);
                return returnstring;
            }
            return this.Template;
        }
    }
}
