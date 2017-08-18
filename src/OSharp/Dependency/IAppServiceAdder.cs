// -----------------------------------------------------------------------
//  <copyright file="IAppServiceAdder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 22:59</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Dependency
{
    /// <summary>
    /// 定义应用程序服务添加者，检索程序集，查找实现了<see cref="ITransientDependency"/>，<see cref="IScopeDependency"/>，<see cref="ISingletonDependency"/> 接口的所有服务，分别按生命周期类型进行添加
    /// </summary>
    public interface IAppServiceAdder
    {
        /// <summary>
        /// 添加应用程序服务到<see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IServiceCollection AddServices(IServiceCollection services);
    }
}