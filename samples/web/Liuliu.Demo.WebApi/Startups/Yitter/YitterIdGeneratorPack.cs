using System.ComponentModel;
using OSharp.Hosting.Identity.Entities;
using OSharp.Hosting.MultiTenancy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.Authorization;
using OSharp.Authorization.Modules;
using OSharp.Caching;
using OSharp.Core.Packs;
using OSharp.Core.Systems;
using OSharp.Entity;
using OSharp.Identity;
using Yitter.IdGenerator;
using Liuliu.Demo.Web.Startups.Yitter;
using OSharp.Entity.KeyGenerate;
using System.Configuration;

namespace Liuliu.Demo.Web.Startups
{
    public class YitterIdGeneratorPack : OsharpPack
    {
        public YitterIdGeneratorPack()
        {
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var _configuration = serviceProvider.GetRequiredService<IConfiguration>();
                // 配置 IdGeneratorOptions
                services.Configure<IdGeneratorOptions>(_configuration.GetSection("IdGeneratorOptions"));
            }
                

            services.Replace<IKeyGenerator<long>, YitterSnowKeyGenerator>(ServiceLifetime.Singleton);
            return services;
        }
    }
}
