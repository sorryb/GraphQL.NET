using GraphQL.Server;
using GraphQLDotNetCore.Repository;
using GraphQL.SystemTextJson;
using GraphQLWebApi.Contracts;
using GraphQLWebApi.Entities;
using GraphQLWebApi.GraphQL.GraphQLSchema;
using Microsoft.EntityFrameworkCore;
using GraphQL.Types;
using System;
using GraphQL.Server.Ui.Playground;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationContext"));
});

builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<AppSchema>();

builder.Services.AddGraphQL()
    .AddSystemTextJson()
    .AddGraphTypes(typeof(AppSchema), ServiceLifetime.Scoped)
    .AddDataLoader();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseGraphQL<AppSchema>();
app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());

app.MapControllers();

app.Run();
