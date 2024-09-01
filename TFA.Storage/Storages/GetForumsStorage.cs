using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage.Storages
{
    internal class GetForumsStorage : IGetForumsStorage
    {
        private readonly ForumDbContext _db;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper mapper;

        public GetForumsStorage(ForumDbContext dbContext, IMemoryCache memoryCache,
            IMapper mapper)
        {
            _db = dbContext;
            _memoryCache = memoryCache;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(nameof(GetForums), x =>
            {
                x.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

                return _db.Forums.ProjectTo<Forum>(
                    mapper.ConfigurationProvider).ToArrayAsync(cancellationToken);
            });
        }
    }
}
