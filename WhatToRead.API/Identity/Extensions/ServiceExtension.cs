using Identity.Services.Interfaces;
using Identity.Services;

namespace Identity.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection service)
        {
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IAdminService, AdminService>();
        }
    }
}
