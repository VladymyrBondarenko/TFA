using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.Authorization
{
    public interface IIntentionManager
    {
        bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;

        bool IsAllowed<TIntention, TObject>(TIntention intention, TObject intentionObject) where TIntention : struct;
    }

    public static class IntentionManagerExtentions
    {
        public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager,
            TIntention intention) where TIntention : struct
        {
            if (!intentionManager.IsAllowed(intention))
            {
                throw new IntentionManagerException();
            }
        }
    }
}
