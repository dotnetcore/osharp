// -----------------------------------------------------------------------
//  <copyright file="AjaxResultType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 20:36</last-date>
// -----------------------------------------------------------------------

namespace OSharp.AspNetCore.UI
{
    /// <summary>
    /// 表示 ajax 操作结果类型的枚举
    /// </summary>
    public enum AjaxResultType
    {
        /// <summary>
        /// 消息结果类型
        /// </summary>
        Info,

        /// <summary>
        /// 成功结果类型
        /// </summary>
        Success,

        /// <summary>
        /// 警告结果类型
        /// </summary>
        Warning,

        /// <summary>
        /// 异常结果类型
        /// </summary>
        Error
    }
}