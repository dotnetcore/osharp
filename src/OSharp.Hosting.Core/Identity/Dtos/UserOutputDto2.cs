// -----------------------------------------------------------------------
//  <copyright file="UserOutputDto2.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;

using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Hosting.Identity.Dtos
{
    /// <summary>
    /// 简单用户输出DTO
    /// </summary>
    [MapFrom(typeof(User))]
    public class UserOutputDto2 : IOutputDto
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 用户邮箱
        /// </summary>
        public string Email { get; set; }
    }
}