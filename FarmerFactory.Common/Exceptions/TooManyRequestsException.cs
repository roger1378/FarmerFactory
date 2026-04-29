using System;

namespace FarmerFactory.Common.Exceptions
{
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException(string message = "Too many requests.") : base(message)
        {
        }
    }
}
