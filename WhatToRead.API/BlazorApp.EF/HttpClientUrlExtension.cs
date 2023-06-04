using BlazorApp.EF.Services;

namespace BlazorApp.EF
{
    public static class HttpClientUrlExtension
    {
        public static void AddHttpClientServices(this IServiceCollection service, WebApplicationBuilder builder)
        {
            service.AddHttpClient<TopicService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiEF"));
            });
            service.AddHttpClient<PostService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiEF"));
            });
            service.AddHttpClient<BookService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiADO"));
            });
            service.AddHttpClient<UserService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiIdentity"));
            });
        }

    }
}
