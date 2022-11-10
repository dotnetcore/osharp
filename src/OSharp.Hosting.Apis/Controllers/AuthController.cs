// -----------------------------------------------------------------------
//  <copyright file="AuthController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-12 12:20</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hosting.Apis.Controllers;

[Description("网站-授权")]
[ModuleInfo(Order = 2)]
public class AuthController : SiteApiControllerBase
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// 初始化一个<see cref="SiteApiControllerBase"/>类型的新实例
    /// </summary>
    public AuthController(IServiceProvider provider)
        : base(provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// 检查URL授权
    /// </summary>
    /// <param name="url">要检查的URL</param>
    /// <returns>是否有权</returns>
    [HttpGet]
    [UnitOfWork]
    [Description("检查URL授权")]
    public bool CheckUrlAuth(string url)
    {
        bool flag = this.CheckFunctionAuth(url);
        return flag;
    }

    /// <summary>
    /// 获取授权信息
    /// 步骤：
    /// 1.获取初始化时缓存的所有ModuleInfo信息，此信息已经包含最新版本的Module->Function[]信息
    /// 2.判断当前用户对于Function的权限
    /// 3.提取有效的模块代码节点
    /// </summary> 
    /// <returns>权限节点</returns>
    [HttpGet]
    [ModuleInfo]
    [Description("获取授权信息")]
    public string[] GetAuthInfo()
    {
        IModuleHandler moduleHandler = _provider.GetRequiredService<IModuleHandler>();
        IFunctionAuthorization functionAuthorization = _provider.GetRequiredService<IFunctionAuthorization>();
        ModuleInfo[] moduleInfos = moduleHandler.ModuleInfos;
            
        //先查找出所有有权限的模块
        List<ModuleInfo> authModules = new List<ModuleInfo>();
        foreach (ModuleInfo moduleInfo in moduleInfos)
        {
            bool hasAuth = moduleInfo.DependOnFunctions.All(m => functionAuthorization.Authorize(m, User).IsOk);
            if (moduleInfo.DependOnFunctions.Length == 0 || hasAuth)
            {
                authModules.Add(moduleInfo);
            }
        }

        List<string> codes = new List<string>();
        foreach (ModuleInfo moduleInfo in authModules)
        {
            string fullCode = moduleInfo.FullCode;
            //模块下边有功能，或者拥有子模块
            if (moduleInfo.DependOnFunctions.Length > 0 
                || authModules.Any(m => m.FullCode.Length > fullCode.Length && m.FullCode.Contains(fullCode) && m.DependOnFunctions.Length > 0))
            {
                codes.AddIfNotExist(fullCode);
            }
        }
        return codes.ToArray();
    }

}
