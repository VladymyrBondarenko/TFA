using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using TFA.Storage;

namespace TFA.E2E
{
    public class ForumServerApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ForumDbConnection"] = _dbContainer.GetConnectionString()
                }).Build();
            builder.UseConfiguration(configuration);

            base.ConfigureWebHost(builder);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ForumDbContext>();
            dbContextOptionsBuilder.UseNpgsql(_dbContainer.GetConnectionString());

            var dbContext = new ForumDbContext(dbContextOptionsBuilder.Options);
            await dbContext.Database.MigrateAsync();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
    }
}
