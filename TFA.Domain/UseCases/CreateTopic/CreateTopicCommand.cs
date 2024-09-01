
namespace TFA.Domain.UseCases.CreateTopic
{
    public record class CreateTopicCommand(Guid ForumId, string Title)
    {
    }
}
