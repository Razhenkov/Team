using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team.Data;
using Team.Data.Dao;
using Team.Services;
using Team.Services.Builders;

namespace Team.Web
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureData(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TeamContext>(opt => opt.UseSqlServer(connectionString));
            services.AddTransient<DbContext, TeamContext>();

            services.AddTransient<IUserDao, UserDao>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IUserServices, UserServices>();

            return services;
        }

        public static IServiceCollection ConfigureBuilder(this IServiceCollection services)
        {
            services.AddTransient<UserBuilder>();

            return services;
        }
    }
}
