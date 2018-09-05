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
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Net
{
    /// <summary>
    /// 默认邮件发送者
    /// </summary>
    public abstract class DefaultEmailSender : IEmailSender
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// SMTP服务器配置参数
        /// </summary>
        public MailSenderOptions mailSenderOptions { get; }

        /// <summary>
        /// 初始化一个<see cref="DefaultEmailSender"/>类型的新实例
        /// </summary>
        protected DefaultEmailSender(IServiceProvider provider)
        {
            _provider = provider;
            OSharpOptions options = _provider.GetOSharpOptions();
            mailSenderOptions = options.MailSender;
            if (mailSenderOptions == null ||
                //mailSenderOptions.DisplayFromAddress.IsNullOrEmpty() ||
                mailSenderOptions.Host.IsNullOrEmpty() ||
                mailSenderOptions.Host.Contains("请替换") ||
                mailSenderOptions.UserName.IsNullOrEmpty() ||
                mailSenderOptions.Password.IsNullOrEmpty())
            {
                throw new OsharpException("邮件发送选项不存在，请在appsetting.json配置OSharp.MailSender节点");
            }
        }

        public virtual SmtpClient Build()
        {
            var client = new SmtpClient();

            try
            {
                ConfigureClient(client);
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }

        protected virtual void ConfigureClient(SmtpClient client)
        {
            client.Connect(
                mailSenderOptions.Host,
                mailSenderOptions.Port,
                GetSecureSocketOption()
            );

            if (mailSenderOptions.UseDefaultCredentials)
            {
                return;
            }

            client.Authenticate(
                mailSenderOptions.UserName,
                mailSenderOptions.Password
            );
        }

        protected virtual SecureSocketOptions GetSecureSocketOption()
        {
            if (mailSenderOptions.SecureSocketOption.HasValue)
            {
                return mailSenderOptions.SecureSocketOption.Value;
            }

            return mailSenderOptions.EnableSsl
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTlsWhenAvailable;
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        public void SendEmail(string to, string subject, string body)
        {
            SendEmail(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body
            });
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await SendEmailAsync(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body
            });
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <param name="isBodyHtml">Email内容是否是Html</param>
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

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <param name="isBodyHtml">Email内容是否是Html</param>
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

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="from">发送人Email</param>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <param name="isBodyHtml">Email内容是否是Html</param>
        /// <returns></returns>
        public void SendEmail(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            SendEmail(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="from">发送人Email</param>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <param name="isBodyHtml">Email内容是否是Html</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendEmailAsync(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="mail">要发送的邮件</param>
        /// <param name="normalize">
        /// 是否要标准化邮件
        /// 如果是，如果他们没有设置地址/名称，他将自动设置，并且设置UTF-8编码
        /// </param>
        public void SendEmail(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                NormalizeMail(mail);
            }
            Send(mail);
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="mail">要发送的邮件</param>
        /// <param name="normalize">
        /// 是否要标准化邮件
        /// 如果是，如果他们没有设置地址/名称，他将自动设置，并且设置UTF-8编码
        /// </param>
        public async Task SendEmailAsync(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                NormalizeMail(mail);
            }
            await SendAsync(mail);
        }

        /// <summary>
        /// 在派生类中实现异步方法来发送电子邮件
        /// </summary>
        /// <param name="mail">发送邮件实体</param>
        protected abstract Task SendAsync(MailMessage mail);

        /// <summary>
        /// 在派生类中实现同步方法来发送电子邮件
        /// </summary>
        /// <param name="mail">发送邮件实体</param>
        protected abstract void Send(MailMessage mail);

        ///// <summary>
        ///// 异步发送邮件
        ///// </summary>
        ///// <param name="mail">要发送的邮件</param>
        ///// <returns></returns>
        //protected async Task SendAsync(MailMessage mail)
        //{
        //    using (var smtpClient = BuildClient())
        //    {
        //        await smtpClient.SendMailAsync(mail);
        //    }
        //}

        ///// <summary>
        ///// 同步发送邮件
        ///// </summary>
        ///// <param name="mail">要发送的邮件</param>
        //protected void Send(MailMessage mail)
        //{
        //    using (var smtpClient = BuildClient())
        //    {
        //        smtpClient.Send(mail);
        //    }
        //}

        /// <summary>
        /// 标准化给定的电子邮件
        /// 如果之前没有赋值则进行默认赋值 <see cref="MailMessage.From"/>
        /// 如果未设置UTF8，则将编码设置为UTF8
        /// </summary>
        /// <param name="mail">要标准化的邮件</param>
        protected virtual void NormalizeMail(MailMessage mail)
        {
            if (mail.From == null || mail.From.Address.IsNullOrEmpty())
            {
                mail.From = new MailAddress(
                    mailSenderOptions.DisplayFromAddress,
                    mailSenderOptions.DisplayName,
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