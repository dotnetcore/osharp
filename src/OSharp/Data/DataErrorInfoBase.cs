// -----------------------------------------------------------------------
//  <copyright file="DataErrorInfoBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-20 11:21</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;


namespace OSharp.Data
{
    public abstract class DataErrorInfoBase : IDataErrorInfo
    {
        /// <summary>获取一条错误消息，指示此对象有什么问题。</summary>
        /// <returns>指示此对象存在什么问题的错误消息。默认值为空字符串（""）。</returns>
        public abstract string Error { get; }

        /// <summary>获取具有给定名称的属性的错误消息。</summary>
        /// <param name="columnName">要获取错误消息的属性的名称。</param>
        /// <returns>该属性的错误消息。默认值为空字符串 ("").</returns>
        public virtual string this[string columnName]
        {
            get
            {
                ValidationContext context = new ValidationContext(this, null, null) { MemberName = columnName };
                List<ValidationResult> results = new List<ValidationResult>();
                PropertyInfo property = GetType().GetProperty(columnName);
                if (property == null)
                {
                    return string.Empty;
                }
                Validator.TryValidateProperty(property.GetValue(this, null), context, results);
                if (results.Count > 0)
                {
                    return string.Join(Environment.NewLine, results.Select(m => m.ErrorMessage).ToArray());
                }
                return string.Empty;
            }
        }
    }
}