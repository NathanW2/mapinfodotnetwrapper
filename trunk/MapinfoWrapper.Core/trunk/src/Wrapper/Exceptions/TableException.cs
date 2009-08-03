namespace MapinfoWrapper.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TableException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TableException()
        {
        }

        public TableException(string message)
            : base(message)
        {
        }

        public TableException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected TableException(
            SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
