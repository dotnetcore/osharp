// -----------------------------------------------------------------------
//  <copyright file="IEntityConfigurationTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-17 2:20</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Dependency;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义实体类配置类型查找器
    /// </summary>
    public interface IEntityConfigurationTypeFinder : ITypeFinder
    {
        /// <summary>
        /// 获取指定上下文类型的实体配置注册信息
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns></returns>
        IEntityRegister[] GetEntityRegisters(Type dbContextType);
    }
}