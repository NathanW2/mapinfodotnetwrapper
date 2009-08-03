namespace MapinfoWrapper.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MapbasicException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MapbasicException()
        {}

        public MapbasicException(string message)
            : base(message)
        {}

        public MapbasicException(string message, Exception inner)
            : base(message, inner)
        {}

        protected MapbasicException(
            SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}
    }
}