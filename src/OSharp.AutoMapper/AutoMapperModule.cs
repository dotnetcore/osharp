// -----------------------------------------------------------------------
//  <copyright file="AutoMapperModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 21:13</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using AutoMapper;
using AutoMapper.Configuration;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core;
using OSharp.Mapping;

using IMapper = OSharp.Mapping.IMapper;


namespace OSharp.AutoMapper
{
    /// <summary>
    /// AutoMapper模块
    /// </summary>
    public class AutoMapperModule : OSharpModule
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IMapFromAttributeTypeFinder, MapFromAttributeTypeFinder>();
            services.AddSingleton<IMapToAttributeTypeFinder, MapToAttributeTypeFinder>();
            services.AddSingleton<IMapTuple, MapAttributeProfile>();
            services.AddSingleton<IMapper, AutoMapperMapper>();

            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider"></param>
        public override void UseServices(IServiceProvider provider)
        {
            MapperConfigurationExpression cfg = new MapperConfigurationExpression();

            //获取已注册到IoC的所有Profile
            IMapTuple[] tuples = provider.GetServices<IMapTuple>().ToArray();
            foreach (IMapTuple mapTuple in tuples)
            {
                mapTuple.CreateMap();
                cfg.AddProfile(mapTuple as Profile);
            }

            Mapper.Initialize(cfg);

            IMapper mapper = provider.GetService<IMapper>();
            MapperExtensions.SetMapper(mapper);
        }
    }
}