using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Exceptions
{
    public class OrganisationManagerException : Exception
    {
        public OrganisationManagerException(string? message)
            : base(message) { }

        public OrganisationManagerException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
