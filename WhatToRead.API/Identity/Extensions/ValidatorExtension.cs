using FluentValidation;
using Identity.Data.Models;
using Identity.Validators;

namespace Identity.Extensions
{
    public static class ValidatorExtension
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<RegisterModel>, RegisterValidator>();
            services.AddTransient<IValidator<TokenRequestModel>, LoginValidator>();
        }
    }
}
