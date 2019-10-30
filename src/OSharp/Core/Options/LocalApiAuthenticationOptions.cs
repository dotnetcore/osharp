namespace OSharp.Core.Options
{
    public class LocalApiAuthenticationOptions
    {
        /// <summary>
        /// 获取或设置 Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置 是否使用Https
        /// </summary>
        public bool UseHttps { get; set; } = false;

        /// <summary>
        /// 获取或设置 验证资源
        /// </summary>
        public string Audience { get; set; }
    }
}
