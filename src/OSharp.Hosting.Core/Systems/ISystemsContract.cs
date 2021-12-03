// -----------------------------------------------------------------------
//  <copyright file="ISystemsContract.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-08 13:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Hosting.Systems.Dtos;
using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.Systems
{
    /// <summary>
    /// 业务契约：系统模块
    /// </summary>
    public interface ISystemsContract
    {
        #region 菜单信息业务

        /// <summary>
        /// 获取 菜单信息查询数据集
        /// </summary>
        IQueryable<Menu> MenuInfos { get; }

        /// <summary>
        /// 检查菜单信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的菜单信息编号</param>
        /// <returns>菜单信息是否存在</returns>
        Task<bool> CheckMenuInfoExists(Expression<Func<Menu, bool>> predicate, int id = default);

        /// <summary>
        /// 添加菜单信息信息
        /// </summary>
        /// <param name="dtos">要添加的菜单信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> CreateMenuInfos(params MenuInputDto[] dtos);

        /// <summary>
        /// 更新菜单信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的菜单信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateMenuInfos(params MenuInputDto[] dtos);

        /// <summary>
        /// 删除菜单信息信息
        /// </summary>
        /// <param name="ids">要删除的菜单信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteMenuInfos(params int[] ids);

        #endregion

    }
}