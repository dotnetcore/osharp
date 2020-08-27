// -----------------------------------------------------------------------
//  <copyright file="IHybridServiceScopeFactory.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-20 23:19</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Dependency
{
    /// <summary>
    /// <see cref="IServiceScope"/>工厂包装一下
    /// </summary>
    public interface IHybridServiceScopeFactory
    {
        /// <summary>
        /// 创建依赖注入服务的作用域，如果当前操作处于HttpRequest作用域中，直接使用HttpRequest的作用域，否则创建新的作用域
        /// </summary>
        /// <returns></returns>
        IServiceScope CreateScope();
    }
}