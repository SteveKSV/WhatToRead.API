using EFTopics.BBL.Data;
using Microsoft.EntityFrameworkCore;
using EFWhatToRead_DAL.Repositories;
using EFWhatToRead_DAL.Repositories.Interfaces.Repositories;
using EFWhatToRead_DAL.Repositories.Interfaces;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_BBL.Managers;
using EFTopics.BBL.Dtos;
using EFWhatToRead_BBL.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using EFWhatToRead_BBL.Models;
using System.Reflection;
using WhatToRead.API.EF.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Swagger
var contact = new OpenApiContact()
{
    Name = "Stepan Klem",
    Email = "klem.stepan@chnu.edu.ua",
};

var info = new OpenApiInfo()
{
    Version = "v1",
    Title = "Web API for Post&Topic blog with JWT Token based security",
    Description = "Implementing JWT Token based security, Pagination, Filtering, Searching, EF Core and more",
    Contact = contact,
};

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", info);
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });
    
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    o.IncludeXmlComments(xmlPath);

    // Security
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//Connection for EF database + DbContext
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationContext>()
.AddDefaultTokenProviders();

// ������ �������������� JWT Bearer
    
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// items that knows only server

var secretKey = builder.Configuration.GetSection("JwtSettings:SecretKey").Value;
var issuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value;
var audience = builder.Configuration.GetSection("JwtSettings:Audience").Value;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = signingKey,
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("OnlyAdmin", policyBuilder => policyBuilder.RequireClaim("RoleUser", "Admin"));
    option.AddPolicy("OnlyUser", policyBuilder => policyBuilder.RequireClaim("RoleUser", "User"));
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// DI Configurations - Validators
builder.Services.AddValidators();

// DI Configurations - Business Layer
builder.Services.AddManagers();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
