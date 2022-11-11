// -----------------------------------------------------------------------
//  <copyright file="MailKitSender.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-01-17 22:20</last-date>
// -----------------------------------------------------------------------

using MailKit.Net.Smtp;

using MimeKit;
using MimeKit.Text;

namespace OSharp.Hosting.Utils;

public class MailKitSender : IEmailSender
{
    private readonly ILogger<MailKitSender> _logger;
    private readonly OsharpOptions _options;

    /// <summary>
    /// 初始化一个<see cref="MailKitSender"/>类型的新实例
    /// </summary>
    public MailKitSender(IServiceProvider provider)
    {
        _logger = provider.GetLogger<MailKitSender>();
        _options = provider.GetOSharpOptions();
    }

    /// <summary>
    /// 发送Email
    /// </summary>
    /// <param name="email">接收人Email</param>
    /// <param name="subject">Email标题</param>
    /// <param name="message">Email内容</param>
    /// <returns></returns>
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        MailSenderOptions mailSender = _options.MailSender;
        if (mailSender == null || mailSender.Host == null || mailSender.Host.Contains("请替换"))
        {
            throw new OsharpException("邮件发送选项不存在，请在appsettings.json配置OSharp:MailSender节点");
        }

        string host = mailSender.Host,
            displayName = mailSender.DisplayName,
            userName = mailSender.UserName,
            password = mailSender.Password;
        bool enableSsl = mailSender.EnableSsl;
        var port = mailSender.Port;
        if (port == 0)
        {
            port = enableSsl ? 465 : 25;
        }

        MailboxAddress sender = new MailboxAddress(displayName, userName);
        MimeMessage mime = new MimeMessage()
        {
            Sender = sender,
            Subject = subject,
            Body = new TextPart(TextFormat.Html) { Text = message }
        };
        mime.From.Add(sender);
        mime.To.Add(new MailboxAddress("", email));
        using var smtp = new SmtpClient();
        smtp.MessageSent += (s, e) =>
        {
            _logger.LogDebug($"邮件“{subject}”发送到“{email}”结果：{e.Response}");
        };
        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
        await smtp.ConnectAsync(host, port, enableSsl);
        await smtp.AuthenticateAsync(userName, password);
        await smtp.SendAsync(mime);
        await smtp.DisconnectAsync(true);
    }
}
