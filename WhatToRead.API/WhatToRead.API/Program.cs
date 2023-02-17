using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using System.Data;
using WhatToRead.API;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Connection/Transaction for ADO.NET/DAPPER database
        builder.Services.AddScoped((s) => new SqlConnection(builder.Configuration.GetConnectionString("DapperConnection")));
        builder.Services.AddScoped<IDbTransaction>(s =>
        {
            SqlConnection conn = s.GetRequiredService<SqlConnection>();
            conn.Open();
            return conn.BeginTransaction();
        });

        builder.Services.AddApplication();
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


    }
}