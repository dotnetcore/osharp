using OSharp.Extensions;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OSharp.Net
{
    /// <summary>
    /// 通过SMTP发送电子邮件
    /// </summary>
    public class SmtpEmailSender : DefaultEmailSender
    {
        /// <summary>
        /// 创建一个新的SmtpEmailSender实例 <see cref="SmtpEmailSender"/>.
        /// </summary>
        /// <param name="provider">provider</param>
        public SmtpEmailSender(IServiceProvider provider)
            :base(provider)
        {
        }

        /// <summary>
        /// 创建发送邮件客户端
        /// </summary>
        /// <returns></returns>
        public SmtpClient BuildSmtpClient()
        {
            var host = mailSenderOptions.Host;
            var port = mailSenderOptions.Port;

            var smtpClient = new SmtpClient(host, port);
            try
            {
                if (mailSenderOptions.EnableSsl)
                {
                    smtpClient.EnableSsl = true;
                }

                if (mailSenderOptions.UseDefaultCredentials)
                {
                    smtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = false;

                    var userName = mailSenderOptions.UserName;
                    var password = mailSenderOptions.Password;
                    var domain = mailSenderOptions.Domain;
                    smtpClient.Credentials = !domain.IsNullOrEmpty()
                        ? new NetworkCredential(userName, password, domain)
                        : new NetworkCredential(userName, password);
                }

                return smtpClient;
            }
            catch
            {
                smtpClient.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="mail">要发送的邮件</param>
        /// <returns></returns>
        protected override async Task SendAsync(MailMessage mail)
        {
            using (var smtpClient = BuildSmtpClient())
            {
                await smtpClient.SendMailAsync(mail);
            }
        }

        /// <summary>
        /// 同步发送邮件
        /// </summary>
        /// <param name="mail">要发送的邮件</param>
        protected override void Send(MailMessage mail)
        {
            using (var smtpClient = BuildSmtpClient())
            {
                smtpClient.Send(mail);
            }
        }
    }
}
