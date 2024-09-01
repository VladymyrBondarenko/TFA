using TFA.Domain.Authentication;
using TFA.Domain.Authorization;

namespace TFA.Domain.UseCases.CreateTopic
{
    internal class TopicIntentionResolver : IIntentionResolver<TopicIntention>
    {
        public bool Resolve(IIdentity subject, TopicIntention intention)
        {
            return intention switch
            {
                TopicIntention.Create => subject.IsAuthenticated(),
                _ => false
            };
        }
    }
}
