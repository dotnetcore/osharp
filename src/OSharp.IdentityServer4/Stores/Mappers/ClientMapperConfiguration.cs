// -----------------------------------------------------------------------
//  <copyright file="ClientMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 11:08</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;

using AutoMapper;
using AutoMapper.Configuration;

using IdentityServer4.Models;

using OSharp.AutoMapper;


namespace OSharp.IdentityServer4.Mappers
{
    public class ClientMapperConfiguration : IAutoMapperConfiguration
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        public void CreateMaps(MapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Entities.Client, Client>()
                .ForMember(dest => dest.ProtocolType, opt => opt.Condition(src => src != null)).ReverseMap();

            mapper.CreateMap<Entities.ClientClaim, Claim>(MemberList.None)
                .ConstructUsing(src => new Claim(src.Type, src.Value)).ReverseMap();

            mapper.CreateMap<Entities.ClientCorsOrigin, string>().ConstructUsing(src => src.Origin).ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.ClientGrantType, string>().ConstructUsing(src => src.GrantType).ReverseMap()
                .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.ClientIdPRestriction, string>().ConstructUsing(src => src.Provider).ReverseMap()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.ClientPostLogoutRedirectUri, string>().ConstructUsing(src => src.PostLogoutRedirectUri)
                .ReverseMap().ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.ClientProperty, KeyValuePair<string, string>>().ReverseMap();

            mapper.CreateMap<Entities.ClientRedirectUri, string>().ConstructUsing(src => src.RedirectUri)
                .ReverseMap().ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.ClientScope, string>().ConstructUsing(src => src.Scope).ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

            mapper.CreateMap<Entities.ClientSecret, Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null)).ReverseMap();
        }
    }
}