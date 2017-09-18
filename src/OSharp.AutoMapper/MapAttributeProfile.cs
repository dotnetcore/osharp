// -----------------------------------------------------------------------
//  <copyright file="MapAttributeProfile.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-14 1:10</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using AutoMapper;

using OSharp.Collections;
using OSharp.Dependency;
using OSharp.Mapping;
using OSharp.Reflection;


namespace OSharp.AutoMapper
{
    /// <inheritdoc cref="IMapTuple" />
    public class MapAttributeProfile : Profile, IMapTuple, ISingletonDependency
    {
        private readonly IMapFromAttributeTypeFinder _mapFromAttributeTypeFinder;
        private readonly IMapToAttributeTypeFinder _mapToAttributeTypeFinder;

        /// <summary>
        /// 初始化一个<see cref="MapAttributeProfile"/>类型的新实例
        /// </summary>
        public MapAttributeProfile(IMapFromAttributeTypeFinder mapFromAttributeTypeFinder, IMapToAttributeTypeFinder mapToAttributeTypeFinder)
        {
            _mapFromAttributeTypeFinder = mapFromAttributeTypeFinder;
            _mapToAttributeTypeFinder = mapToAttributeTypeFinder;
        }

        /// <inheritdoc />
        public void CreateMap()
        {
            List<(Type Source, Type Target)> tuples = new List<(Type Source, Type Target)>();

            Type[] types = _mapFromAttributeTypeFinder.FindAll(true);
            foreach (Type targetType in types)
            {
                MapFromAttribute attribute = targetType.GetAttribute<MapFromAttribute>();
                foreach (Type sourceType in attribute.SourceTypes)
                {
                    var tuple = ValueTuple.Create(sourceType, targetType);
                    tuples.AddIfNotExist(tuple);
                }
            }

            types = _mapToAttributeTypeFinder.FindAll(true);
            foreach (Type sourceType in types)
            {
                MapToAttribute attribute = sourceType.GetAttribute<MapToAttribute>();
                foreach (Type targetType in attribute.TargetTypes)
                {
                    var tuple = ValueTuple.Create(sourceType, targetType);
                    tuples.AddIfNotExist(tuple);
                }
            }

            foreach ((Type Source, Type Target) tuple in tuples)
            {
                CreateMap(tuple.Source, tuple.Target);
            }
        }
    }
}