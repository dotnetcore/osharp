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
using System.Text;
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
    public class DefaultEmailSender : IEmailSender
    {
        private readonly IServiceProvider _provider;

        private readonly MailSenderOptions _mailSenderOptions;

        /// <summary>
        /// 初始化一个<see cref="DefaultEmailSender"/>类型的新实例
        /// </summary>
        public DefaultEmailSender(IServiceProvider provider)
        {
            _provider = provider;
            OSharpOptions options = _provider.GetOSharpOptions();
            _mailSenderOptions = options.MailSender;
        }

        public void SendEmail(string to, string subject, string body)
        {
            SendEmail(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body
            });
        }

        public void SendEmail(string to, string subject, string body, bool isBodyHtml = true)
        {
            SendEmail(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            });
        }

        public void SendEmail(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            SendEmail(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        public void SendEmail(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                NormalizeMail(mail);
            }
            SendEmail(mail);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await SendEmailAsync(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body
            });
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendEmailAsync(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            });
        }

        public async Task SendEmailAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendEmailAsync(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        public async Task SendEmailAsync(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                NormalizeMail(mail);
            }
            await SendEmailAsync(mail);
        }

        /// <summary>
        /// Normalizes given email.
        /// Fills <see cref="MailMessage.From"/> if it's not filled before.
        /// Sets encodings to UTF8 if they are not set before.
        /// </summary>
        /// <param name="mail">Mail to be normalized</param>
        protected virtual void NormalizeMail(MailMessage mail)
        {
            if (mail.From == null || mail.From.Address.IsNullOrEmpty())
            {
                mail.From = new MailAddress(
                    _mailSenderOptions.DisplayName,
                    _mailSenderOptions.DisplayName,
                    Encoding.UTF8
                    );
            }

            if (mail.HeadersEncoding == null)
            {
                mail.HeadersEncoding = Encoding.UTF8;
            }

            if (mail.SubjectEncoding == null)
            {
                mail.SubjectEncoding = Encoding.UTF8;
            }

            if (mail.BodyEncoding == null)
            {
                mail.BodyEncoding = Encoding.UTF8;
            }
        }

        ///// <summary>
        ///// 发送Email
        ///// </summary>
        ///// <param name="to">接收人Email</param>
        ///// <param name="subject">Email标题</param>
        ///// <param name="body">Email内容</param>
        ///// <returns></returns>
        //public Task SendEmailAsync(string to, string subject, string body)
        //{
        //    OSharpOptions options = _provider.GetOSharpOptions();
        //    MailSenderOptions mailSender = options.MailSender;
        //    if (mailSender == null || mailSender.Host == null || mailSender.Host.Contains("请替换"))
        //    {
        //        throw new OsharpException("邮件发送选项不存在，请在appsetting.json配置OSharp.MailSender节点");
        //    }

        //    string host = mailSender.Host,
        //        displayName = mailSender.DisplayName,
        //        userName = mailSender.UserName,
        //        password = mailSender.Password;
        //    int port = mailSender.Port,
        //        timeout = mailSender.Timeout;
        //    bool enableSsl = mailSender.EnableSsl;
        //    SmtpClient client = new SmtpClient(host, port)
        //    {
        //        EnableSsl = enableSsl,
        //        Timeout = timeout,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(userName, password)
        //    };

        //    string fromEmail = userName.Contains("@") ? userName : "{0}@{1}".FormatWith(userName, client.Host.Replace("smtp.", ""));
        //    MailMessage mail = new MailMessage
        //    {
        //        From = new MailAddress(fromEmail, displayName),
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true
        //    };
        //    mail.To.Add(to);
        //    return client.SendMailAsync(mail);
        //}
    }
}