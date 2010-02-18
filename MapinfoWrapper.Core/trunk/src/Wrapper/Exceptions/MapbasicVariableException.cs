using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.Exceptions
{
    public class MapbasicVariableException : MapbasicException
    {
        public MapbasicVariableException(string message, Exception inner)
            : base(message,inner)
        {

        }
    }
}
