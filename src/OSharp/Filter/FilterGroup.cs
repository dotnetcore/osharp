// -----------------------------------------------------------------------
//  <copyright file="FilterGroup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-07-26 15:20</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using OSharp.Properties;


namespace OSharp.Filter
{
    /// <summary>
    /// 筛选条件组
    /// </summary>
    public class FilterGroup
    {
        private FilterOperate _operate;

        #region 构造函数

        /// <summary>
        /// 初始化一个<see cref="FilterGroup"/>的新实例
        /// </summary>
        public FilterGroup()
            : this(FilterOperate.And)
        { }

        /// <summary>
        /// 初始化一个<see cref="FilterGroup"/>类型的新实例
        /// </summary>
        /// <param name="operateCode">条件间操作方式的前台码</param>
        public FilterGroup(string operateCode)
            : this(FilterHelper.GetFilterOperate(operateCode))
        { }

        /// <summary>
        /// 使用操作方式初始化一个<see cref="FilterGroup"/>的新实例
        /// </summary>
        /// <param name="operate">条件间操作方式</param>
        public FilterGroup(FilterOperate operate)
        {
            Operate = operate;
            Rules = new List<FilterRule>();
            Groups = new List<FilterGroup>();
        }

        #endregion

        /// <summary>
        /// 获取或设置 条件集合
        /// </summary>
        public ICollection<FilterRule> Rules { get; set; }

        /// <summary>
        /// 获取或设置 条件组集合
        /// </summary>
        public ICollection<FilterGroup> Groups { get; set; }

        /// <summary>
        /// 获取或设置 条件间操作方式，仅限And, Or
        /// </summary>
        public FilterOperate Operate
        {
            get { return _operate; }
            set
            {
                if (value != FilterOperate.And && value != FilterOperate.Or)
                {
                    throw new InvalidOperationException(Resources.Filter_GroupOperateError);
                }
                _operate = value;
            }
        }

        /// <summary>
        /// 添加规则
        /// </summary>
        public void AddRule(FilterRule rule)
        {
            if (Rules.All(m => !m.Equals(rule)))
            {
                Rules.Add(rule);
            }
        }
    }
}