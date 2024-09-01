using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.Authentication
{
    public interface IIdentity
    {
        public Guid UserId { get; }
    }

    public static class IdentityExtentions
    {
        public static bool IsAuthenticated(this IIdentity identity)
        {
            return identity.UserId != Guid.Empty;
        }
    }
}