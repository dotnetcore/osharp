// -----------------------------------------------------------------------
//  <copyright file="PasswordAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-08-29 15:21</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

using OSharp.Extensions;


namespace OSharp.DataAnnotations
{
    /// <summary>
    /// 确认一个密码数据类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PasswordAttribute : DataTypeAttribute
    {
        private string _value;

        /// <summary>
        /// 以最小长度为6、需要数字、不允许纯数字、需要小写字母、不需要大写字母 初始化 <see cref="T:System.ComponentModel.DataAnnotations.PasswordAttribute"/> 类的新实例。
        /// </summary>
        public PasswordAttribute()
            : base(DataType.Password)
        {
            RequiredLength = 6;
            RequiredDigit = true;
            CanOnlyDigit = false;
            RequiredLowercase = true;
            RequiredUppercase = false;
        }

        /// <summary>
        /// 获取或设置 密码最小长度
        /// </summary>
        public int RequiredLength { get; set; }

        /// <summary>
        /// 获取或设置 需要数字
        /// </summary>
        public bool RequiredDigit { get; set; }

        /// <summary>
        /// 获取或设置 是否允许纯数字
        /// </summary>
        public bool CanOnlyDigit { get; set; }

        /// <summary>
        /// 获取或设置 需要小字字母
        /// </summary>
        public bool RequiredLowercase { get; set; }

        /// <summary>
        /// 获取或设置 需要大小字母
        /// </summary>
        public bool RequiredUppercase { get; set; }

        #region Overrides of DataTypeAttribute

        /// <summary>
        /// 检查数据字段的值是否有效。
        /// </summary>
        /// <returns>
        /// 如果指定的值有效或 null，则为 true；否则，为 false。
        /// </returns>
        /// <param name="value">要验证的数据字段值。</param>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            string input = value as string;
            if (input == null)
            {
                return false;
            }
            _value = input;
            if (input.Length < RequiredLength)
            {
                return false;
            }
            if (RequiredDigit && !input.IsMatch(@"[0-9]"))
            {
                return false;
            }
            if (!CanOnlyDigit && input.IsMatch(@"^[0-9]+$"))
            {
                return false;
            }
            if (RequiredLowercase && !input.IsMatch(@"[a-z]"))
            {
                return false;
            }
            return !RequiredUppercase || input.IsMatch(@"[A-Z]");
        }

        /// <summary>
        /// 基于发生错误的数据字段对错误消息应用格式设置。
        /// </summary>
        /// <returns>
        /// 带有格式的错误消息的实例。
        /// </returns>
        /// <param name="name">要包括在带有格式的消息中的名称。</param>
        public override string FormatErrorMessage(string name)
        {
            name.CheckNotNullOrEmpty("name" );
            if (_value.Length < RequiredLength)
            {
                return "{0} 长度必须大于{1}位".FormatWith(name, RequiredLength);
            }
            if (RequiredDigit && !_value.IsMatch(@"[0-9]"))
            {
                return "{0} 必须包含数字".FormatWith(name);
            }
            if (!CanOnlyDigit && _value.IsMatch(@"^[0-9]+$"))
            {
                return "{0} 不允许是全是数字";
            }
            if (RequiredLowercase && !_value.IsMatch(@"[a-z]"))
            {
                return "{0} 必须包含小写字母".FormatWith(name);
            }
            if (RequiredUppercase && !_value.IsMatch(@"[A-Z]"))
            {
                return "{0} 必须包含大写字母".FormatWith(name);
            }
            return base.FormatErrorMessage(name);
        }

        #endregion
    }
}