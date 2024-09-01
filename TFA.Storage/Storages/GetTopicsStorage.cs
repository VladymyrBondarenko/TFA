using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Storage.Storages
{
    internal class GetTopicsStorage : IGetTopicsStorage
    {
        private readonly ForumDbContext _db;
        private readonly IMapper mapper;

        public GetTopicsStorage(ForumDbContext db,
            IMapper mapper)
        {
            _db = db;
            this.mapper = mapper;
        }

        public async Task<(IEnumerable<Topic> resources, int totalCount)> GetTopics(Guid forumId, int skip, int take, 
            CancellationToken cancellationToken)
        {
            var query = _db.Topics
                .Where(x => x.ForumId == forumId);

            var totalCount = await query.CountAsync();
            var resources = await query
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();

            return (resources.Select(mapper.Map<Topic>), totalCount);
        }
    }
}
