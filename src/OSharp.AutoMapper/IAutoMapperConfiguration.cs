// -----------------------------------------------------------------------
//  <copyright file="IMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 0:22</last-date>
// -----------------------------------------------------------------------

using AutoMapper.Configuration;

using OSharp.Dependency;


namespace OSharp.AutoMapper
{
    /// <summary>
    /// 定义通过<see cref="MapperConfigurationExpression"/>配置对象映射的功能
    /// </summary>
    [MultipleDependency]
    public interface IAutoMapperConfiguration
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        void CreateMaps(MapperConfigurationExpression mapper);
    }
}