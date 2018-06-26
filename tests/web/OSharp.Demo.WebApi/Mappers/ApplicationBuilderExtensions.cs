// -----------------------------------------------------------------------
//  <copyright file="ApplicationBuilderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-26 22:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.Composition.Convention;
using System.Linq;

using AutoMapper;
using AutoMapper.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Dependency;


namespace OSharp.Demo.WebApi.Mappers
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/>辅助扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 接受额外的Dto映射
        /// </summary>
        public static IApplicationBuilder UseDtoMappings(this IApplicationBuilder app)
        {
            IServiceProvider provider = app.ApplicationServices;
            MapperConfigurationExpression mapper = provider.GetService<MapperConfigurationExpression>();

            mapper.CreateMap<Role, RoleNode>().ForMember(rn => rn.RoleId, opt => opt.MapFrom(r => r.Id)).ForMember(rn => rn.RoleName, opt => opt.MapFrom(r => r.Name));

            return app;
        }

    }
}