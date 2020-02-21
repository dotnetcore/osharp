// -----------------------------------------------------------------------
//  <copyright file="IdentityResourceMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 17:08</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using AutoMapper;
using AutoMapper.Configuration;

using IdentityServer4.Models;

using OSharp.AutoMapper;


namespace OSharp.IdentityServer4.Mappers
{
    public class IdentityResourceMapperConfiguration : IAutoMapperConfiguration
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        public void CreateMaps(MapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Entities.IdentityResource, IdentityResource>(MemberList.Destination)
                .ConstructUsing(src => new IdentityResource()).ReverseMap();

            mapper.CreateMap<Entities.IdentityClaim, string>().ConstructUsing(x => x.Type).ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.IdentityResourceProperty, KeyValuePair<string, string>>().ReverseMap();
        }
    }
}