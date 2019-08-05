using System;
using System.Collections.Generic;
using System.Text;

namespace Core3.Application.Exceptions
{
    public class UnauthrizedException : Exception
    {
        public UnauthrizedException() { }

        public UnauthrizedException(string message)
            : base(message) { }
    }
}
