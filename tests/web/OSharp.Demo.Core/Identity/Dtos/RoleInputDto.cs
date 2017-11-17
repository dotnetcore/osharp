// -----------------------------------------------------------------------
//  <copyright file="RoleInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 13:39</last-date>
// -----------------------------------------------------------------------

using OSharp.Demo.Identity.Entities;
using OSharp.Identity;
using OSharp.Mapping;


namespace OSharp.Demo.Identity.Dtos
{
    /// <summary>
    /// 输入DTO：角色信息
    /// </summary>
    [MapTo(typeof(Role))]
    public class RoleInputDto : RoleInputDtoBase<int>
    { }
}