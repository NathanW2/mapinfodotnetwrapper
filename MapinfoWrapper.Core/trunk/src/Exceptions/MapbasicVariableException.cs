﻿using System;

namespace MapInfo.Wrapper.Exceptions
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
