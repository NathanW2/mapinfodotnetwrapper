using System;
using MapInfo.Wrapper.Core.Exceptions;

namespace Mapinfo.Wrapper.Exceptions
{
    public class MapbasicVariableException : MapbasicException
    {
        public MapbasicVariableException(string message)
            : base(message)
        {
        }

        public MapbasicVariableException(string message, Exception inner)
            : base(message,inner)
        {

        }
    }
}
