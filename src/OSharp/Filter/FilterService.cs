// -----------------------------------------------------------------------
//  <copyright file="FilterService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-20 0:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Security.Claims;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Identity;
using OSharp.Linq;
using OSharp.Reflection;


namespace OSharp.Filter
{
    /// <summary>
    /// 查询表达式服务
    /// </summary>
    public class FilterService : IFilterService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="FilterService"/>类型的新实例
        /// </summary>
        public FilterService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Implementation of IFilterService

        /// <summary>
        /// 获取指定查询条件组的查询表达式
        /// </summary>
        /// <typeparam name="T">表达式实体类型</typeparam>
        /// <param name="group">查询条件组，如果为null，则直接返回 true 表达式</param>
        /// <returns>查询表达式</returns>
        public virtual Expression<Func<T, bool>> GetExpression<T>(FilterGroup group)
        {
            return FilterHelper.GetExpression<T>(group);

        }

        /// <summary>
        /// 获取指定查询条件的查询表达式
        /// </summary>
        /// <typeparam name="T">表达式实体类型</typeparam>
        /// <param name="rule">查询条件，如果为null，则直接返回 true 表达式</param>
        /// <returns>查询表达式</returns>
        public virtual Expression<Func<T, bool>> GetExpression<T>(FilterRule rule)
        {
            return FilterHelper.GetExpression<T>(rule);
        }

        /// <summary>
        /// 获取指定查询条件组的查询表达式，并综合数据权限
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="group">传入的查询条件组，为空时则只返回数据权限过滤器</param>
        /// <param name="operation">数据权限操作</param>
        /// <returns>综合之后的表达式</returns>
        public virtual Expression<Func<T, bool>> GetDataFilterExpression<T>(FilterGroup group = null,
            DataAuthOperation operation = DataAuthOperation.Read)
        {
            Expression<Func<T, bool>> exp = m => true;
            if (group != null)
            {
                exp = GetExpression<T>(group);
            }
            //从缓存中查找当前用户的角色与实体T的过滤条件
            ClaimsPrincipal user = _serviceProvider.GetCurrentUser();
            if (user == null)
            {
                return exp;
            }

            IDataAuthCache dataAuthCache = _serviceProvider.GetService<IDataAuthCache>();
            if (dataAuthCache == null)
            {
                return exp;
            }

            // 要判断数据权限功能,先要排除没有执行当前功能权限的角色,判断剩余角色的数据权限
            string[] roleNames = user.Identity.GetRoles();
            ScopedDictionary scopedDict = _serviceProvider.GetService<ScopedDictionary>();
            if (scopedDict?.Function != null)
            {
                roleNames = scopedDict.DataAuthValidRoleNames;
            }
            string typeName = typeof(T).GetFullNameWithModule();
            Expression<Func<T, bool>> subExp = null;
            foreach (string roleName in roleNames)
            {
                FilterGroup subGroup = dataAuthCache.GetFilterGroup(roleName, typeName, operation);
                if (subGroup == null)
                {
                    continue;
                }
                subExp = subExp == null ? GetExpression<T>(subGroup) : subExp.Or(GetExpression<T>(subGroup));
            }
            if (subExp != null)
            {
                if (group == null)
                {
                    return subExp;
                }
                exp = subExp.And(exp);
            }

            return exp;
        }

        /// <summary>
        /// 验证指定查询条件组是否能正常转换为表达式
        /// </summary>
        /// <param name="group">查询条件组</param>
        /// <param name="type">实体类型</param>
        /// <returns>验证操作结果</returns>
        public virtual OperationResult CheckFilterGroup(FilterGroup group, Type type)
        {
            return FilterHelper.CheckFilterGroup(group, type);
        }

        #endregion
    }
}