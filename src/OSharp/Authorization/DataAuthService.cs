// -----------------------------------------------------------------------
//  <copyright file="DataAuthService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-17 12:46</last-date>
// -----------------------------------------------------------------------


namespace OSharp.Authorization;

/// <summary>
/// 数据权限服务实现
/// </summary>
public class DataAuthService : IDataAuthService
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// 初始化一个<see cref="DataAuthService"/>类型的新实例
    /// </summary>
    public DataAuthService(IServiceProvider provider)
    {
        _provider = provider;
    }
        
    /// <summary>
    /// 获取 数据权限过滤服务
    /// </summary>
    protected IFilterService FilterService => _provider.GetRequiredService<IFilterService>();

    /// <summary>
    /// 获取 当前用户
    /// </summary>
    protected ClaimsPrincipal CurrentUser => _provider.GetCurrentUser();

    /// <summary>
    /// 获取 数据权限缓存
    /// </summary>
    protected IDataAuthCache DataAuthCache => _provider.GetService<IDataAuthCache>();

    /// <summary>
    /// 获取 数据字典
    /// </summary>
    protected ScopedDictionary ScopedDictionary => _provider.GetService<ScopedDictionary>();

    /// <summary>
    /// 获取指定实体的数据权限过滤表达式
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="operation">数据权限操作</param>
    /// <param name="group">传入的查询条件组，为空时则只返回数据权限过滤器</param>
    /// <returns>实体的数据权限过滤表达式</returns>
    public Expression<Func<T, bool>> GetDataFilter<T>(DataAuthOperation operation, FilterGroup group = null)
    {
        Expression<Func<T, bool>> exp = m => true;
        if (group != null)
        {
            exp = FilterService.GetExpression<T>(group);
        }
        //从缓存中查找当前用户的角色与实体T的过滤条件
        ClaimsPrincipal user = CurrentUser;
        if (user == null)
        {
            return exp;
        }

        IDataAuthCache dataAuthCache = DataAuthCache;
        if (dataAuthCache == null)
        {
            return exp;
        }

        // 要判断数据权限功能,先要排除没有执行当前功能权限的角色,判断剩余角色的数据权限
        ScopedDictionary scopedDict = ScopedDictionary;
        string[] roleNames = user.Identity.GetRoles();
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
            subExp = subExp == null ? FilterService.GetExpression<T>(subGroup) : subExp.Or(FilterService.GetExpression<T>(subGroup));
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
    /// 检查指定操作的数据权限，验证要操作的数据是否符合特定的验证委托
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="operation">数据权限操作</param>
    /// <param name="entities">待检测的实体数据</param>
    /// <returns>是否有权限</returns>
    public bool CheckDataAuth<TEntity>(DataAuthOperation operation, params TEntity[] entities)
    {
        if (entities.Length == 0)
        {
            return true;
        }

        Expression<Func<TEntity, bool>> exp = GetDataFilter<TEntity>(operation);
        Func<TEntity, bool> func = exp.Compile();
        bool has = entities.All(func);
        return has;
    }
}