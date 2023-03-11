using EFTopics.DAL.Data;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using EFWhatToRead_DAL.Repositories;
using EFWhatToRead_DAL.Repositories.Interfaces.Repositories;
using EFWhatToRead_DAL.Repositories.Interfaces;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_BBL.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connection for EF database + DbContext
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// DI Configurations - Business Layer
builder.Services.AddScoped<ITopicManager, TopicManager>();
builder.Services.AddScoped<IPostManager, PostManager>();

// DI Configurations - Data Access Layer
builder.Services.AddScoped<ITopicsRepository, TopicsRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
