// -----------------------------------------------------------------------
//  <copyright file="MvcOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-20 15:15</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Options;

/// <summary>
/// Mvc选项
/// </summary>
public class MvcOptions
{
    /// <summary>
    /// 获取或设置 Json属性小写，默认：true
    /// </summary>
    public bool IsLowercaseJsonProperty { get; set; } = true;

    /// <summary>
    /// 获取或设置 是否URL小写，默认：false
    /// </summary>
    public bool IsLowercaseUrls { get; set; } = false;

    /// <summary>
    /// 获取或设置 是否将Long类型转换为字符串，默认：true
    /// </summary>
    public bool IsLongToStringConvert { get; set; } = true;

}
