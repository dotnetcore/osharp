using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Serialization;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc.Conventions;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Core.Packs;
using OSharp.Dependency;


namespace Liuliu.Demo.Web.Startups
{
    public class AspNetCoreMvcPack : OsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
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
            services.AddMvc(options =>
            {
                options.Conventions.Add(new DashedRoutingConvention());
                options.Filters.Add(new FunctionAuthorizationFilter()); //全局功能权限过滤器
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDistributedMemoryCache();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseMvcWithAreaRoute();
            ILogger logger = ServiceLocator.Instance.GetLogger(GetType());
            logger.LogInformation("AspNetCoreMvcPack 模块初始化完毕");
        }
    }
}
