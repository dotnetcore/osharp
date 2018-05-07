// -----------------------------------------------------------------------
//  <copyright file="RegisterEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-08 4:06</last-date>
// -----------------------------------------------------------------------

using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.EventBuses;


namespace OSharp.Demo.Identity.Events
{
    public class RegisterEventData : EventDataBase
    {
        /// <summary>
        /// 获取或设置 注册信息
        /// </summary>
        public RegisterDto RegisterDto { get; set; }

        /// <summary>
        /// 获取或设置 注册用户
        /// </summary>
        public User User { get; set; }
    }
}