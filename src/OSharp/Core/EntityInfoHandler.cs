// -----------------------------------------------------------------------
//  <copyright file="EntityInfoHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-14 18:27</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Dependency;


namespace OSharp.Core
{
    /// <summary>
    /// 实体信息处理器
    /// </summary>
    public class EntityInfoHandler : EntityInfoHandlerBase<EntityInfo, EntityInfoHandler>, ISingletonDependency
    {
        /// <summary>
        /// 初始化一个<see cref="EntityInfoHandlerBase{TEntityInfo,TEntityInfoProvider}"/>类型的新实例
        /// </summary>
        public EntityInfoHandler(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }
    }
}