using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Extensions;
using Wrapper.Commands;
using System.Text.RegularExpressions;

namespace Wrapper.TableOperations
{
    /// <summary>
    /// Contains methods for constructing a the Open Table mapinfo command string.
    /// </summary>
    public class OpenTableCommandBuilder : CommandStringBuilder
    {
        string template = @"Open Table {FileName} {TableName}
                           {Hide} {ReadOnly} {Interactive} {Password}
                           {NoIndex} {View Automatic} {DenyWrite} {Grid}";


        /// <summary>
        /// Gets the open table template which is used to build the open table command string.
        /// </summary>
        /// <remarks>At the moment right after construction this string will contain the full template including the tokens,
        /// if any attributes have been set in the instance of <see cref="T:OpenTableCommandBuilder"/> it will contain the template
        /// string,the values of any replaced tokens, also any remaining tokens.
        /// </remarks>
        /// <example>
        /// <code>
        /// OpenTableCommandBuilder commmandbuilder = new OpenTableCommandBuilder(@"C:\Temp\Test.Tab");
        /// Console.WriteLine(commandbuilder.BuildCommandString());
        /// 
        /// //Returns "Open Table {FileName} {TableName}"
        /// //Then doing:
        /// commandbuilder.HasName("Test");
        /// Console.WriteLine(commandbuilder.BuildCommandString());
        /// 
        /// //Returns "Open Table "C:\Temp\Test" As Test"
        /// </code>
        /// </example>
        public override string Template
        {
            get
            {
                return template;
            }
            protected set
            {
                template = value;
            }
        }

        /// <summary>
        /// Creates a new instance of the open table command builder.
        /// </summary>
        /// <param name="tablePath">The path or name of the table to use in the open table command.</param>
        public OpenTableCommandBuilder(string tablePath)
        {
            SetTablePath(tablePath);
        }

        /// <summary>
        /// Sets the table path for the open table command string.
        /// </summary>
        /// <param name="tablePath">The table path/name to use.</param>
        public OpenTableCommandBuilder SetTablePath(string tablePath)
        {
            this.Template = this.ReplaceTokenWithValue("{FileName}", tablePath.InQuotes());
            return this;
        }

        /// <summary>
        /// Sets the name of the table to a specified alias.
        /// </summary>
        /// <param name="tableName">The alias by which the table should be indentifed.</param>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder HasName(string tableName)
        {
            string tablenamecommand = "as {0}".FormatWith(tableName);
            this.Template = this.ReplaceTokenWithValue("{TableName}", tablenamecommand);
            return this;
        }


        /// <summary>
        /// Sets the flag to open the table as hidden from the user.
        /// </summary>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder IsHidden()
        {
            this.Template = this.ReplaceTokenWithValue("{Hide}", "Hide");
            return this;
        }

        /// <summary>
        /// Sets the flag to open the table as read only, so that no write changes maybe made.
        /// </summary>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder IsReadOnly()
        {
            this.Template = this.ReplaceTokenWithValue("{ReadOnly}", "ReadOnly");
            return this;
        }


        /// <summary>
        /// Sets the flag to open the table as interactive, so that if the table can
        /// not be found the user will be asked to locate.
        /// </summary>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder OpenAsInteractive()
        {
            this.Template = this.ReplaceTokenWithValue("{Interactive}", "Interactive");
            return this;
        }

        /// <summary>
        /// Sets the password flag and value to open the table with.  Only needs to be used when using MS Access based tables.
        /// </summary>
        /// <param name="password">The password to use with the table.</param>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder Password(string password)
        {
            string passwordcommand = "Password {0}".FormatWith(password.InQuotes());
            this.Template = this.ReplaceTokenWithValue("{Password}", passwordcommand);
            return this;
        }

        /// <summary>
        /// Sets a flag to not create a index for MS Access tables when opened.
        /// </summary>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder CreateNoIndex()
        {
            this.Template = this.ReplaceTokenWithValue("{NoIndex}", "NoIndex");
            return this;
        }

        /// <summary>
        /// Sets the flag to tell Mapinfo when it opens mapinfo to reuse already open
        /// map windows or open a new one if there arn't any open at the time.
        /// </summary>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder AutomaticView()
        {
            this.Template = this.ReplaceTokenWithValue("{View Automatic}", "View Automatic");
            return this;
        }

        /// <summary>
        /// Sets the flag so that Mapinfo will stop other users if they have the same table
        /// open from making edits.
        /// </summary>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder DenyWrite()
        {
            this.Template = this.ReplaceTokenWithValue("{DenyWrite}", "DenyWrite");
            return this;
        }

        /// <summary>
        /// Sets a flag with how mapinfo should handle opening grid files.
        /// </summary>
        /// <param name="gridHandleEnum"></param>
        /// <returns>The current instance of the OpenTableCommandBuilder.</returns>
        public OpenTableCommandBuilder GridHadler(GridHandleEnum gridHandleEnum)
        {
            string name = Enum.GetName(typeof(GridHandleEnum), gridHandleEnum);
            this.Template = this.ReplaceTokenWithValue("{Grid}", name);
            return this;
        }
    }
}
