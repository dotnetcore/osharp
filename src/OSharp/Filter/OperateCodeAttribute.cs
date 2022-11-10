// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:04 18:08</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Filter;

/// <summary>
/// 表示查询操作的前台操作码
/// </summary>
public class OperateCodeAttribute : Attribute
{
    /// <summary>
    /// 初始化一个<see cref="OperateCodeAttribute"/>类型的新实例
    /// </summary>
    public OperateCodeAttribute(string code)
    {
        Code = code;
    }

    /// <summary>
    /// 获取 属性名称
    /// </summary>
    public string Code { get; private set; }
}