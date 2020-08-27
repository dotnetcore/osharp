// -----------------------------------------------------------------------
//  <copyright file="ModuleHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Authorization.Entities;

using OSharp.Authorization;


namespace OSharp.Hosting.Authorization
{
    /// <summary>
    /// 模块信息处理器
    /// </summary>
    public class ModuleHandler : ModuleHandlerBase<Module, ModuleInputDto, int, ModuleFunction>
    {
        /// <summary>
        /// 初始化一个<see cref="ModuleHandlerBase{TModule, TModuleInputDto, TModuleKey, TModuleFunction}"/>类型的新实例
        /// </summary>
        public ModuleHandler(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }
    }
}