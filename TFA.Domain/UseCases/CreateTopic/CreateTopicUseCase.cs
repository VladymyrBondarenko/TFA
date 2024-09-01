using FluentValidation;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Domain.UseCases.CreateTopic
{
    internal class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly IValidator<CreateTopicCommand> _validator;
        private readonly IIntentionManager _intentionManager;
        private readonly ICreateTopicStorage _createTopicStorage;
        private readonly IGetForumsStorage _getForumsStorage;
        private readonly IIdentityProvider _identityProvider;

        public CreateTopicUseCase(
            IValidator<CreateTopicCommand> validator,
            IIntentionManager intentionManager,
            ICreateTopicStorage createTopicStorage,
            IGetForumsStorage getForumsStorage,
            IIdentityProvider identityProvider)
        {
            _validator = validator;
            _intentionManager = intentionManager;
            _createTopicStorage = createTopicStorage;
            _getForumsStorage = getForumsStorage;
            _identityProvider = identityProvider;
        }

        public async Task<Topic> Execute(CreateTopicCommand createTopicCommand,
            CancellationToken cancellationToken = default)
        {
            _intentionManager.ThrowIfForbidden(TopicIntention.Create);

            await _validator.ValidateAndThrowAsync(createTopicCommand);

            await _getForumsStorage.ThrowIfForumNotFound(createTopicCommand.ForumId, cancellationToken);

            return await _createTopicStorage.CreateTopic(createTopicCommand.ForumId, 
                createTopicCommand.Title, _identityProvider.Current.UserId, 
                cancellationToken);
        }
    }
}
