// -----------------------------------------------------------------------
//  <copyright file="ListFilterGroup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 21:24</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;

using OSharp.AspNetCore.Http;
using OSharp.Filter;
using OSharp.Json;


namespace OSharp.AspNetCore.UI
{
    /// <summary>
    /// 初始化一个<see cref="ListFilterGroup"/>类型的新实例
    /// </summary>
    public class ListFilterGroup : FilterGroup
    {
        /// <summary>
        /// 初始化一个<see cref="ListFilterGroup"/>类型的新实例
        /// </summary>
        public ListFilterGroup(HttpRequest request)
        {
            string jsonGroup = request.Params("filter_group");
            if (jsonGroup.IsNullOrEmpty())
            {
                return;
            }
            FilterGroup group = JsonHelper.FromJson<FilterGroup>(jsonGroup);
            Rules = group.Rules;
            Groups = group.Groups;
            Operate = group.Operate;
        }
    }
}