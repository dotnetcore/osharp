// -----------------------------------------------------------------------
//  <copyright file="RoleNode.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Identity.Entities;

using OSharp.Mapping;


namespace Liuliu.Demo.Identity.Dtos
{
    /// <summary>
    /// 角色节点
    /// </summary>
    [MapFrom(typeof(Role))]
    public class RoleNode
    {
        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 获取或设置 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}