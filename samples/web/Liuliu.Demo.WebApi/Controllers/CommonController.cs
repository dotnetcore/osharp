using System.ComponentModel;

using Lazy.Captcha.Core;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Modules;
using OSharp.Hosting.Apis.Controllers;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-通用")]
    [ModuleInfo(Order = 3)]
    public class CommonController : SiteApiControllerBase
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个<see cref="SiteApiControllerBase"/>类型的新实例
        /// </summary>
        public CommonController(IServiceProvider provider)
            : base(provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        [HttpGet]
        [ModuleInfo]
        [Description("生成验证码")]
        public IActionResult GenerateCaptcha(string id)
        {
            ICaptcha captcha = _provider.GetRequiredService<ICaptcha>();
            CaptchaData data = captcha.Generate(id);
            MemoryStream ms = new MemoryStream(data.Bytes);
            return File(ms, "image/gif");
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        [HttpPost]
        [ModuleInfo]
        [Description("验证验证码")]
        public bool ValidateCaptcha(string id, string code)
        {
            ICaptcha captcha = _provider.GetRequiredService<ICaptcha>();
            return captcha.Validate(id, code);
        }
    }
}
