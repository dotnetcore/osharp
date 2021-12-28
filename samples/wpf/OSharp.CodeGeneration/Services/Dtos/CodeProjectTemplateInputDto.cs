// -----------------------------------------------------------------------
//  <copyright file="CodeProjectTemplateInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-12 16:08</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Dtos
{
    [MapTo(typeof(CodeProjectTemplate))]
    public class CodeProjectTemplateInputDto : IInputDto<Guid>
    {
        /// <summary>获取或设置 主键，唯一标识</summary>
        public Guid Id { get; set; }

        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 项目编号
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 获取或设置 模板编号
        /// </summary>
        public Guid TemplateId { get; set; }

    }
}
