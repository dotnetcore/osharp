// -----------------------------------------------------------------------
//  <copyright file="ClientMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 11:08</last-date>
// -----------------------------------------------------------------------

using System;
using System.Security.Claims;

using AutoMapper;
using AutoMapper.Configuration;

using IdentityServer4.Models;

using OSharp.AutoMapper;


namespace OSharp.IdentityServer4
{
    public class ClientMapperConfiguration : IAutoMapperConfiguration
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        public void CreateMaps(MapperConfigurationExpression mapper)
        {
            
        }
    }
}