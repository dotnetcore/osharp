// -----------------------------------------------------------------------
//  <copyright file="CommonController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-12 16:30</last-date>
// -----------------------------------------------------------------------

using System.Reflection;

using Lazy.Captcha.Core;

using AssemblyExtensions = OSharp.Reflection.AssemblyExtensions;


namespace OSharp.Hosting.Apis.Controllers;

[Description("网站-通用")]
[ModuleInfo(Order = 3)]
//[ApiExplorerSettings(GroupName = "buss")]
public class CommonController : SiteApiControllerBase
{
    private readonly IServiceProvider _provider;

    private IVerifyCodeService VerifyCodeService => _provider.GetRequiredService<IVerifyCodeService>();

    private IWebHostEnvironment Environment => _provider.GetRequiredService<IWebHostEnvironment>();

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
    public IActionResult Captcha(string id)
    {
        ICaptcha captcha = _provider.GetRequiredService<ICaptcha>();
        CaptchaData data = captcha.Generate(id);
        MemoryStream ms = new MemoryStream(data.Bytes);
        return File(ms, "image/gif");
    }

    /// <summary>
    /// 检查验证码
    /// </summary>
    [HttpGet]
    [ModuleInfo]
    [Description("检查验证码")]
    public IActionResult CheckCaptcha(string id, string code)
    {
        ICaptcha captcha = _provider.GetRequiredService<ICaptcha>();
        bool flag = captcha.Validate(id, code, false);
        return new JsonResult(flag);
    }

    /// <summary>
    /// 获取系统信息
    /// </summary>
    /// <returns>系统信息</returns>
    [HttpGet]
    [ModuleInfo]
    [Description("系统信息")]
    public object SystemInfo()
    {
        IServiceProvider provider = HttpContext.RequestServices;

        dynamic info = new ExpandoObject();
        info.Packs = provider.GetAllPacks().Select(m => new
        {
            m.GetType().Name,
            Class = m.GetType().FullName,
            Level = m.Level.ToString(),
            m.Order,
            m.IsEnabled
        }).ToList();

        string cliVersion = AssemblyExtensions.GetCliVersion();
        string osharpVersion = Assembly.GetExecutingAssembly().GetProductVersion();

        info.Object = new
        {
            Message = "WebApi 数据服务已启动",
            CliVersion = cliVersion,
            OsharpVersion = osharpVersion
        };

        return info;
    }
}
