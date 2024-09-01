using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics
{
    public class GetTopicsUseCaseShould
    {
        private readonly GetTopicsUseCase _sut;
        private readonly Mock<IGetTopicsStorage> _storage;
        private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> _getTopicsSetup;
        private readonly Mock<IGetForumsStorage> _getForumStorage;
        private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> _getForumsSetup;

        public GetTopicsUseCaseShould()
        {
            var validator = new Mock<IValidator<GetTopicsQuery>>();
            validator
                .Setup(x => x.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _storage = new Mock<IGetTopicsStorage>();
            _getTopicsSetup = _storage.Setup(x => x.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), 
                It.IsAny<int>(), It.IsAny<CancellationToken>()));

            _getForumStorage = new Mock<IGetForumsStorage>();
            _getForumsSetup = _getForumStorage.Setup(x =>
                x.GetForums(It.IsAny<CancellationToken>()));

            _sut = new GetTopicsUseCase(validator.Object, _storage.Object, _getForumStorage.Object);
        }

        [Fact]
        public async Task ReturnTopics_ExtractedFromStorage_WhenForumExists()
        {
            var forumId = new Guid("4dd2807d-7acc-44de-9f5a-8e1cceea114a");
            var skip = 10;
            var take = 5;

            var forums = new[] { new Forum { Id = forumId } };
            _getForumsSetup.ReturnsAsync(forums);

            var expectedResources = new[] { new Topic() };
            var expectedTotalCount = 6;
            _getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));

            var (actualResources, actualTotalCount) = await _sut.Execute(new GetTopicsQuery(forumId, skip, take), 
                CancellationToken.None);

            actualResources.Should().BeEquivalentTo(expectedResources);
            actualTotalCount.Should().Be(expectedTotalCount);

            _storage.Verify(x => x.GetTopics(forumId, skip, take, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ThrowError_WhenForumNotExists()
        {
            var forumId = new Guid("4dd2807d-7acc-44de-9f5a-8e1cceea114a");
            var skip = 10;
            var take = 5;

            var forums = new[] { new Forum { Id = new Guid("2ae96582-0f2d-429e-b4f8-7c9e55c3b60e") } };
            _getForumsSetup.ReturnsAsync(forums);

            await _sut.Invoking(x => x.Execute(new GetTopicsQuery(forumId, skip, take), CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>();
        }
    }
}