// -----------------------------------------------------------------------
//  <copyright file="LoginDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using OSharp.Identity;


namespace OSharp.Hosting.Identity.Dtos
{
    /// <summary>
    /// 登录信息DTO
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 获取或设置 登录账号，可以是用户名，Email，手机号等
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 获取或设置 记住登录
        /// </summary>
        public bool Remember { get; set; }

        /// <summary>
        /// 获取或设置 客户端类型
        /// </summary>
        public RequestClientType ClientType { get; set; }

        /// <summary>
        /// 获取或设置 是否Token登录
        /// </summary>
        public bool IsToken { get; set; }

        /// <summary>
        /// 获取或设置 IP地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 获取或设置 客户端代理头
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 获取或设置 回调地址
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}