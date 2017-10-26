// -----------------------------------------------------------------------
//  <copyright file="SecurityService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-10-24 23:53</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;
using OSharp.Infrastructure;


namespace OSharp.Demo.Security
{
    /// <summary>
    /// 业务实现：权限安全模块
    /// </summary>
    public partial class SecurityService : ISecurityContract
    {
        private readonly IRepository<Function, Guid> _functionRepository;

        /// <summary>
        /// 初始化一个<see cref="SecurityService"/>类型的新实例
        /// </summary>
        public SecurityService(IRepository<Function, Guid> functionRepository)
        {
            _functionRepository = functionRepository;
        }
    }
}