// -----------------------------------------------------------------------
//  <copyright file="ApplicationBuilderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 12:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using AutoMapper;
using AutoMapper.Configuration;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Mapping;
using IMapper = OSharp.Mapping.IMapper;


namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// AutoMapper的<see cref="IApplicationBuilder"/>扩展方法
    /// </summary>
    public static class AutoMapperApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用AutoMapper
        /// </summary>
        public static IApplicationBuilder UseAutoMapper(this IApplicationBuilder app,
            Action<IMapperConfigurationExpression> additionalInitAction = null)
        {
            MapperConfigurationExpression cfg = new MapperConfigurationExpression();
            if (additionalInitAction != null)
            {
                additionalInitAction(cfg);
            }

            //获取已注册到IoC的所有Profile
            IMapTuple[] tuples = app.ApplicationServices.GetServices<IMapTuple>().ToArray();
            foreach (IMapTuple mapTuple in tuples)
            {
                mapTuple.CreateMap();
                cfg.AddProfile(mapTuple as Profile);
            }

            Mapper.Initialize(cfg);

            IMapper mapper = app.ApplicationServices.GetService<IMapper>();
            MapperExtensions.SetMapper(mapper);

            return app;
        }
    }
}