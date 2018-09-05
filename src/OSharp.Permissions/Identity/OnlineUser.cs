// -----------------------------------------------------------------------
//  <copyright file="OnlineUser.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-09 11:38</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace OSharp.Identity
{
    /// <summary>
    /// 在线用户信息
    /// </summary>
    public class OnlineUser
    {
        /// <summary>
        /// 初始化一个<see cref="OnlineUser"/>类型的新实例
        /// </summary>
        public OnlineUser()
        {
            Roles = new string[0];
            ExtendData = new Dictionary<string, object>();
        }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 用户Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 用户头像
        /// </summary>
        public string HeadImg { get; set; }

        /// <summary>
        /// 获取或设置 是否管理
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 获取或设置 用户角色
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 获取 扩展数据字典
        /// </summary>
        public IDictionary<string, object> ExtendData { get; }
    }
}