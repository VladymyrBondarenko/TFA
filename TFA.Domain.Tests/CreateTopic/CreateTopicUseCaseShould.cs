using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.Models;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Storage;
using TFA.Storage.Common;

namespace TFA.Domain.Tests.CreateTopic
{
    public class CreateTopicUseCaseShould
    {
        //private ForumDbContext _db;
        //private readonly ISetup<IGuidFactory, Guid> _createTopicIdSetup;
        //private ISetup<IMomentProvider, DateTimeOffset> _createTopicCreatedAtSetup;
        private readonly Mock<ICreateTopicStorage> _storage;
        private readonly ISetup<ICreateTopicStorage, Task<Topic>> _createTopicSetup;
        private readonly Mock<IGetForumsStorage> _getForumStorage;
        private readonly ISetup<IIdentity, Guid> _getCurrentUserIdSetup;
        private ISetup<IIntentionManager, bool> _intentionIsAllowedSetup;
        private CreateTopicUseCase _sut;
        private ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> _getForumsSetup;

        public CreateTopicUseCaseShould()
        {
            //var dbContextOptionsBuilder = new DbContextOptionsBuilder<ForumDbContext>()
            //    .UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));
            //_db = new ForumDbContext(dbContextOptionsBuilder.Options);

            //var guidFactory = new Mock<IGuidFactory>();
            //_createTopicIdSetup = guidFactory.Setup(x => x.Create());

            //var momentProvider = new Mock<IMomentProvider>();
            //_createTopicCreatedAtSetup = momentProvider.Setup(x => x.Now);

            _storage = new Mock<ICreateTopicStorage>();
            _createTopicSetup = _storage.Setup(x => x.CreateTopic(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

            _getForumStorage = new Mock<IGetForumsStorage>();
            _getForumsSetup = _getForumStorage.Setup(x => 
                x.GetForums(It.IsAny<CancellationToken>()));

            var identity = new Mock<IIdentity>();
            var identityProvider = new Mock<IIdentityProvider>();
            identityProvider.Setup(x => x.Current).Returns(identity.Object);
            _getCurrentUserIdSetup = identity.Setup(x => x.UserId);

            var intentionManager = new Mock<IIntentionManager>();
            _intentionIsAllowedSetup = intentionManager.Setup(x => x.IsAllowed(It.IsAny<TopicIntention>()));

            var validator = new Mock<IValidator<CreateTopicCommand>>();
            validator.Setup(x => x.ValidateAsync(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _sut = new CreateTopicUseCase(validator.Object, intentionManager.Object, 
                _storage.Object, _getForumStorage.Object, identityProvider.Object);
        }

        [Fact]
        public async Task ThrowIntentionManagerException_WhenUserIsAnonymyus()
        {
            _intentionIsAllowedSetup.Returns(false);

            var forumId = new Guid("cce6b328-fc3b-4564-8e08-dcb702be2fc8");

            await _sut.Invoking(x => x.Execute(new CreateTopicCommand(forumId, "title")))
                .Should().ThrowAsync<IntentionManagerException>();
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
        {
            _intentionIsAllowedSetup.Returns(true);

            _getForumsSetup.ReturnsAsync(Array.Empty<Forum>());

            var forumId = new Guid("ddb54ba6-8964-44f7-b729-f2cabf69c4c8");

            await _sut.Invoking(x => x.Execute(new CreateTopicCommand(forumId, "title")))
                .Should().ThrowAsync<ForumNotFoundException>();
        }

        [Fact]
        public async Task ReturnCreatedTopic()
        {
            _intentionIsAllowedSetup.Returns(true);

            var forumId = new Guid("ddb54ba6-8964-44f7-b729-f2cabf69c4c8");
            var userId = new Guid("ad5ac88c-8af9-4cac-9ab0-a53b3c7e75fa");
            var title = "title";

            var forums = new[] { new Forum { Id = forumId, Title = title } };
            _getForumsSetup.ReturnsAsync(forums);
            _getCurrentUserIdSetup.Returns(userId);

            var expected = new Topic();
            _createTopicSetup.ReturnsAsync(expected);

            var actual = await _sut.Execute(new CreateTopicCommand(forumId, title));
            actual.Should().Be(expected);

            _storage.Verify(x => x.CreateTopic(forumId, title, userId, It.IsAny<CancellationToken>()));
        }
    }
}
