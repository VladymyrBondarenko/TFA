using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.Models;
using TFA.Domain.UseCases.CreateForum;
using TFA.Storage.Common;

namespace TFA.Storage.Storages
{
    internal class CreateForumStorage : ICreateForumStorage
    {
        private readonly ForumDbContext db;
        private readonly IGuidFactory guidFactory;
        private readonly IMomentProvider momentProvider;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper mapper;

        public CreateForumStorage(ForumDbContext db,
            IGuidFactory guidFactory,
            IMomentProvider momentProvider,
            IMemoryCache memoryCache,
            IMapper mapper)
        {
            this.db = db;
            this.guidFactory = guidFactory;
            this.momentProvider = momentProvider;
            this.memoryCache = memoryCache;
            this.mapper = mapper;
        }

        public async Task<Forum> CreateForum(string Title, CancellationToken cancellationToken)
        {
            var id = guidFactory.Create();
            await db.Forums.AddAsync(new Models.Forum
            {
                Id = id,
                Title = Title,
                CreatedAt = momentProvider.Now
            });
            await db.SaveChangesAsync();

            memoryCache.Remove(nameof(GetForumsStorage.GetForums));

            var createdForum = await db.Forums.FindAsync(id);

            return mapper.Map<Forum>(createdForum);
        }
    }
}
