// -----------------------------------------------------------------------
//  <copyright file="IEmailSender.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-11 0:30</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Net;

/// <summary>
/// 定义Email发送功能
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// 发送Email
    /// </summary>
    /// <param name="email">接收人Email</param>
    /// <param name="subject">Email标题</param>
    /// <param name="message">Email内容</param>
    /// <returns></returns>
    Task SendEmailAsync(string email, string subject, string message);
}