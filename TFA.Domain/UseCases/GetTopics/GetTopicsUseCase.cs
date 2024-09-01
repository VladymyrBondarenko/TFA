using FluentValidation;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Domain.UseCases.GetTopics
{
    internal class GetTopicsUseCase : IGetTopicsUseCase
    {
        private readonly IValidator<GetTopicsQuery> _validator;
        private readonly IGetTopicsStorage _storage;
        private readonly IGetForumsStorage _forumsStorage;

        public GetTopicsUseCase(IValidator<GetTopicsQuery> validator,
            IGetTopicsStorage storage, IGetForumsStorage forumsStorage)
        {
            _validator = validator;
            _storage = storage;
            _forumsStorage = forumsStorage;
        }

        public async Task<(IEnumerable<Topic> resources, int totalCount)> Execute(GetTopicsQuery query, CancellationToken cancellationToken)
        {
            await _forumsStorage.ThrowIfForumNotFound(query.ForumId, cancellationToken);

            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            return await _storage.GetTopics(query.ForumId, query.Skip, query.Take, cancellationToken);
        }
    }
}