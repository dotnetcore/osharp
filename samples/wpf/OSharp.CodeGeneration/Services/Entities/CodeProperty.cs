// -----------------------------------------------------------------------
//  <copyright file="CodeProperty.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-04 23:06</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using OSharp.Entity;


namespace OSharp.CodeGeneration.Services.Entities
{
    /// <summary>
    /// 实体类：代码实体属性信息
    /// </summary>
    [Description("代码实体属性")]
    [TableNamePrefix("CodeGen")]
    public class CodeProperty : EntityBase<Guid>, ICreatedTime, ILockable
    {
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
        /// 获取或设置 是否在列表中显示
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
        public bool IsReadonly { get; set; } = false;

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

        /// <summary>获取或设置 创建时间</summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 实体编号
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// 获取或设置 所属实体信息
        /// </summary>
        [JsonIgnore]
        public virtual CodeEntity Entity { get; set; }

        public static CodeProperty GetProperty(string name, string display, Type type, bool input, bool output = true, bool filter = true, bool sort = true, bool update = true) => new CodeProperty()
        {
            Name = name, Display = display, TypeName = type.FullName, Filterable = filter, Sortable = sort, IsInputDto = input,
            IsOutputDto = output, Updatable = update
        };

        public bool IsString()
        {
            return TypeName != "System.String";
        }

        public bool IsValueType()
        {
            return Type.GetType(TypeName)?.IsValueType == true || IsEnum;
        }
    }
}
