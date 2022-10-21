// -----------------------------------------------------------------------
//  <copyright file="ValidateCodeType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-28 22:29</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Drawing
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum ValidateCodeType
    {
        /// <summary>
        /// 纯数值
        /// </summary>
        Number,

        /// <summary>
        /// 数值与字母的组合
        /// </summary>
        NumberAndLetter,

        /// <summary>
        /// 汉字
        /// </summary>
        Hanzi
    }
}