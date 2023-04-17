using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using System.Data;
using WhatToRead.API;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "WhatToRead_ADO API",
                Description = "API for work with booking search",
                TermsOfService = new Uri("https://go.microsoft.com/fwlink/?LinkID=206977"),
                Contact = new OpenApiContact
                {
                    Name = "Stepan Klem",
                    Email = "klem.stepan@chnu.edu.ua",
                }
            });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        
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