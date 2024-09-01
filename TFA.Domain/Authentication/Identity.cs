using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.Authentication
{
    public class Identity : IIdentity
    {
        public Identity(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}