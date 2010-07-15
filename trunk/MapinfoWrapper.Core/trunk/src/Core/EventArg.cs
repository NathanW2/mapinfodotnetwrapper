using System;

namespace MapInfo.Wrapper.Core
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            value = value;
        }

        private T value;

        public T Value
        {
            get { return value; }
        }
    }
}