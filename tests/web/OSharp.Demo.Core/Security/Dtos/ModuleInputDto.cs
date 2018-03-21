// -----------------------------------------------------------------------
//  <copyright file="ModuleInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 14:53</last-date>
// -----------------------------------------------------------------------

using OSharp.Demo.Security.Entities;
using OSharp.Mapping;
using OSharp.Security;


namespace OSharp.Demo.Security.Dtos
{
    /// <summary>
    /// 输入DTO：模块信息
    /// </summary>
    [MapTo(typeof(Module))]
    public class ModuleInputDto : ModuleInputDtoBase<int>
    { }
}