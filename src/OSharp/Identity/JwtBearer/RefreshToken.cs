// -----------------------------------------------------------------------
//  <copyright file="RefreshToken.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-02 3:52</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Identity.JwtBearer;

/// <summary>
/// 刷新Token信息
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// 获取或设置 客户端Id
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 获取或设置 标识值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 获取或设置 过期时间
    /// </summary>
    public DateTime EndUtcTime { get; set; }
}