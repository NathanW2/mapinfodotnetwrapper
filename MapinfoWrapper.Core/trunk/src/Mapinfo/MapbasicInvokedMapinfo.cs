using MapInfo.Wrapper;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MapInfo.Wrapper.Exceptions;

namespace MapInfo.Wrapper.Mapinfo
{
    /// <summary>Holds an instance of MapInfo which has been created in the miadm.dll when calling a .NET assembly from MapBasic.</summary>
    public class MapBasicInvokedMapInfo : IMapInfoWrapper
    {
        private const string LastErrorCodePropertyName = "LastErrorCode";
        private const string LastErrorMessagePropertyName = "LastErrorMessage";
        private const string VisiblePropertyExceptionMessage = "The Visible property is not supported for MapBasic-invoked instances.";
        private readonly object mapInfoInstance;
        private readonly Type mapInfoType;

        /// <summary>Initializes a new instance of the MapBasicInvokedMapInfo class.</summary>
        /// <param name="mapinfoInstance">A System.Object representing a running instance of MapInfo.</param>
        /// <param name="mapInfoType">A System.Type representing the type of object for the current MapInfo instance.</param>
        private MapBasicInvokedMapInfo(object mapinfoInstance, Type mapInfoType)
        {
            this.mapInfoType = mapInfoType;
            this.mapInfoInstance = mapinfoInstance;
        }

        /// <summary>Gets or sets a value representing the last MapInfo error code.</summary>
        public int LastErrorCode
        {
            get
            {
                return this.RetrieveMapObjectProperty<int>(LastErrorCodePropertyName);
            }

            set
            {
                this.SetMapObjectProperty(LastErrorCodePropertyName, value);
            }
        }

        /// <summary>Gets or sets a value representing the last MapInfo error message.</summary>
        public string LastErrorMessage
        {
            get
            {
                return this.RetrieveMapObjectProperty<string>(LastErrorMessagePropertyName);
            }

            set
            {
                this.SetMapObjectProperty(LastErrorMessagePropertyName, value);
            }
        }

        /// <summary>Gets or sets a value indicating whether the MapInfo interface is visible. **Not supported for MapBasic-invoked sessions.</summary>
        public bool Visible
        {
            get
            {
                throw new NotSupportedException(VisiblePropertyExceptionMessage);
            }

            set
            {
                throw new NotSupportedException(VisiblePropertyExceptionMessage);
            }
        }

        /// <summary>Creates a MapInfo object based on a currently executing instance.</summary>
        /// <returns>A SouthernDowns.MapInfoExtensions.MapBasicInvokedMapInfo representing a MapInfo instance.</returns>
        public static MapBasicInvokedMapInfo CreateMapInfoFromInstance()
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(f => f.GetName().Name == "miadm").FirstOrDefault();
            if (assembly == null)
            {
                throw new MapinfoException("Unable to locate existing MapInfo instance.");
            }

            Type interopType = assembly.GetType("MapInfo.MiPro.Interop.InteropServices");
            PropertyInfo propertyInfo = interopType.GetProperty("MapInfoApplication", BindingFlags.Public | BindingFlags.Static);
            return new MapBasicInvokedMapInfo(propertyInfo.GetValue(null, null), propertyInfo.PropertyType);
        }

        /// <summary>Interprets a System.String as a MapBasic statement and executes the statement.</summary>
        /// <param name="command">A System.String representing a MapBasic statement.</param>
        /// <remarks>This method is synchronous.</remarks>
        /// <exception cref="MapInfoException">Thrown when an exception occurs in the MapInfo Professional instance.</exception> 
        public void Do(string command)
        {
            Debug.Print("Do: " + command.Replace("\n", "\nDo: "));
            this.InvokeMapInfoMethod("Do", new object[] { command });
        }

        private void InvokeMapInfoMethod(string methodName, object[] parameters)
        {
            MethodInfo method = this.mapInfoType.GetMethod(methodName);
            method.Invoke(this.mapInfoInstance, parameters);
        }

        /// <summary>Interprets a System.String as a MapBasic expression.</summary>
        /// <param name="expression">A System.String representing a MapBasic expression.</param>
        /// <returns>A System.String representing the result of the MapBasic expression.</returns>
        /// <remarks>
        /// If the expression has a Logical (Boolean) value, MapInfo Professional returns a one-character string, "T" or "F".
        /// This method is synchronous.
        /// </remarks>
        /// <exception cref="MapInfoException">Thrown when an exception occurs in the MapInfo Professional instance.</exception> 
        public string Eval(string expression)
        {
            Debug.Print("Eval: " + expression.Replace("\n", "\nEval: "));
            return this.InvokeMapInfoMethodAsString("Eval", new object[] { expression });
        }

        public object GetUnderlyingMapinfoInstance()
        {
            return this.mapInfoInstance;
        }

        public MapInfoCallback Callback
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>Executes a MapInfo Professional menu command.</summary>
        /// <remarks>
        /// This method activates a standard menu command or button; to activate a custom menu command or
        /// button, use the Do method to issue a "Run Menu Command ID [id]" statement.
        /// </remarks>
        /// <param name="id">A value representing a MapInfo Professional menu command ID.</param>
        /// <exception cref="MapInfoException">Thrown when an exception occurs in the MapInfo Professional instance.</exception> 
        public void RunMenuCommand(int id)
        {
            this.InvokeMapInfoMethod("RunMenuCommand", new object[] { id });
        }

        /// <summary>
        /// Registers the OLE Automation object as a "sink" for MapInfo Professional-generated notifications. Use this
        /// method when you register a callback from within the MapInfo Professional process (for example, from a DLL
        /// that is called via MapBasic). Using this method ensures that your callback object will work side-by-side with other
        /// applications that may be running within the MapInfo Professional process.
        /// </summary>
        /// <param name="callbackObject">A System.Object representing a Mapinfo Professional OLE callback object.</param>
        /// <exception cref="MapInfoException">Thrown when an exception occurs in the MapInfo Professional instance.</exception> 
        public void RegisterCallback(object callbackObject)
        {
            this.InvokeMapInfoMethod("RegisterCallback", new[] { callbackObject });
        }

        /// <summary>
        /// Unregisters an OLE Automation object that was registered via the RegisterCallback method. You must pass the
        /// same argument that was used in the call to RegisterCallback.
        /// </summary>
        /// <param name="callbackObject">A System.Object representing a Mapinfo Professional OLE callback object that was previously registered.</param>
        /// <exception cref="MapInfoException">Thrown when an exception occurs in the MapInfo Professional instance.</exception> 
        public void UnregisterCallback(object callbackObject)
        {
            this.InvokeMapInfoMethod("UnregisterCallback", new[] { callbackObject });
        }

        /// <summary>Retrieves the value of a property on the MapInfo COM object.</summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="propertyName">A System.String representing the name of the property to be queried.</param>
        /// <returns>The value of the property.</returns>
        private T RetrieveMapObjectProperty<T>(string propertyName)
        {
            return (T)this.mapInfoInstance.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, this.mapInfoInstance, null);
        }

        /// <summary>Sets the value of a property on the MapInfo COM object.</summary>
        /// <param name="propertyName">A System.String representing the name of the property to be set.</param>
        /// <param name="value">A System.Object representing the new value for the property.</param>
        private void SetMapObjectProperty(string propertyName, object value)
        {
            this.mapInfoInstance.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this.mapInfoInstance, new[] { value });
        }

        /// <summary>Invokes a method on the MapInfo COM object.</summary>
        /// <typeparam name="T">The type of object to be returned from the method.</typeparam>
        /// <param name="methodName">A System.String representing the name of the method to be invoked.</param>
        /// <param name="parameters">An array of objects representing the method parameters.</param>
        /// <returns>The return value for the method.</returns>
        private string InvokeMapInfoMethodAsString(string methodName, object[] parameters)
        {
            MethodInfo method = this.mapInfoType.GetMethod(methodName);
            return (string)method.Invoke(this.mapInfoInstance, parameters);
        }
    }
}
