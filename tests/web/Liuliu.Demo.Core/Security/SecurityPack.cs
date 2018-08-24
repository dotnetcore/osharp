// -----------------------------------------------------------------------
//  <copyright file="SecurityPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Liuliu.Demo.Security.Dtos;
using Liuliu.Demo.Security.Entities;

using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Security;
using OSharp.Secutiry;


namespace Liuliu.Demo.Security
{
    /// <summary>
    /// 权限安全模块
    /// </summary>
    [Description("权限安全模块")]
    public class SecurityPack
        : SecurityPackBase<SecurityManager, FunctionAuthorization, FunctionAuthCache, DataAuthCache, ModuleHandler,
            Function, FunctionInputDto, EntityInfo, EntityInfoInputDto, Module, ModuleInputDto, int, ModuleFunction,
            ModuleRole, ModuleUser, EntityRole, EntityRoleInputDto, int, int>
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 1;
    }
}