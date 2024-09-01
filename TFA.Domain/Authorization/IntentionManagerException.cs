using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.Authorization
{
    public class IntentionManagerException : Exception
    {
        public IntentionManagerException() : base("Action is not allowed")
        {
            
        }
    }
}
