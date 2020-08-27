// -----------------------------------------------------------------------
//  <copyright file="ModuleFunction.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Authorization.Entities;
using OSharp.Authorization.Functions;


namespace OSharp.Hosting.Authorization.Entities
{
    /// <summary>
    /// 实体类：模块功能信息
    /// </summary>
    [Description("模块功能信息")]
    public class ModuleFunction : ModuleFunctionBase<int>
    {
        /// <summary>
        /// 获取或设置 模块信息
        /// </summary>
        public virtual Module Module { get; set; }

        /// <summary>
        /// 获取或设置 功能信息
        /// </summary>
        public virtual Function Function { get; set; }
    }
}