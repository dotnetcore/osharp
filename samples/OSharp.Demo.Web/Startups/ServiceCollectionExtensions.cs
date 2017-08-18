using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Demo.Identity;
using OSharp.Dependency;
using OSharp.Entity;

namespace Microsoft.AspNetCore.Builder
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 将OSharp服务添加到<see cref="IServiceCollection"/>
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services)
        {
            services.AddAppServices();

            services.AddScoped<IUnitOfWork, DefaultDbContext>(provider =>
            {
                IEntityConfigurationTypeFinder finder = provider.GetService<IEntityConfigurationTypeFinder>();
                IConfiguration config = provider.GetService<IConfiguration>();
                DbContextOptionsBuilder builder = new DbContextOptionsBuilder<DefaultDbContext>();
                builder.UseSqlServer(config.GetConnectionString("DefaultDbContext"));
                return new DefaultDbContext(builder.Options, finder);
            });

            return services;
        }
    }
}
