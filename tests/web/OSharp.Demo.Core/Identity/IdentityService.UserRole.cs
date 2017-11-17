// -----------------------------------------------------------------------
//  <copyright file="IdentityService.UserRole.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 14:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;


namespace OSharp.Demo.Identity
{
    public partial class IdentityService
    {
        /// <summary>
        /// 获取 用户角色信息查询数据集
        /// </summary>
        public IQueryable<UserRole> UserRoles
        {
            get { return _userRoleRepository.Query(); }
        }

        /// <summary>
        /// 检查用户角色信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的用户角色信息编号</param>
        /// <returns>用户角色信息是否存在</returns>
        public Task<bool> CheckUserRoleExists(Expression<Func<UserRole, bool>> predicate, Guid id = default(Guid))
        {
            return _userRoleRepository.CheckExistsAsync(predicate, id);
        }
        
    }
}