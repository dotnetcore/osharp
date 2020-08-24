// -----------------------------------------------------------------------
//  <copyright file="EntityInfoBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using OSharp.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Json;
using OSharp.Reflection;


namespace OSharp.Authorization.EntityInfos
{
    /// <summary>
    /// 实体信息基类
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public abstract class EntityInfoBase : EntityBase<Guid>, IEntityInfo
    {
        /// <summary>
        /// 获取或设置 实体名称
        /// </summary>
        [Required, DisplayName("实体名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 实体类型名称
        /// </summary>
        [Required, DisplayName("实体类型名称")]
        public string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 是否启用数据审计
        /// </summary>
        [DisplayName("是否数据审计")]
        public bool AuditEnabled { get; set; } = true;

        /// <summary>
        /// 获取或设置 实体属性信息JSON字符串
        /// </summary>
        [Required, DisplayName("实体属性信息Json字符串")]
        public string PropertyJson { get; set; }

        /// <summary>
        /// 获取 实体属性信息
        /// </summary>
        [NotMapped]
        public EntityProperty[] Properties
        {
            get
            {
                if (string.IsNullOrEmpty(PropertyJson) || !PropertyJson.StartsWith("["))
                {
                    return new EntityProperty[0];
                }

                return PropertyJson.FromJsonString<EntityProperty[]>();
            }
        }

        /// <summary>
        /// 从实体类型初始化实体信息
        /// </summary>
        /// <param name="entityType"></param>
        public virtual void FromType(Type entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            TypeName = entityType.GetFullNameWithModule();
            Name = entityType.GetDescription();
            AuditEnabled = true;

            PropertyInfo[] propertyInfos = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string[] exceptNames = { "DeletedTime" };
            PropertyJson = propertyInfos.Where(m => !exceptNames.Contains(m.Name) && !m.IsVirtual()).Select(property =>
            {
                EntityProperty ep = new EntityProperty()
                {
                    Name = property.Name,
                    Display = property.GetDescription(),
                    TypeName = property.PropertyType.FullName
                };
                //枚举类型，获取枚举项作为取值范围
                if (property.PropertyType.IsEnum)
                {
                    ep.TypeName = typeof(int).FullName;
                    Type enumType = property.PropertyType;
                    Array values = enumType.GetEnumValues();
                    int[] intValues = values.Cast<int>().ToArray();
                    string[] names = values.Cast<Enum>().Select(m => m.ToDescription()).ToArray();
                    for (int i = 0; i < intValues.Length; i++)
                    {
                        string value = intValues[i].ToString();
                        ep.ValueRange.Add(new { id = value, text = names[i] });
                    }
                }

                if (property.HasAttribute<UserFlagAttribute>())
                {
                    ep.IsUserFlag = true;
                }

                return ep;
            }).ToArray().ToJsonString();
        }

        #region Overrides of Object

        /// <summary>返回一个表示当前对象的 string。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return $"{Name}[{TypeName}]";
        }

        #endregion
    }
}