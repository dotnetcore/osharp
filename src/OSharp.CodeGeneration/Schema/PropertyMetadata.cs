// -----------------------------------------------------------------------
//  <copyright file="PropertyMetadata.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-06 12:31</last-date>
// -----------------------------------------------------------------------


using OSharp.Collections;


namespace OSharp.CodeGeneration.Schema
{
    /// <summary>
    /// 属性元数据
    /// </summary>
    public class PropertyMetadata
    {
        /// <summary>
        /// 获取或设置 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 属性类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 显示名称
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 是否必须
        /// </summary>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// 获取或设置 最大长度
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// 获取或设置 最小长度
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// 获取或设置 取值范围
        /// </summary>
        public object[] Range { get; set; }

        /// <summary>
        /// 获取或设置 最大值
        /// </summary>
        public object Max { get; set; }

        /// <summary>
        /// 获取或设置 最小值
        /// </summary>
        public object Min { get; set; }

        /// <summary>
        /// 获取或设置 是否值类型可空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 获取或设置 是否虚属性
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// 获取或设置 是否外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 获取或设置 是否包含在输入Dto
        /// </summary>
        public bool IsInputDto { get; set; } = true;

        /// <summary>
        /// 获取或设置 是否包含在输出Dto
        /// </summary>
        public bool IsOutputDto { get; set; } = true;

        /// <summary>
        /// 获取或设置 枚举元数据
        /// </summary>
        public EnumMetadata[] EnumMetadatas { get; set; }

        private EntityMetadata _entity;
        /// <summary>
        /// 获取或设置 所属实体信息
        /// </summary>
        public EntityMetadata Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                value.Properties.AddIfNotExist(this);
            }
        }

        /// <summary>
        /// 是否有验证属性 
        /// </summary>
        public bool HasValidateAttribute()
        {
            return IsRequired.HasValue || MaxLength.HasValue || MinLength.HasValue || Range != null || Max != null || Min != null;
        }

        /// <summary>
        /// 获取属性表示类型的简单类型，如 System.String 返回 string
        /// </summary>
        public string ToSingleTypeName()
        {
            return TypeHelper.ToSingleTypeName(TypeName, IsNullable);
        }

        /// <summary>
        /// 获取属性表示类型的JS类型字符串
        /// </summary>
        public string ToJsTypeName()
        {
            PropertyMetadata prop = this;
            string name = "object";
            switch (prop.TypeName)
            {
                case "System.Byte":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                    name = "number";
                    break;
                case "System.String":
                case "System.Guid":
                    name = "string";
                    break;
                case "System.Boolean":
                    name = "boolean";
                    break;
                case "System.DateTime":
                    name = "date";
                    break;
            }
            if (prop.EnumMetadatas != null)
            {
                name = "number";
            }
            return name;
        }
    }
}