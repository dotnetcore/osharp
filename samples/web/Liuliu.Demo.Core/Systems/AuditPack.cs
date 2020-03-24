// -----------------------------------------------------------------------
//  <copyright file="SystemPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:33</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Audits;
using OSharp.Core.Packs;


namespace Liuliu.Demo.Systems
{
    /// <summary>
    /// 审计模块
    /// </summary>
    [Description("审计模块")]
    public class AuditPack : AuditPackBase
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IAuditStore, AuditDatabaseStore>();
            services.AddScoped<IAuditContract, AuditService>();

            return base.AddServices(services);
        }
    }
}