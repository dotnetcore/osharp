// -----------------------------------------------------------------------
//  <copyright file="AutoMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 0:24</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Authorization.Entities;


namespace OSharp.Hosting.Authorization.Dtos;

/// <summary>
/// DTO对象映射类
/// </summary>
public class AutoMapperConfiguration : AutoMapperTupleBase
{
    /// <summary>
    /// 创建对象映射
    /// </summary>
    public override void CreateMap()
    {
        //mapper.CreateMap<EntityRole, EntityRoleOutputDto>()
        //    .ForMember(dto => dto.FilterGroup, opt => opt.ResolveUsing(mr => mr.FilterGroupJson?.FromJsonString<FilterGroup>()));
    }
}
