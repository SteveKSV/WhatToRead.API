using BlazorApp.EF;
using BlazorApp.EF.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Adding validators
builder.Services.AddSingleton<PostValidator>();

// Adding Http Client Service
builder.Services.AddHttpClientServices(builder);
builder.Services.AddHttpContextAccessor();

// For Authorization and Authentication
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
//builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();


// Adding AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
