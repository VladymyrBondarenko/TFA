using FluentAssertions;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.Tests.CreateTopic
{
    public class CreateTopicCommandValidatorShould
    {
        private readonly CreateTopicCommandValidator _sut;

        public CreateTopicCommandValidatorShould()
        {
            _sut = new CreateTopicCommandValidator();
        }

        [Fact]
        public void ReturnSuccess_WhenCommandIsValid()
        {
            var cmd = new CreateTopicCommand(new Guid("186b5565-2daa-4feb-936d-38f0abc7b923"), "Test");
            _sut.Validate(cmd).IsValid
                .Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetInvalidCreateTopicCommands))]
        public void ReturnFailure_WhenCommandIsInvalid(CreateTopicCommand cmd)
        {
            _sut.Validate(cmd).IsValid
                .Should().BeFalse();
        }

        public static IEnumerable<object[]> GetInvalidCreateTopicCommands()
        {
            var validCmd = new CreateTopicCommand(new Guid("186b5565-2daa-4feb-936d-38f0abc7b923"), "Test");

            yield return new object[] { validCmd with { ForumId = Guid.Empty } };
            yield return new object[] { validCmd with { Title = string.Empty } };
            yield return new object[] { validCmd with { Title = "     " } };
            yield return new object[] { validCmd with { Title = string.Join(',', Enumerable.Range(0, 100)) } };
        }
    }
}
