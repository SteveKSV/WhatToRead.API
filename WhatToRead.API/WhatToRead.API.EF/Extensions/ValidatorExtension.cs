using EFTopics.BBL.Dtos;
using EFWhatToRead_BBL.Helpers;
using FluentValidation;

namespace WhatToRead.API.EF.Extensions
{
    public static class ValidatorExtension
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<PostDto>, PostValidator>();
        }
    }
}
