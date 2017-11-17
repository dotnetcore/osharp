// -----------------------------------------------------------------------
//  <copyright file="IdentityService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 14:48</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Demo.Identity.Entities;
using OSharp.Entity;


namespace OSharp.Demo.Identity
{
    /// <summary>
    /// 业务实现：身份认证模块
    /// </summary>
    public partial class IdentityService : IIdentityContract
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<UserRole, Guid> _userRoleRepository;

        /// <summary>
        /// 初始化一个<see cref="IdentityService"/>类型的新实例
        /// </summary>
        public IdentityService(IRepository<User, int> userRepository,
            IRepository<Role, int> roleRepository,
            IRepository<UserRole, Guid> userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
    }
}