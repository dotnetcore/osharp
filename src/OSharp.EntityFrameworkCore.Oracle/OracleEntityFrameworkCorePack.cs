using System;
using System.ComponentModel;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Packs;
using OSharp.Entity.KeyGenerate;


namespace OSharp.Entity.Oracle
{
    /// <summary>
    /// OracleEntityFrameworkCore模块
    /// </summary>
    [Description("OracleEntityFrameworkCore模块")]
    public class OracleEntityFrameworkCorePack : EntityFrameworkCorePackBase
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services = base.AddServices(services);
            services.AddSingleton<ISequentialGuidGenerator, OracleSequentialGuidGenerator>();
            services.AddScoped(typeof(ISqlExecutor<,>), typeof(OracleDapperSqlExecutor<,>));
            services.AddSingleton<IDbContextOptionsBuilderDriveHandler, OracleDbContextOptionsBuilderDriveHandler>();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            bool? hasOracle = provider.GetOSharpOptions()?.DbContexts?.Values.Any(m => m.DatabaseType == DatabaseType.Oracle);
            if (hasOracle == null || !hasOracle.Value)
            {
                return;
            }

            base.UsePack(provider);
        }
    }
}