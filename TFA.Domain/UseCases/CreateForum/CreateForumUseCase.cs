using FluentValidation;
using TFA.Domain.Authorization;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateForum
{
    internal class CreateForumUseCase : ICreateForumUseCase
    {
        private readonly ICreateForumStorage storage;
        private readonly IValidator<CreateForumCommand> validator;
        private readonly IIntentionManager intentionManager;

        public CreateForumUseCase(ICreateForumStorage storage, 
            IValidator<CreateForumCommand> validator,
            IIntentionManager intentionManager)
        {
            this.storage = storage;
            this.validator = validator;
            this.intentionManager = intentionManager;
        }

        public async Task<Forum> Execute(CreateForumCommand command, CancellationToken cancellationToken)
        {
            intentionManager.ThrowIfForbidden(ForumIntention.Create);

            await validator.ValidateAndThrowAsync(command, cancellationToken);

            return await storage.CreateForum(command.Title, cancellationToken);
        }
    }
}
