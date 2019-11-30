using System;

namespace PapaLib.Event
{
    public class EventException : ApplicationException
    {
        public EventException() : base() {}
        public EventException(string message) : base(message) {}
    }
}
