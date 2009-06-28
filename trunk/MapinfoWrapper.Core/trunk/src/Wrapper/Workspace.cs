using System;
using System.Collections.Generic;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper
{
    /// <summary>
    /// Contains methods for working with workspaces.
    /// </summary>
    public class Workspace
    {
        /// <summary>
        /// Opens a workspace in a instance of Mapinfo.
        /// </summary>
        /// <param name="workspacePath">The path to the workspace which needs to be opened.</param>
        /// <returns>A instance of a <see cref="Workspace"/> which can be used to get infomation about the opened workspace.</returns>
        public static Workspace OpenWorkspace(string workspacePath)
        {
            IMapinfoWrapper wrapper = ServiceLocator.GetInstance<IMapinfoWrapper>();
            wrapper.RunCommand("Run Application {0}".FormatWith(workspacePath.InQuotes()));
            return new Workspace();
        }
    }
}
