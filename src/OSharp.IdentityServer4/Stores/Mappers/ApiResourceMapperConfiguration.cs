// -----------------------------------------------------------------------
//  <copyright file="ApiResourceMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 17:04</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using AutoMapper;
using AutoMapper.Configuration;

using IdentityServer4.Models;

using OSharp.AutoMapper;


namespace OSharp.IdentityServer4.Mappers
{
    public class ApiResourceMapperConfiguration : IAutoMapperConfiguration
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        public void CreateMaps(MapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Entities.ApiResource, ApiResource>(MemberList.Destination).ConstructUsing(src => new ApiResource())
                .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets)).ReverseMap();

            mapper.CreateMap<Entities.ApiResourceClaim, string>().ConstructUsing(x => x.Type).ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.ApiResourceProperty, KeyValuePair<string, string>>().ReverseMap();

            mapper.CreateMap<Entities.ApiScope, Scope>(MemberList.Destination).ConstructUsing(src => new Scope()).ReverseMap();

            mapper.CreateMap<Entities.ApiScopeClaim, string>().ConstructUsing(x => x.Type).ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.ApiSecret, Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(src => src != null)).ReverseMap();
        }
    }
}