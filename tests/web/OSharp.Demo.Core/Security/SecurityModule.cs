// -----------------------------------------------------------------------
//  <copyright file="SecurityModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-11 10:46</last-date>
// -----------------------------------------------------------------------

using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Demo.Security.Dtos;
using OSharp.Demo.Security.Entities;
using OSharp.Security;


namespace OSharp.Demo.Security
{
    public class SecurityModule
        : SecurityModuleBase<SecurityManager, Function, FunctionInputDto, EntityInfo, EntityInfoInputDto,
            Module, ModuleInputDto, int, ModuleFunction, ModuleRole, ModuleUser, int, int>
    { }
}