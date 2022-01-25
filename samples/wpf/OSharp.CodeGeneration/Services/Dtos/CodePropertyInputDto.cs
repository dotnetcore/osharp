// -----------------------------------------------------------------------
//  <copyright file="CodePropertyInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-10 21:59</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Dtos
{
    [MapTo(typeof(CodeProperty))]
    public class CodePropertyInputDto : IInputDto<Guid>
    {
        /// <summary>获取或设置 主键，唯一标识</summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 属性名称
        /// </summary>
        [Required(), StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 显示名称
        /// </summary>
        [StringLength(200)]
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 属性类型名称
        /// </summary>
        [Required(), StringLength(500)]
        public string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 是否在列表中列出
        /// </summary>
        public bool Listable { get; set; } = true;

        /// <summary>
        /// 获取或设置 是否可更新
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// 获取或设置 是否可排序
        /// </summary>
        public bool Sortable { get; set; }

        /// <summary>
        /// 获取或设置 是否可筛选
        /// </summary>
        public bool Filterable { get; set; }

        /// <summary>
        /// 获取或设置 是否必须
        /// </summary>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// 获取或设置 最小长度
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// 获取或设置 最大长度
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// 获取或设置 是否值类型可空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 获取或设置 是否枚举类型
        /// </summary>
        public bool IsEnum { get; set; }

        /// <summary>
        /// 获取或设置 是否只读
        /// </summary>
        public bool IsReadonly { get; set; }

        /// <summary>
        /// 获取或设置 是否隐藏
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 获取或设置 是否虚属性
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// 获取或设置 是否外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 获取或设置 是否导航属性
        /// </summary>
        public bool IsNavigation { get; set; }

        /// <summary>
        /// 获取或设置 关联实体
        /// </summary>
        public string RelateEntity { get; set; }

        /// <summary>
        /// 获取或设置 数据权限标识
        /// </summary>
        public string DataAuthFlag { get; set; }

        /// <summary>
        /// 获取或设置 是否包含在输入Dto
        /// </summary>
        public bool IsInputDto { get; set; } = true;

        /// <summary>
        /// 获取或设置 是否包含在输出Dto
        /// </summary>
        public bool IsOutputDto { get; set; } = true;

        /// <summary>
        /// 获取或设置 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 获取或设置 排序号
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 实体编号
        /// </summary>
        public Guid EntityId { get; set; }
    }
}
