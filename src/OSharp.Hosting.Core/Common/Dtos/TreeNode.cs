// -----------------------------------------------------------------------
//  <copyright file="TreeNode.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace OSharp.Hosting.Common.Dtos
{
    /// <summary>
    /// 树形节点
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        /// 初始化一个<see cref="TreeNode"/>类型的新实例
        /// </summary>
        public TreeNode()
        {
            Items = new List<TreeNode>();
        }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 获取或设置 排序码
        /// </summary>
        public double OrderCode { get; set; }

        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 图标样式
        /// </summary>
        public string IconCls { get; set; }

        /// <summary>
        /// 获取或设置 URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置 是否有子项
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// 获取或设置 附加数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 获取或设置 子项集合
        /// </summary>
        public List<TreeNode> Items { get; }
    }
}