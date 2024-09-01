using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Filters;
using TFA.Server.Helpers;
using TFA.Server.Middlewares;
using TFA.Domain.DependencyInjection;
using TFA.Storage.DependencyInjection;
using TFA.Server.MappingProfiles;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(x => x.AddSerilog(new LoggerConfiguration()
    // level
    .MinimumLevel.Debug()
    // properties
    .Enrich.WithProperty("Application", "TFA")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    // for open search
    .WriteTo.Logger(lc => 
        lc.Filter.ByExcluding(Matching.FromSource("Microsoft"))
        .WriteTo.OpenSearch(builder.Configuration.GetConnectionString("Logs"), "forum-logs-{0:yyyy.MM.dd}"))
    // for console
    .WriteTo.Logger(lc => 
        lc.WriteTo.Console())
    .CreateLogger()
    ));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("ForumDbConnection"));

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<DomainToResponseProfile>();
});

var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed data
    app.PrepareDataPopulation();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();

public partial class Program
{
}