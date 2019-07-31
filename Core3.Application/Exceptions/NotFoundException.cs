using System;
using System.Collections.Generic;
using System.Text;

namespace Core3.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base(string.Format("Entity \"{0}\" ({1}) is missing", name, key))
        {

        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string name, object key)
            : base(string.Format("Entity \"{0}\" ({1}) experienced bad request", name, key))
        {

        }
    }
}
