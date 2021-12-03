// -----------------------------------------------------------------------
//  <copyright file="TypeMetadataHandler.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-06 13:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using OSharp.Entity;
using OSharp.Reflection;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// ����Ԫ���ݴ�����
    /// </summary>
    public class TypeMetadataHandler : ITypeMetadataHandler
    {
        /// <summary>
        /// ��ȡʵ�����Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        public TypeMetadata[] GetEntityTypeMetadatas()
        {
            Type[] entityTypes = AssemblyManager.FindTypesByBase(typeof(IEntity<>)).Where(m => !m.HasAttribute<IgnoreGenTypeAttribute>()).ToArray();
            return entityTypes.OrderBy(m => m.FullName).Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// ��ȡ����DTO���͵�Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        public TypeMetadata[] GetInputDtoMetadatas()
        {
            Type[] inputDtoTypes = AssemblyManager.FindTypesByBase(typeof(IInputDto<>)).Where(m => !m.HasAttribute<IgnoreGenTypeAttribute>())
                .ToArray();
            return inputDtoTypes.OrderBy(m => m.FullName).Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// ��ȡ���DTO���͵�Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        public TypeMetadata[] GetOutputDtoMetadata()
        {
            Type[] outDtoTypes = AssemblyManager.FindTypesByBase(typeof(IOutputDto)).Where(m => !m.HasAttribute<IgnoreGenTypeAttribute>()).ToArray();
            return outDtoTypes.OrderBy(m => m.FullName).Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// ��ȡָ�����͵�Ԫ����
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>Ԫ����</returns>
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