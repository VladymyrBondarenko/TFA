using Microsoft.EntityFrameworkCore;
using TFA.Storage;

namespace TFA.Server.Helpers
{
    public static class DataPreparationHelper
    {
        public static void PrepareDataPopulation(this WebApplication app)
        {
            using var seviceScope = app.Services.CreateScope();

            var dbContext = seviceScope.ServiceProvider.GetService<ForumDbContext>();
            if (dbContext != null)
            {
                SeedData(dbContext);
            }
        }

        private static void SeedData(ForumDbContext dbContext)
        {
            Console.WriteLine("Applying migration");

            dbContext.Database.Migrate();

            Console.WriteLine("Data already exist");
        }
    }
}
