using BlazorApp.EF.Helpers;
using BlazorApp.EF.Models;
using BlazorApp.EF.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<PostValidator>();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl")) });

builder.Services.AddHttpClient<TopicService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiEF"));
});
builder.Services.AddHttpClient<PostService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiEF"));
});
builder.Services.AddHttpClient<BookService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiADO"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
