// -----------------------------------------------------------------------
//  <copyright file="IEntityRegisterManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-08 2:39</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义实体管理器
    /// </summary>
    public interface IEntityManager
    {
        /// <summary>
        /// 初始化实体类型注册
        /// </summary>
        void Initialize();

        /// <summary>
        /// 获取指定上下文类型的实体配置注册信息
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns></returns>
        IEntityRegister[] GetEntityRegisters(Type dbContextType);

        /// <summary>
        /// 获取 实体类所属的数据上下文类
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>数据上下文类型</returns>
        Type GetDbContextTypeForEntity(Type entityType);
    }
}