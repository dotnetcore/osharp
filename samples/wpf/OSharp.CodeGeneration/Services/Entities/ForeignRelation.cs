// -----------------------------------------------------------------------
//  <copyright file="ForeignRelation.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-07 22:26</last-date>
// -----------------------------------------------------------------------

namespace OSharp.CodeGeneration.Services.Entities
{
    /// <summary>
    /// 外键关系
    /// </summary>
    public enum ForeignRelation
    {
        /// <summary>
        /// 多对一
        /// </summary>
        ManyToOne,

        /// <summary>
        /// 一对多
        /// </summary>
        OneToMany,

        /// <summary>
        /// 一对一
        /// </summary>
        OneToOne,

        /// <summary>
        /// 拥有单个子项关系
        /// </summary>
        OwnsOne,

        /// <summary>
        /// 拥有多个子项关系
        /// </summary>
        OwnsMany
    }
}
