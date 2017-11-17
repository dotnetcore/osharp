// -----------------------------------------------------------------------
//  <copyright file="UserInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 13:34</last-date>
// -----------------------------------------------------------------------

using OSharp.Demo.Identity.Entities;
using OSharp.Identity;
using OSharp.Mapping;


namespace OSharp.Demo.Identity.Dtos
{
    /// <summary>
    /// 输入DTO：用户信息
    /// </summary>
    [MapTo(typeof(User))]
    public class UserInputDto : UserInputDtoBase<int>
    { }
}