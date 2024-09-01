using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.Authentication
{
    public interface IIdentityProvider
    {
        IIdentity Current { get; }
    }
}
