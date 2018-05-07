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
        /// <param name="email">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="message">Email内容</param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}
