// -----------------------------------------------------------------------
//  <copyright file="SystemsService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-02-28 23:55</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Entity;
using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.Systems
{
    /// <summary>
    /// 业务实现：系统模块
    /// </summary>
    public partial class SystemsService : ISystemsContract
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个<see cref="SystemsService"/>类型的新实例
        /// </summary>
        public SystemsService(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected IRepository<Menu, int> MenuInfoRepository => _provider.GetService<IRepository<Menu, int>>();
    }
}