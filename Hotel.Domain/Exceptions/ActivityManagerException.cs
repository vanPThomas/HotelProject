using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Exceptions
{
    public class ActivityManagerException : Exception
    {
        public ActivityManagerException(string? message)
            : base(message) { }

        public ActivityManagerException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
