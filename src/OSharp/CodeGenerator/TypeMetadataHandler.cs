// -----------------------------------------------------------------------
//  <copyright file="TypeMetadataHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-06 13:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using OSharp.Entity;
using OSharp.Reflection;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// 类型元数据处理器
    /// </summary>
    public class TypeMetadataHandler : ITypeMetadataHandler
    {
        /// <summary>
        /// 获取实体类的元数据
        /// </summary>
        /// <returns>元数据集合</returns>
        public TypeMetadata[] GetEntityTypeMetadatas()
        {
            Type[] entityTypes = AssemblyManager.FindTypesByBase(typeof(IEntity<>)).Where(m => !m.HasAttribute<IgnoreGenTypeAttribute>()).ToArray();
            return entityTypes.OrderBy(m => m.FullName).Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// 获取输入DTO类型的元数据
        /// </summary>
        /// <returns>元数据集合</returns>
        public TypeMetadata[] GetInputDtoMetadatas()
        {
            Type[] inputDtoTypes = AssemblyManager.FindTypesByBase(typeof(IInputDto<>)).Where(m => !m.HasAttribute<IgnoreGenTypeAttribute>())
                .ToArray();
            return inputDtoTypes.OrderBy(m => m.FullName).Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// 获取输出DTO类型的元数据
        /// </summary>
        /// <returns>元数据集合</returns>
        public TypeMetadata[] GetOutputDtoMetadata()
        {
            Type[] outDtoTypes = AssemblyManager.FindTypesByBase(typeof(IOutputDto)).Where(m => !m.HasAttribute<IgnoreGenTypeAttribute>()).ToArray();
            return outDtoTypes.OrderBy(m => m.FullName).Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// 获取指定类型的元数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>元数据</returns>
        public TypeMetadata GetTypeMetadata(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return new TypeMetadata(type);
        }
    }
}