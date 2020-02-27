// -----------------------------------------------------------------------
//  <copyright file="EntityInfoInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-15 17:26</last-date>
// -----------------------------------------------------------------------

using OSharp.Authorization.EntityInfos;
using OSharp.Mapping;


namespace OSharp.Authorization.Dtos
{
    /// <summary>
    /// 输入DTO：实体信息
    /// </summary>
    [MapTo(typeof(EntityInfo))]
    public class EntityInfoInputDto : EntityInfoInputDtoBase
    { }
}