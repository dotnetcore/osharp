using System.Net.Mail;
using System.Threading.Tasks;


namespace OSharp.Net
{
    /// <summary>
    /// 定义Email发送功能
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        void SendEmail(string to, string subject, string body);

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <returns></returns>
        Task SendEmailAsync(string to, string subject, string body);

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <param name="isBodyHtml">Email内容是否是Html</param>
        void SendEmail(string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <param name="isBodyHtml">Email内容是否是Html</param>
        /// <returns></returns>
        Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="from">发送人Email</param>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <param name="isBodyHtml">Email内容是否是Html</param>
        void SendEmail(string from, string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="from">发送人Email</param>
        /// <param name="to">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <param name="isBodyHtml">Email内容是否是Html</param>
        /// <returns></returns>
        Task SendEmailAsync(string from, string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="mail">Mail to be sent</param>
        /// <param name="normalize">
        /// Should normalize email?
        /// If true, it sets sender address/name if it's not set before and makes mail encoding UTF-8. 
        /// </param>
        void SendEmail(MailMessage mail, bool normalize = true);

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="mail">Mail to be sent</param>
        /// <param name="normalize">
        /// Should normalize email?
        /// If true, it sets sender address/name if it's not set before and makes mail encoding UTF-8. 
        /// </param>
        Task SendEmailAsync(MailMessage mail, bool normalize = true);
    }
}
