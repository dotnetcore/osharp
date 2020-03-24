// -----------------------------------------------------------------------
//  <copyright file="OsharpBuilderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-02 14:27</last-date>
// -----------------------------------------------------------------------

using OSharp.Core.Packs;
using OSharp.Dependency;
using OSharp.EventBuses;


namespace OSharp.Core.Builders
{
    /// <summary>
    /// IOSharpBuilder扩展方法
    /// </summary>
    public static class OsharpBuilderExtensions
    {
        /// <summary>
        /// 添加核心模块
        /// </summary>
        internal static IOsharpBuilder AddCorePack(this IOsharpBuilder builder)
        {
            builder.AddPack<OsharpCorePack>()
                .AddPack<DependencyPack>()
                .AddPack<EventBusPack>();

            return builder;
        }
    }
}