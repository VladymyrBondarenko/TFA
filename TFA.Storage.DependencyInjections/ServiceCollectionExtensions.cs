using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;
using TFA.Storage.Common;
using TFA.Storage.Storages;

namespace TFA.Storage.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumStorage(this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContextPool<ForumDbContext>(opt =>
            {
                opt.UseNpgsql(connectionString);
            });

            services.AddScoped<ICreateTopicStorage, CreateTopicStorage>()
                .AddScoped<IGetForumsStorage, GetForumsStorage>()
                .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
                .AddScoped<ICreateForumStorage, CreateForumStorage>();

            services.AddScoped<IGuidFactory, GuidFactory>()
                .AddScoped<IMomentProvider, MomentProvider>();

            services.AddMemoryCache();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(ForumDbContext)));

            return services;
        }
    }
}
