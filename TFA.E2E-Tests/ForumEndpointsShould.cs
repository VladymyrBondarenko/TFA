using FluentAssertions;
using System.Net.Http.Json;
using TFA.Server.Models;

namespace TFA.E2E
{
    public class ForumEndpointsShould : IClassFixture<ForumServerApplicationFactory>
    {
        private readonly ForumServerApplicationFactory factory;

        public ForumEndpointsShould(ForumServerApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task ReturnListOfForums()
        {
            using var httpClient = factory.CreateClient();
            using var response = await httpClient.GetAsync("api/forums");

            response.Invoking(x => x.EnsureSuccessStatusCode()).Should().NotThrow();

            var result = await response.Content.ReadAsStringAsync();
            result.Should().Be("[]");
        }

        [Fact]
        public async Task CreateNewForum()
        {
            using var httpClient = factory.CreateClient();

            var title = "Test Forum";

            using var getInitialResponse = await httpClient.GetAsync("api/forums");
            var initialForums = await getInitialResponse.Content.ReadFromJsonAsync<Forum[]>();
            initialForums
                .Should().NotBeNull().And
                .Subject.As<Forum[]>()
                .Should().BeEmpty();

            using var postResponse = await httpClient.PostAsync("api/forums",
                JsonContent.Create(new { Title = title }));

            postResponse.Invoking(x => x.EnsureSuccessStatusCode()).Should().NotThrow();
            var forum = await postResponse.Content.ReadFromJsonAsync<Forum>();
            forum
                .Should().NotBeNull().And
                .Subject.As<Forum>().Title
                .Should().Be(title);
            forum?.Id.Should().NotBeEmpty();

            using var getResponse = await httpClient.GetAsync("api/forums");
            var forums = await getResponse.Content.ReadFromJsonAsync<Forum[]>();
            forums
                .Should().NotBeNull().And
                .Subject.As<Forum[]>()
                .Should().Contain(x => x.Title == title);
        }
    }
}