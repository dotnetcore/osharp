// -----------------------------------------------------------------------
//  <copyright file="SiteApiController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-07 1:12</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hosting.Apis.Controllers;

/// <summary>
/// 站点根节点的API控制器基类，使用OSharpPolicy权限策略
/// </summary>
[DisplayName("网站")]
[ApiAuthorize]
public abstract class SiteApiControllerBase : ApiControllerBase
{
    protected static readonly Random Random = new Random();

    /// <summary>
    /// 初始化一个<see cref="SiteApiControllerBase"/>类型的新实例
    /// </summary>
    protected SiteApiControllerBase(IServiceProvider provider)
    { }
}
