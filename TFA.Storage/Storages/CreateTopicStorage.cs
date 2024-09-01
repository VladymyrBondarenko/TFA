using AutoMapper;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage.Common;
using TFA.Storage.Models;

namespace TFA.Storage.Storages
{
    internal class CreateTopicStorage : ICreateTopicStorage
    {
        private readonly ForumDbContext _db;
        private readonly IGuidFactory _guidFactory;
        private readonly IMomentProvider _momentProvider;
        private readonly IMapper mapper;

        public CreateTopicStorage(ForumDbContext db,
            IGuidFactory guidFactory,
            IMomentProvider momentProvider,
            IMapper mapper)
        {
            _db = db;
            _guidFactory = guidFactory;
            _momentProvider = momentProvider;
            this.mapper = mapper;
        }

        public async Task<Domain.Models.Topic> CreateTopic(Guid forumId, string title, Guid userId,
            CancellationToken cancellationToken = default)
        {
            var id = _guidFactory.Create();

            await _db.Topics.AddAsync(new Topic
            {
                Id = id,
                Title = title,
                ForumId = forumId,
                UserId = userId,
                CreatedAt = _momentProvider.Now
            });
            await _db.SaveChangesAsync(cancellationToken);

            var addedTopic = await _db.Topics.FindAsync(id, cancellationToken);

            return mapper.Map<Domain.Models.Topic>(addedTopic);
        }
    }
}