// -----------------------------------------------------------------------
//  <copyright file="EntityInfoHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:14</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Authorization.EntityInfos
{
    /// <summary>
    /// 实体信息处理器
    /// </summary>
    public class EntityInfoHandler : EntityInfoHandlerBase<EntityInfo, EntityInfoHandler>
    {
        /// <summary>
        /// 初始化一个<see cref="EntityInfoHandlerBase{TEntityInfo,TEntityInfoProvider}"/>类型的新实例
        /// </summary>
        public EntityInfoHandler(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }
    }
}