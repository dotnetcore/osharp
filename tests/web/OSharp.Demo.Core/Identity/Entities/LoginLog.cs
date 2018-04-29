// -----------------------------------------------------------------------
//  <copyright file="LoginLog.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-30 0:01</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Data;
using OSharp.Entity;


namespace OSharp.Demo.Identity.Entities
{
    /// <summary>
    /// 实体类：用户登录日志
    /// </summary>
    public class LoginLog : EntityBase<Guid>, ICreatedTime
    {
        /// <summary>
        /// 初始化一个<see cref="LoginLog"/>类型的新实例
        /// </summary>
        public LoginLog()
        {
            Id = CombGuid.NewGuid();
        }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 登录IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 获取或设置 用户代理头
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 退出时间
        /// </summary>
        public DateTime? LogoutTime { get; set; }
    }
}