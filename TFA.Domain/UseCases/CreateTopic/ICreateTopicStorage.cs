using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicStorage
    {
        Task<Topic> CreateTopic(Guid forumId, string title, Guid userId,
            CancellationToken cancellationToken = default);
    }
}
