using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid userId) :
            base($"User with id {userId} was not found")
        {
        }
    }
}
