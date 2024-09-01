using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateForum
{
    public interface ICreateForumUseCase
    {
        public Task<Forum> Execute(CreateForumCommand command, CancellationToken cancellationToken);
    }
}
