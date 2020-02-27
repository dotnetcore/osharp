// -----------------------------------------------------------------------
//  <copyright file="ModuleFunctionBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-17 20:01</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.Authorization.Entities
{
    /// <summary>
    /// 模块功能信息基类
    /// </summary>
    [TableNamePrefix("Auth")]
    public abstract class ModuleFunctionBase<TModuleKey> : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 模块编号
        /// </summary>
        [DisplayName("模块编号")]
        public TModuleKey ModuleId { get; set; }

        /// <summary>
        /// 获取或设置 功能编号
        /// </summary>
        [DisplayName("功能编号")]
        public Guid FunctionId { get; set; }
    }
}