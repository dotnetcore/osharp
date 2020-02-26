// -----------------------------------------------------------------------
//  <copyright file="UserRoleInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Identity.Entities;

using OSharp.Identity.Dtos;
using OSharp.Mapping;


namespace Liuliu.Demo.Identity.Dtos
{
    /// <summary>
    /// 输入DTO：用户角色信息
    /// </summary>
    [MapTo(typeof(UserRole))]
    public class UserRoleInputDto : UserRoleInputDtoBase<Guid, int, int>
    { }
}