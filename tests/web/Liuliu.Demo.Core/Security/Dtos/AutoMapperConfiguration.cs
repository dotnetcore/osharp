// -----------------------------------------------------------------------
//  <copyright file="AutoMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 0:24</last-date>
// -----------------------------------------------------------------------

using AutoMapper.Configuration;

using Liuliu.Demo.Security.Entities;

using OSharp.AutoMapper;
using OSharp.Dependency;
using OSharp.Extensions;
using OSharp.Filter;
using OSharp.Json;


namespace Liuliu.Demo.Security.Dtos
{
    /// <summary>
    /// DTO对象映射类
    /// </summary>
    public class AutoMapperConfiguration : IAutoMapperConfiguration, ISingletonDependency
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        public void CreateMaps(MapperConfigurationExpression mapper)
        {
            mapper.CreateMap<EntityRoleInputDto, EntityRole>()
                .ForMember(mr => mr.FilterGroupJson, opt => opt.ResolveUsing(dto => dto.FilterGroup.ToJsonString()));

            //mapper.CreateMap<EntityRole, EntityRoleOutputDto>()
            //    .ForMember(dto => dto.FilterGroup, opt => opt.ResolveUsing(mr => mr.FilterGroupJson?.FromJsonString<FilterGroup>()));
        }
    }
}