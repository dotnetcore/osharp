// -----------------------------------------------------------------------
//  <copyright file="User.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 11:28</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;


namespace OSharp.Demo.Identity.Entities
{
    /// <summary>
    /// 实体类：用户信息
    /// </summary>
    public class User : EntityBase<int>, ICreatedTime
    {
        /// <summary>
        /// 初始化一个<see cref="User"/>类型的新实例
        /// </summary>
        public User()
        {
            CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <inheritdoc />
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 用户详细信息
        /// </summary>
        public UserDetail UserDetail { get; set; }
    }
}