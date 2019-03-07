// -----------------------------------------------------------------------
//  <copyright file="DefaultEmailSender.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-08 2:47</last-date>
// -----------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Net
{
    /// <summary>
    /// 默认邮件发送者
    /// </summary>
    [Dependency(ServiceLifetime.Singleton, TryAdd = true)]
    public class DefaultEmailSender : IEmailSender
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个<see cref="DefaultEmailSender"/>类型的新实例
        /// </summary>
        public DefaultEmailSender(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="email">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string body)
        {
            OsharpOptions options = _provider.GetOSharpOptions();
            MailSenderOptions mailSender = options.MailSender;
            if (mailSender == null || mailSender.Host == null || mailSender.Host.Contains("请替换"))
            {
                throw new OsharpException("邮件发送选项不存在，请在appsetting.json配置OSharp.MailSender节点");
            }

            string host = mailSender.Host,
                displayName = mailSender.DisplayName,
                userName = mailSender.UserName,
                password = mailSender.Password;
            SmtpClient client = new SmtpClient(host)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName, password)
            };

            string fromEmail = userName.Contains("@") ? userName : "{0}@{1}".FormatWith(userName, client.Host.Replace("smtp.", ""));
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail, displayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(email);
            return client.SendMailAsync(mail);
        }
    }
}