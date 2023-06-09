using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_BBL.Managers;

namespace WhatToRead.API.EF.Extensions
{
    public static class ManagerExtension
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<ITopicManager, TopicManager>();
            services.AddScoped<IPostManager, PostManager>();
            services.AddScoped<IAccountManager, AccountManager>();
        }
    }
}
