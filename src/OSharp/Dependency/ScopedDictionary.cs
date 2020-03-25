// -----------------------------------------------------------------------
//  <copyright file="ScopedDictionary.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-01 21:17</last-date>
// -----------------------------------------------------------------------

using OSharp.Audits;

using System;
using System.Collections.Concurrent;
using System.Security.Claims;

using OSharp.Authorization.Functions;


namespace OSharp.Dependency
{
    /// <summary>
    /// 基于Scoped生命周期的数据字典，可用于在Scoped环境中传递数据
    /// </summary>
    public sealed class ScopedDictionary : ConcurrentDictionary<string, object>, IDisposable
    {
        /// <summary>
        /// 获取或设置 当前执行的功能
        /// </summary>
        public IFunction Function { get; set; }

        /// <summary>
        /// 获取或设置 对于当前功能有效的角色集合，用于数据权限判断
        /// </summary>
        public string[] DataAuthValidRoleNames { get; set; } = new string[0];

        /// <summary>
        /// 获取或设置 当前操作审计
        /// </summary>
        public AuditOperationEntry AuditOperation { get; set; }

        /// <summary>
        /// 获取或设置 当前用户
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

        /// <summary>释放资源.</summary>
        public void Dispose()
        {
            Function = null;
            AuditOperation = null;
            Identity = null;
            this.Clear();
        }
    }
}