using FluentAssertions;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics
{
    public class GetTopicsQueryValidatorShould
    {
        private GetTopicsQueryValidator _sut;

        public GetTopicsQueryValidatorShould()
        {
            _sut = new GetTopicsQueryValidator();
        }

        [Fact]
        public void ReturnSuccess_WhenQueryIsValid()
        {
            var query = new GetTopicsQuery(
                new Guid("1ddeb3f4-6f0a-4db1-8876-52df2a328ebe"),
                10, 5);

            _sut.Validate(query).IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> GetInvalidQueries()
        {
            var validQuery = new GetTopicsQuery(
                new Guid("186b5565-2daa-4feb-936d-38f0abc7b923"), 
                10, 5);

            yield return new object[] { validQuery with { ForumId = Guid.Empty } };
            yield return new object[] { validQuery with { Skip = -5 } };
            yield return new object[] { validQuery with { Take = -5 } };
        }

        [Theory]
        [MemberData(nameof(GetInvalidQueries))]
        public void ReturnFailure_WhenQueryIsInvalid(GetTopicsQuery query)
        {
            _sut.Validate(query).IsValid.Should().BeFalse();
        }
    }
}
