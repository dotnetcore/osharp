// -----------------------------------------------------------------------
//  <copyright file="CodeForeignInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-10 22:03</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Dtos
{
    [MapTo(typeof(CodeForeign))]
    public class CodeForeignInputDto : IInputDto<Guid>
    {
        /// <summary>获取或设置 主键，唯一标识</summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 己方导航属性
        /// </summary>
        public string SelfNavigation { get; set; }

        /// <summary>
        /// 获取或设置 己方外键属性
        /// </summary>
        public string SelfForeignKey { get; set; }

        /// <summary>
        /// 获取或设置 对方实体
        /// </summary>
        public string OtherEntity { get; set; }

        /// <summary>
        /// 获取或设置 对方导航属性
        /// </summary>
        public string OtherNavigation { get; set; }

        /// <summary>
        /// 获取或设置 是否必须
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 获取或设置 关系数据删除行为
        /// </summary>
        public DeleteBehavior? DeleteBehavior { get; set; }

        /// <summary>
        /// 获取或设置 外键关系
        /// </summary>
        public ForeignRelation ForeignRelation { get; set; }

        /// <summary>
        /// 获取或设置 实体编号
        /// </summary>
        public Guid EntityId { get; set; }

    }
}
