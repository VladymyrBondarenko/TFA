using Microsoft.EntityFrameworkCore;
using TFA.Server.Helpers;
using TFA.Server.Middlewares;
using TFA.Domain.DependencyInjection;
using TFA.Storage.DependencyInjection;
using AutoMapper;
using System.Reflection;
using TFA.Server.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiLogging(builder.Configuration, builder.Environment);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("ForumDbConnection"));

builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

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