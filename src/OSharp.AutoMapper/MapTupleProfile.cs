// -----------------------------------------------------------------------
//  <copyright file="MapAttributeProfile.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 21:20</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using AutoMapper;

using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Mapping;
using OSharp.Reflection;


namespace OSharp.AutoMapper
{
    /// <summary>
    /// 创建源类型与目标类型的配对
    /// </summary>
    public class MapTupleProfile : Profile, IMapTuple
    {
        private readonly ILogger<MapTupleProfile> _logger;

        /// <summary>
        /// 初始化一个<see cref="MapTupleProfile"/>类型的新实例
        /// </summary>
        public MapTupleProfile(
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MapTupleProfile>();
        }

        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        public void CreateMap()
        {
            List<(Type Source, Type Target)> tuples = new List<(Type Source, Type Target)>();

            Type[] types = AssemblyManager.FindTypesByAttribute<MapFromAttribute>();
            foreach (Type targetType in types)
            {
                MapFromAttribute attribute = targetType.GetAttribute<MapFromAttribute>(true);
                foreach (Type sourceType in attribute.SourceTypes)
                {
                    var tuple = ValueTuple.Create(sourceType, targetType);
                    tuples.AddIfNotExist(tuple);
                }
            }

            types = AssemblyManager.FindTypesByAttribute<MapToAttribute>();
            foreach (Type sourceType in types)
            {
                MapToAttribute attribute = sourceType.GetAttribute<MapToAttribute>(true);
                foreach (Type targetType in attribute.TargetTypes)
                {
                    var tuple = ValueTuple.Create(sourceType, targetType);
                    tuples.AddIfNotExist(tuple);
                }
            }

            foreach ((Type Source, Type Target) tuple in tuples)
            {
                CreateMap(tuple.Source, tuple.Target);
                _logger.LogDebug($"创建“{tuple.Source}”到“{tuple.Target}”的对象映射关系");
            }
            _logger.LogInformation($"创建了 {tuples.Count} 个对象映射关系");
        }
    }
}