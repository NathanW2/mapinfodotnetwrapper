using System;

namespace MapInfo.Wrapper.Exceptions
{
    [global::System.Serializable]
    public class MapinfoException : ApplicationException
    {
        public MapinfoException() { }
        public MapinfoException(string message) : base(message) { }
        public MapinfoException(string message, Exception inner) : base(message, inner) { }
        public MapinfoException(string message, Exception inner,int errorCode) : base(message,inner)
        {
            this.MapinfoErrorCode = errorCode;
        }

        public int MapinfoErrorCode { get; private set; }

        protected MapinfoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
