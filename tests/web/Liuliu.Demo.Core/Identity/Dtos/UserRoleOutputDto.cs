// -----------------------------------------------------------------------
//  <copyright file="UserRoleOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Liuliu.Demo.Identity.Entities;

using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Mapping;


namespace Liuliu.Demo.Identity.Dtos
{
    /// <summary>
    /// 输出DTO：用户角色信息
    /// </summary>
    [MapFrom(typeof(UserRole))]
    public class UserRoleOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                IIdentityContract contract = ServiceLocator.Instance.GetService<IIdentityContract>();
                return contract.Users.Where(m => m.Id == UserId).Select(m => m.UserName).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取或设置 角色名
        /// </summary>
        public string RoleName
        {
            get
            {
                IIdentityContract contract = ServiceLocator.Instance.GetService<IIdentityContract>();
                return contract.Roles.Where(m => m.Id == RoleId).Select(m => m.Name).FirstOrDefault();
            }
        }

        #region Implementation of IDataAuthEnabled

        /// <summary>
        /// 获取或设置 是否可更新的数据权限状态
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// 获取或设置 是否可删除的数据权限状态
        /// </summary>
        public bool Deletable { get; set; }

        #endregion
    }
}