// -----------------------------------------------------------------------
//  <copyright file="ITypeMetadataHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-06 13:42</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// 类型元数据处理器
    /// </summary>
    public interface ITypeMetadataHandler
    {
        /// <summary>
        /// 获取实体类的元数据
        /// </summary>
        /// <returns>元数据集合</returns>
        TypeMetadata[] GetEntityTypeMetadatas();

        /// <summary>
        /// 获取输入DTO类型的元数据
        /// </summary>
        /// <returns>元数据集合</returns>
        TypeMetadata[] GetInputDtoMetadatas();

        /// <summary>
        /// 获取输出DTO类型的元数据
        /// </summary>
        /// <returns>元数据集合</returns>
        TypeMetadata[] GetOutputDtoMetadata();

        /// <summary>
        /// 获取指定类型的元数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>元数据</returns>
        TypeMetadata GetTypeMetadata(Type type);
    }
}