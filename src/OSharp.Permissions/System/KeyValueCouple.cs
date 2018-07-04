// -----------------------------------------------------------------------
//  <copyright file="KeyValueCouple.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-25 21:32</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

using OSharp.Core.Data;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Json;


namespace OSharp.System
{
    /// <summary>
    /// 实体类：数据键值对
    /// </summary>
    [Description("键值对信息")]
    public class KeyValueCouple : EntityBase<Guid>, ILockable, IKeyValueCouple
    {
        /// <summary>
        /// 初始化一个<see cref="KeyValueCouple"/>类型的新实例
        /// </summary>
        public KeyValueCouple()
        { }

        /// <summary>
        /// 初始化一个<see cref="KeyValueCouple"/>类型的新实例
        /// </summary>
        public KeyValueCouple(string key, object value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 获取或设置 数据键名
        /// </summary>
        [Required]
        [DisplayName("数据键名")]
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置 数据值JSON字符串
        /// </summary>
        [DisplayName("数据值JSON")]
        public string ValueJson { get; set; }

        /// <summary>
        /// 获取或设置 数据值类型
        /// </summary>
        [DisplayName("数据值类型名")]
        public string ValueType { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        [DisplayName("是否锁定")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 数据值
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        [DisplayName("数据值")]
        public object Value
        {
            get
            {
                if (ValueJson == null || ValueType == null)
                {
                    return null;
                }
                Type type = Type.GetType(ValueType);
                if (type == null)
                {
                    throw new OsharpException($"获取Key为“{Key}”的字典值时类型“{ValueType}”无法获取");
                }
                return ValueJson.FromJsonString(type);
            }
            set
            {
                ValueType = value?.GetType().ToString();
                ValueJson = value?.ToJsonString();
            }
        }

        /// <summary>
        /// 获取强类型数据值
        /// </summary>
        public T GetValue<T>()
        {
            object value = Value;
            if (Equals(value, default(T)))
            {
                return default(T);
            }
            if (value is T)
            {
                return (T)value;
            }
            throw new OsharpException($"获取强类型字典值时传入类型“{typeof(T)}”与实际数据类型“{ValueType}”不匹配");
        }
    }
}