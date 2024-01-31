using AwesomeSocialMedia.Newsfeed.API.Consumers;
using AwesomeSocialMedia.Newsfeed.Application;
using AwesomeSocialMedia.Newsfeed.Core.Core.Repositories;
using AwesomeSocialMedia.Newsfeed.Infrastructure;
using AwesomeSocialMedia.Newsfeed.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<PostCreatedConsumer>();
builder.Services.AddHostedService<UserUpdatedConsumer>();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

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

app.MapControllers();

app.Run();

