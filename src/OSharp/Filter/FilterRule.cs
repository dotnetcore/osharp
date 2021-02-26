// -----------------------------------------------------------------------
//  <copyright file="FilterRule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-07-26 15:26</last-date>
// -----------------------------------------------------------------------

using System;

#if NETSTANDARD2_0
using Microsoft.DotNet.PlatformAbstractions;
#endif


namespace OSharp.Filter
{
    /// <summary>
    /// 筛选条件
    /// </summary>
    public class FilterRule
    {
        #region 构造函数

        /// <summary>
        /// 初始化一个<see cref="FilterRule"/>的新实例
        /// </summary>
        public FilterRule()
            : this(null, null)
        { }

        /// <summary>
        /// 使用指定数据名称，数据值初始化一个<see cref="FilterRule"/>的新实例
        /// </summary>
        /// <param name="field">数据名称</param>
        /// <param name="value">数据值</param>
        public FilterRule(string field, object value)
            : this(field, value, FilterOperate.Equal)
        { }

        /// <summary>
        /// 初始化一个<see cref="FilterRule"/>类型的新实例
        /// </summary>
        /// <param name="field">数据名称</param>
        /// <param name="value">数据值</param>
        /// <param name="operateCode">操作方式的前台码</param>
        public FilterRule(string field, object value, string operateCode)
            : this(field, value, FilterHelper.GetFilterOperate(operateCode))
        { }

        /// <summary>
        /// 使用指定数据名称，数据值及操作方式初始化一个<see cref="FilterRule"/>的新实例
        /// </summary>
        /// <param name="field">数据名称</param>
        /// <param name="value">数据值</param>
        /// <param name="operate">操作方式</param>
        public FilterRule(string field, object value, FilterOperate operate)
        {
            Field = field;
            Value = value;
            Operate = operate;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置 属性名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 获取或设置 属性值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 获取或设置 操作类型
        /// </summary>
        public FilterOperate Operate { get; set; }

        #endregion

        #region Overrides of Object

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is FilterRule rule))
            {
                return false;
            }
            return rule.Field == Field && rule.Value == Value && rule.Operate == Operate;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
#if NET5_0
            return HashCode.Combine(Field, Value, Operate);
#else
            var combiner = new HashCodeCombiner();
            combiner.Add(Field);
            combiner.Add(Value);
            combiner.Add(Operate);
            return combiner.CombinedHash;
#endif
        }

        #endregion
    }
}