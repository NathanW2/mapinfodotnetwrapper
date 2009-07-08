using System;

namespace MapinfoWrapper.Mapinfo
{
    /// <summary>
    /// Allows Mapinfo commands that are being run against Mapinfo to be logged.
    /// 
    /// <para>As this call just fowards commands to the underlying <see cref="IMapinfoWrapper"/> calls should be
    /// made against this object.</para>
    /// 
    /// <para>This class fowards the calls to the underlying Mapinfo wrapper object, before and after the logging actions have
    /// been preformed. Due to this there maybe some overhead and should really only be used to see what commands are getting excuted
    /// and removed at release.</para>
    /// <example>
    /// Example:
    /// <code>
    /// Action&lt;String&gt; doaction = (command) =&gt; Console.WriteLine("Run -&gt; {0}", command);
    /// Action&lt;String&gt; evalaction = (command) =&gt; Console.WriteLine("Eval -&gt; {0}", command);
    /// Action&lt;String&gt; returnaction = (command) =&gt; Console.WriteLine("   &lt;- {0}", command);
    /// IMapinfoWrapper mapinfo = new MapinfoLogger(COMMapinfo.CreateInstance(), doaction, evalaction, returnaction);        
    ///</code>
    ///</example>
    ///</summary>
    internal class MapinfoLogger : IMapinfoWrapper
    {
        private IMapinfoWrapper mapinfo;
        private Action<String> action;
        private Action<String> evalaction;
        private Action<String> returnevalaction;

        /// <summary>
        /// Creates a new instace of the <see cref="MapinfoLogger"/> object.
        /// </summary>
        /// <param name="mapinfoInstance">The instance of Mapinfo to log against.</param>
        /// <param name="doCommandLogAction">The action that will be invoked before the RunCommand is run.</param>
        /// <param name="evalCommandLogAction">The action that will be invoked before the Evaluate command is run.</param>
        /// <param name="evalReturnLogAction">The action that will be invoked after the Evaluate command in run, this action is passed the result
        /// of the Evaluate command.</param>
        public MapinfoLogger(IMapinfoWrapper mapinfoInstance, Action<String> doCommandLogAction, Action<String> evalCommandLogAction, Action<String> evalReturnLogAction)
        {
            this.mapinfo = mapinfoInstance;
            this.action = doCommandLogAction;
            this.evalaction = evalCommandLogAction;
            this.returnevalaction = evalReturnLogAction;
        }

        #region IMapinfoWrapper Members

        public void RunCommand(string commandString)
        {
            this.action(commandString);
            this.mapinfo.RunCommand(commandString);
        }

        public string Evaluate(string commandString)
        {
            this.evalaction(commandString);
            string value = this.mapinfo.Evaluate(commandString);
            this.returnevalaction(value);
            return value;
        }

        public object GetUnderlyingMapinfoInstance()
        {
            return this.mapinfo;
        }

        #endregion
    }
}
