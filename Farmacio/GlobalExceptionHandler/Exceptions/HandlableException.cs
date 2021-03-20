using System;
using System.Net;

namespace GlobalExceptionHandler.Exceptions
{
    public abstract class HandlableException : Exception
    {
        public abstract HttpStatusCode Code { get; set; }

        public HandlableException()
        {
        }

        public HandlableException(string message) :
            base(message)
        {
        }
    }
}