// -----------------------------------------------------------------------
//  <copyright file="PageRequest.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 21:20</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.AspNetCore.Http;

using OSharp.AspNetCore.Http;
using OSharp.Filter;
using OSharp.Json;


namespace OSharp.AspNetCore.UI
{
    /// <summary>
    /// 分页查询请求
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// 初始化一个<see cref="PageRequest"/>类型的新实例
        /// </summary>
        public PageRequest(HttpRequest request)
        {
            string jsonGroup = request.Params("filter_group");
            FilterGroup = !jsonGroup.IsNullOrEmpty() ? JsonHelper.FromJson<FilterGroup>(jsonGroup) : new FilterGroup();

            int pageIndex = request.Params("pageIndex").CastTo(1);
            int pageSize = request.Params("pageSize").CastTo(20);
            PageCondition = new PageCondition(pageIndex, pageSize);
            string sortField = request.Params("sortField");
            string sortOrder = request.Params("sortOrder");
            if (!sortField.IsNullOrEmpty() && !sortOrder.IsNullOrEmpty())
            {
                string[] files = sortField.Split(",", true);
                string[] orders = sortOrder.Split(",", true);
                if (files.Length != orders.Length)
                {
                    throw new ArgumentException("查询列表的排序列名与方向参数个数不一致");
                }
                List<SortCondition> sortConditions = new List<SortCondition>();
                for (int i = 0; i < files.Length; i++)
                {
                    ListSortDirection direction = orders[i].ToLower() == "desc" ? ListSortDirection.Descending : ListSortDirection.Ascending;
                    sortConditions.Add(new SortCondition(files[i], direction));
                }
                PageCondition.SortConditions = sortConditions.ToArray();
            }
            else
            {
                PageCondition.SortConditions = new SortCondition[0];
            }
        }

        /// <summary>
        /// 获取 查询条件组
        /// </summary>
        public FilterGroup FilterGroup { get; }

        /// <summary>
        /// 获取 分页查询条件信息
        /// </summary>
        public PageCondition PageCondition { get; }

        /// <summary>
        /// 添加默认排序条件，只有排序条件为空时有效
        /// </summary>
        public void AddDefaultSortCondition(params SortCondition[] conditions)
        {
            Check.NotNullOrEmpty(conditions, nameof(conditions));

            if (PageCondition.SortConditions.Length == 0)
            {
                PageCondition.SortConditions = conditions;
            }
        }
    }
}