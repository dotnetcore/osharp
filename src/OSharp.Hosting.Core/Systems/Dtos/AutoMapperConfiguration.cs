// -----------------------------------------------------------------------
//  <copyright file="AutoMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-01 11:10</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using AutoMapper.Configuration;

using OSharp.AutoMapper;
using OSharp.Dependency;
using OSharp.Hosting.Systems.Entities;
using OSharp.Mapping;


namespace OSharp.Hosting.Systems.Dtos
{
    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        public void CreateMaps(MapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Menu, MenuOutputDto>()
                .ForMember(dto => dto.Children, opt => opt.MapFrom(entity => entity.Children.Select(m => m.MapTo<MenuOutputDto>())));
        }
    }
}