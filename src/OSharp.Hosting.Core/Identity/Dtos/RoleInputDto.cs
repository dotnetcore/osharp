// -----------------------------------------------------------------------
//  <copyright file="RoleInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;

using OSharp.Identity.Dtos;
using OSharp.Mapping;


namespace OSharp.Hosting.Identity.Dtos
{
    /// <summary>
    /// 输入DTO：角色信息
    /// </summary>
    [MapTo(typeof(Role))]
    public class RoleInputDto : RoleInputDtoBase<int>
    { }
}