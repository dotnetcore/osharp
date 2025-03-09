// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorizationManagerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-26 23:05</last-date>
// -----------------------------------------------------------------------


using Liuliu.Demo.Authorization.Dtos;
using Liuliu.Demo.Authorization.Entities;
using Liuliu.Demo.Identity.Entities;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Authorization;
using OSharp.Authorization.Dtos;
using OSharp.Authorization.Events;
using OSharp.Authorization.Functions;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Mapping;
using System.Linq.Expressions;

namespace Liuliu.Demo.Authorization;

/// <summary>
/// 功能权限管理器基类
/// </summary>
/// <typeparam name="Function">功能类型</typeparam>
/// <typeparam name="FunctionInputDto">功能输入DTO类型</typeparam>
/// <typeparam name="Module">模块类型</typeparam>
/// <typeparam name="ModuleInputDto">模块输入类型</typeparam>
/// <typeparam name="long">模块编号类型</typeparam>
/// <typeparam name="ModuleFunction">模块功能类型</typeparam>
/// <typeparam name="ModuleRole">模块角色类型</typeparam>
/// <typeparam name="ModuleUser">模块用户类型</typeparam>
/// <typeparam name="UserRole">用户角色类型</typeparam>
/// <typeparam name="long">用户角色编号类型</typeparam>
/// <typeparam name="Role">角色类型</typeparam>
/// <typeparam name="long">角色编号类型</typeparam>
/// <typeparam name="User">用户类型</typeparam>
/// <typeparam name="long">用户编号类型</typeparam>
/// <summary>
/// 功能权限管理器
/// </summary>
public class FunctionAuthManager
    : FunctionAuthorizationManagerBase<Function, FunctionInputDto, Module, ModuleInputDto, long, ModuleFunction, ModuleRole, ModuleUser, UserRole,
        long, Role, long, User, long>
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// 初始化一个 SecurityManager 类型的新实例
    /// </summary>
    /// <param name="provider">服务提供程序</param>
    public FunctionAuthManager(IServiceProvider provider)
        : base(provider)
    {
        _provider = provider;
    }

    #region 属性

    /// <summary>
    /// 获取 事件总线
    /// </summary>
    protected IEventBus EventBus => _provider.GetService<IEventBus>();

    /// <summary>
    /// 获取 功能仓储
    /// </summary>
    protected IRepository<Function, long> FunctionRepository => _provider.GetService<IRepository<Function, long>>();

    /// <summary>
    /// 获取 模块仓储
    /// </summary>
    protected IRepository<Module, long> ModuleRepository => _provider.GetService<IRepository<Module, long>>();

    /// <summary>
    /// 获取 模块功能仓储
    /// </summary>
    protected IRepository<ModuleFunction, long> ModuleFunctionRepository => _provider.GetService<IRepository<ModuleFunction, long>>();

    /// <summary>
    /// 获取 模块角色仓储
    /// </summary>
    protected IRepository<ModuleRole, long> ModuleRoleRepository => _provider.GetService<IRepository<ModuleRole, long>>();

    /// <summary>
    /// 获取 模块用户仓储
    /// </summary>
    protected IRepository<ModuleUser, long> ModuleUserRepository => _provider.GetService<IRepository<ModuleUser, long>>();

    /// <summary>
    /// 获取 用户角色仓储
    /// </summary>
    protected IRepository<UserRole, long> UserRoleRepository => _provider.GetService<IRepository<UserRole, long>>();

    /// <summary>
    /// 获取 角色仓储
    /// </summary>
    protected IRepository<Role, long> RoleRepository => _provider.GetService<IRepository<Role, long>>();

    /// <summary>
    /// 获取 用户仓储
    /// </summary>
    protected IRepository<User, long> UserRepository => _provider.GetService<IRepository<User, long>>();

    #endregion

    #region Implementation of IFunctionStore<Function,in FunctionInputDto>

    /// <summary>
    /// 获取 功能信息查询数据集
    /// </summary>
    public IQueryable<Function> Functions => FunctionRepository.QueryAsNoTracking();

    /// <summary>
    /// 检查功能信息是否存在
    /// </summary>
    /// <param name="predicate">检查谓语表达式</param>
    /// <param name="id">更新的功能信息编号</param>
    /// <returns>功能信息是否存在</returns>
    public virtual Task<bool> CheckFunctionExists(Expression<Func<Function, bool>> predicate, long id = default)
    {
        return FunctionRepository.CheckExistsAsync(predicate, id);
    }

    /// <summary>
    /// 更新功能信息
    /// </summary>
    /// <param name="dtos">包含更新信息的功能信息DTO信息</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> UpdateFunctions(params FunctionInputDto[] dtos)
    {
        Check2.Validate<FunctionInputDto, long>(dtos, nameof(dtos));

        OperationResult result = await FunctionRepository.UpdateAsync(dtos,
            (dto, entity) =>
            {
                if (dto.IsLocked && entity.Area == "Admin" && entity.Controller == "Function"
                    && (entity.Action == "Update" || entity.Action == "Read"))
                {
                    throw new OsharpException($"功能信息“{entity.Name}”不能锁定");
                }
                if (dto.AuditEntityEnabled && !dto.AuditOperationEnabled && !entity.AuditOperationEnabled && !entity.AuditEntityEnabled)
                {
                    dto.AuditOperationEnabled = true;
                }
                else if (!dto.AuditOperationEnabled && dto.AuditEntityEnabled && entity.AuditOperationEnabled && entity.AuditEntityEnabled)
                {
                    dto.AuditEntityEnabled = false;
                }
                if (dto.AccessType != entity.AccessType)
                {
                    entity.IsAccessTypeChanged = true;
                }
                return Task.FromResult(0);
            });
        if (result.Succeeded)
        {
            //功能信息缓存刷新事件
            FunctionCacheRefreshEventData clearEventData = new FunctionCacheRefreshEventData();
            await EventBus.PublishAsync(clearEventData);

            //功能权限缓存刷新事件
            FunctionAuthCacheRefreshEventData removeEventData = new FunctionAuthCacheRefreshEventData()
            {
                FunctionIds = dtos.Select(m => m.Id).ToArray()
            };
            await EventBus.PublishAsync(removeEventData);
        }
        return result;
    }

    #endregion Implementation of IFunctionStore<Function,in FunctionInputDto>

    #region Implementation of IModuleStore<Module,in ModuleInputDto,in long>

    /// <summary>
    /// 获取 模块信息查询数据集
    /// </summary>
    public IQueryable<Module> Modules => ModuleRepository.QueryAsNoTracking();

    /// <summary>
    /// 检查模块信息是否存在
    /// </summary>
    /// <param name="predicate">检查谓语表达式</param>
    /// <param name="id">更新的模块信息编号</param>
    /// <returns>模块信息是否存在</returns>
    public virtual Task<bool> CheckModuleExists(Expression<Func<Module, bool>> predicate, long id = default)
    {
        return ModuleRepository.CheckExistsAsync(predicate, id);
    }

    /// <summary>
    /// 添加模块信息
    /// </summary>
    /// <param name="dto">要添加的模块信息DTO信息</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> CreateModule(ModuleInputDto dto)
    {
        const string treePathItemFormat = "${0}$";
        Check.NotNull(dto, nameof(dto));
        if (dto.Name.Contains('.'))
        {
            return new OperationResult(OperationResultType.Error, $"模块名称“{dto.Name}”不能包含字符“-”");
        }
        var exist = Modules.Where(m => m.Name == dto.Name && m.ParentId != null && m.ParentId.Equals(dto.ParentId))
            .SelectMany(m => Modules.Where(n => n.Id.Equals(m.ParentId)).Select(n => n.Name)).FirstOrDefault();
        if (exist != null)
        {
            return new OperationResult(OperationResultType.Error, $"模块“{exist}”中已存在名称为“{dto.Name}”的子模块，不能重复添加。");
        }
        exist = Modules.Where(m => m.Code == dto.Code && m.ParentId != null && m.ParentId.Equals(dto.ParentId))
            .SelectMany(m => Modules.Where(n => n.Id.Equals(m.ParentId)).Select(n => n.Name)).FirstOrDefault();
        if (exist != null)
        {
            return new OperationResult(OperationResultType.Error, $"模块“{exist}”中已存在代码为“{dto.Code}”的子模块，不能重复添加。");
        }

        Module entity = dto.MapTo<Module>();
        //排序码，不存在为1，否则同级最大+1
        var peerModules = Modules.Where(m => m.ParentId.Equals(dto.ParentId)).Select(m => new { m.OrderCode }).ToArray();
        if (peerModules.Length == 0)
        {
            entity.OrderCode = 1;
        }
        else
        {
            double maxCode = peerModules.Max(m => m.OrderCode);
            entity.OrderCode = maxCode + 1;
        }
        //父模块
        string parentTreePathString = null;
        if (!Equals(dto.ParentId, default(long)))
        {
            var parent = Modules.Where(m => m.Id.Equals(dto.ParentId)).Select(m => new { m.Id, m.TreePathString }).FirstOrDefault();
            if (parent == null)
            {
                return new OperationResult(OperationResultType.Error, $"编号为“{dto.ParentId}”的父模块信息不存在");
            }
            entity.ParentId = dto.ParentId;
            parentTreePathString = parent.TreePathString;
        }
        else
        {
            entity.ParentId = null;
        }
        if (await ModuleRepository.InsertAsync(entity) > 0)
        {
            entity.TreePathString = entity.ParentId == null
                ? treePathItemFormat.FormatWith(entity.Id)
                : GetModuleTreePath(entity.Id, parentTreePathString, treePathItemFormat);
            await ModuleRepository.UpdateAsync(entity);
            return new OperationResult(OperationResultType.Success, $"模块“{dto.Name}”创建成功");
        }
        return OperationResult.NoChanged;
    }

    /// <summary>
    /// 更新模块信息
    /// </summary>
    /// <param name="dto">包含更新信息的模块信息DTO信息</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> UpdateModule(ModuleInputDto dto)
    {
        const string treePathItemFormat = "${0}$";
        Check.NotNull(dto, nameof(dto));
        if (dto.Name.Contains('.'))
        {
            return new OperationResult(OperationResultType.Error, $"模块名称“{dto.Name}”不能包含字符“-”");
        }
        var exist1 = Modules.Where(m => m.Name == dto.Name && m.ParentId != null && m.ParentId.Equals(dto.ParentId) && !m.Id.Equals(dto.Id))
            .SelectMany(m => Modules.Where(n => n.Id.Equals(m.ParentId)).Select(n => new { n.Id, n.Name })).FirstOrDefault();
        if (exist1 != null)
        {
            return new OperationResult(OperationResultType.Error, $"模块“{exist1.Name}”中已存在名称为“{dto.Name}”的子模块，不能重复添加。");
        }
        var exist2 = Modules.Where(m => m.Code == dto.Code && m.ParentId != null && m.ParentId.Equals(dto.ParentId) && !m.Id.Equals(dto.Id))
            .SelectMany(m => Modules.Where(n => n.Id.Equals(m.ParentId)).Select(n => new { n.Id, n.Name })).FirstOrDefault();
        if (exist2 != null)
        {
            return new OperationResult(OperationResultType.Error, $"模块“{exist2.Name}”中已存在代码为“{dto.Code}”的子模块，不能重复添加。");
        }
        Module entity = await ModuleRepository.GetAsync(dto.Id);
        if (entity == null)
        {
            return new OperationResult(OperationResultType.Error, $"编号为“{dto.Id}”的模块信息不存在。");
        }
        entity = dto.MapTo(entity);
        if (!Equals(dto.ParentId, default(long)))
        {
            if (!entity.ParentId.Equals(dto.ParentId))
            {
                var parent = Modules.Where(m => m.Id.Equals(dto.ParentId)).Select(m => new { m.Id, m.TreePathString }).FirstOrDefault();
                if (parent == null)
                {
                    return new OperationResult(OperationResultType.Error, $"编号为“{dto.ParentId}”的父模块信息不存在");
                }
                entity.ParentId = dto.ParentId;
                entity.TreePathString = GetModuleTreePath(entity.Id, parent.TreePathString, treePathItemFormat);
            }
        }
        else
        {
            entity.ParentId = null;
            entity.TreePathString = treePathItemFormat.FormatWith(entity.Id);
        }
        return await ModuleRepository.UpdateAsync(entity) > 0
            ? new OperationResult(OperationResultType.Success, $"模块“{dto.Name}”更新成功")
            : OperationResult.NoChanged;
    }

    /// <summary>
    /// 删除模块信息
    /// </summary>
    /// <param name="id">要删除的模块信息编号</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> DeleteModule(long id)
    {
        Module entity = await ModuleRepository.GetAsync(id);
        if (entity == null)
        {
            return OperationResult.Success;
        }
        if (await ModuleRepository.CheckExistsAsync(m => m.ParentId.Equals(id)))
        {
            return new OperationResult(OperationResultType.Error, $"模块“{entity.Name}”的子模块不为空，不能删除");
        }
        //清除附属引用
        await ModuleFunctionRepository.DeleteBatchAsync(m => m.ModuleId.Equals(id));
        await ModuleRoleRepository.DeleteBatchAsync(m => m.ModuleId.Equals(id));
        await ModuleUserRepository.DeleteBatchAsync(m => m.ModuleId.Equals(id));

        OperationResult result = await ModuleRepository.DeleteAsync(entity) > 0
            ? new OperationResult(OperationResultType.Success, $"模块“{entity.Name}”删除成功")
            : OperationResult.NoChanged;
        if (result.Succeeded)
        {
            //功能权限缓存刷新事件
            long[] functionIds = ModuleFunctionRepository.QueryAsNoTracking(m => m.ModuleId.Equals(id)).Select(m => m.FunctionId).ToArray();
            FunctionAuthCacheRefreshEventData removeEventData = new FunctionAuthCacheRefreshEventData() { FunctionIds = functionIds };
            await EventBus.PublishAsync(removeEventData);
        }
        return result;
    }

    /// <summary>
    /// 获取树节点及其子节点的所有模块编号
    /// </summary>
    /// <param name="rootIds">树节点</param>
    /// <returns>模块编号集合</returns>
    public virtual long[] GetModuleTreeIds(params long[] rootIds)
    {
        return rootIds.SelectMany(m => ModuleRepository.QueryAsNoTracking(n => n.TreePathString.Contains($"${m}$")).Select(n => n.Id)).Distinct()
            .ToArray();
    }

    private static string GetModuleTreePath(long currentId, string parentTreePath, string treePathItemFormat)
    {
        return $"{parentTreePath},{treePathItemFormat.FormatWith(currentId)}";
    }

    #endregion Implementation of IModuleStore<Module,in ModuleInputDto,in long>

    #region Implementation of IModuleFunctionStore<ModuleFunction>

    /// <summary>
    /// 获取 模块功能信息查询数据集
    /// </summary>
    public IQueryable<ModuleFunction> ModuleFunctions => ModuleFunctionRepository.QueryAsNoTracking();

    /// <summary>
    /// 检查模块功能信息是否存在
    /// </summary>
    /// <param name="predicate">检查谓语表达式</param>
    /// <param name="id">更新的模块功能信息编号</param>
    /// <returns>模块功能信息是否存在</returns>
    public virtual Task<bool> CheckModuleFunctionExists(Expression<Func<ModuleFunction, bool>> predicate, long id = default)
    {
        return ModuleFunctionRepository.CheckExistsAsync(predicate, id);
    }

    /// <summary>
    /// 设置模块的功能信息
    /// </summary>
    /// <param name="moduleId">模块编号</param>
    /// <param name="functionIds">要设置的功能编号</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> SetModuleFunctions(long moduleId, long[] functionIds)
    {
        Module module = await ModuleRepository.GetAsync(moduleId);
        if (module == null)
        {
            return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
        }

        long[] existFunctionIds = ModuleFunctionRepository.QueryAsNoTracking(m => m.ModuleId.Equals(moduleId)).Select(m => m.FunctionId).ToArray();
        long[] addFunctionIds = functionIds.Except(existFunctionIds).ToArray();
        long[] removeFunctionIds = existFunctionIds.Except(functionIds).ToArray();
        List<string> addNames = new List<string>(), removeNames = new List<string>();
        int count = 0;

        foreach (long functionId in addFunctionIds)
        {
            Function function = await FunctionRepository.GetAsync(functionId);
            if (function == null)
            {
                continue;
            }
            ModuleFunction moduleFunction = new ModuleFunction() { ModuleId = moduleId, FunctionId = functionId };
            count = count + await ModuleFunctionRepository.InsertAsync(moduleFunction);
            addNames.Add(function.Name);
        }
        foreach (long functionId in removeFunctionIds)
        {
            Function function = await FunctionRepository.GetAsync(functionId);
            if (function == null)
            {
                continue;
            }
            ModuleFunction moduleFunction = ModuleFunctionRepository.QueryAsNoTracking()
                .FirstOrDefault(m => m.ModuleId.Equals(moduleId) && m.FunctionId == functionId);
            if (moduleFunction == null)
            {
                continue;
            }
            count = count + await ModuleFunctionRepository.DeleteAsync(moduleFunction);
            removeNames.Add(function.Name);
        }

        if (count > 0)
        {
            //功能权限缓存刷新事件
            FunctionAuthCacheRefreshEventData removeEventData = new FunctionAuthCacheRefreshEventData()
            {
                FunctionIds = addFunctionIds.Union(removeFunctionIds).Distinct().ToArray()
            };
            await EventBus.PublishAsync(removeEventData);

            return new OperationResult(OperationResultType.Success,
                $"模块“{module.Name}”添加功能“{addNames.ExpandAndToString()}”，移除功能“{removeNames.ExpandAndToString()}”操作成功");
        }
        return OperationResult.NoChanged;
    }

    #endregion Implementation of IModuleFunctionStore<ModuleFunction>

    #region Implementation of IModuleRoleStore<ModuleRole>

    /// <summary>
    /// 获取 模块角色信息查询数据集
    /// </summary>
    public IQueryable<ModuleRole> ModuleRoles => ModuleRoleRepository.QueryAsNoTracking();

    /// <summary>
    /// 检查模块角色信息是否存在
    /// </summary>
    /// <param name="predicate">检查谓语表达式</param>
    /// <param name="id">更新的模块角色信息编号</param>
    /// <returns>模块角色信息是否存在</returns>
    public virtual Task<bool> CheckModuleRoleExists(Expression<Func<ModuleRole, bool>> predicate, long id = default)
    {
        return ModuleRoleRepository.CheckExistsAsync(predicate, id);
    }

    /// <summary>
    /// 设置角色的可访问模块
    /// </summary>
    /// <param name="roleId">角色编号</param>
    /// <param name="moduleIds">要赋予的模块编号集合</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> SetRoleModules(long roleId, long[] moduleIds)
    {
        Role role = await RoleRepository.GetAsync(roleId);
        if (role == null)
        {
            return new OperationResult(OperationResultType.QueryNull, $"编号为“{roleId}”的角色信息不存在");
        }

        long[] existModuleIds = ModuleRoleRepository.QueryAsNoTracking(m => m.RoleId.Equals(roleId)).Select(m => m.ModuleId).ToArray();
        long[] addModuleIds = moduleIds.Except(existModuleIds).ToArray();
        long[] removeModuleIds = existModuleIds.Except(moduleIds).ToArray();
        List<string> addNames = new List<string>(), removeNames = new List<string>();
        int count = 0;

        foreach (long moduleId in addModuleIds)
        {
            Module module = await ModuleRepository.GetAsync(moduleId);
            if (module == null)
            {
                return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
            }
            ModuleRole moduleRole = new ModuleRole() { ModuleId = moduleId, RoleId = roleId };
            count = count + await ModuleRoleRepository.InsertAsync(moduleRole);
            addNames.Add(module.Name);
        }
        foreach (long moduleId in removeModuleIds)
        {
            Module module = await ModuleRepository.GetAsync(moduleId);
            if (module == null)
            {
                return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
            }
            ModuleRole moduleRole = ModuleRoleRepository.GetFirst(m => m.RoleId.Equals(roleId) && m.ModuleId.Equals(moduleId));
            if (moduleRole == null)
            {
                continue;
            }
            count = count + await ModuleRoleRepository.DeleteAsync(moduleRole);
            removeNames.Add(module.Name);
        }

        if (count > 0)
        {
            //功能权限缓存刷新事件
            moduleIds = addModuleIds.Union(removeModuleIds).Distinct().ToArray();
            long[] functionIds = ModuleFunctionRepository.QueryAsNoTracking(m => moduleIds.Contains(m.ModuleId))
                .Select(m => m.FunctionId).Distinct().ToArray();
            FunctionAuthCacheRefreshEventData removeEventData = new FunctionAuthCacheRefreshEventData() { FunctionIds = functionIds };
            await EventBus.PublishAsync(removeEventData);

            if (addNames.Count > 0 && removeNames.Count == 0)
            {
                return new OperationResult(OperationResultType.Success, $"角色“{role.Name}”添加模块“{addNames.ExpandAndToString()}”操作成功");
            }
            if (addNames.Count == 0 && removeNames.Count > 0)
            {
                return new OperationResult(OperationResultType.Success, $"角色“{role.Name}”移除模块“{removeNames.ExpandAndToString()}”操作成功");
            }
            return new OperationResult(OperationResultType.Success,
                $"角色“{role.Name}”添加模块“{addNames.ExpandAndToString()}”，移除模块“{removeNames.ExpandAndToString()}”操作成功");
        }
        return OperationResult.NoChanged;
    }

    /// <summary>
    /// 获取角色可访问模块编号
    /// </summary>
    /// <param name="roleId">角色编号</param>
    /// <returns>模块编号集合</returns>
    public virtual long[] GetRoleModuleIds(long roleId)
    {
        long[] moduleIds = ModuleRoleRepository.QueryAsNoTracking(m => m.RoleId.Equals(roleId)).Select(m => m.ModuleId).Distinct().ToArray();
        return GetModuleTreeIds(moduleIds);
    }

    #endregion Implementation of IModuleRoleStore<ModuleRole>

    #region Implementation of IModuleUserStore<ModuleUser>

    /// <summary>
    /// 获取 模块用户信息查询数据集
    /// </summary>
    public IQueryable<ModuleUser> ModuleUsers => ModuleUserRepository.QueryAsNoTracking();

    /// <summary>
    /// 检查模块用户信息是否存在
    /// </summary>
    /// <param name="predicate">检查谓语表达式</param>
    /// <param name="id">更新的模块用户信息编号</param>
    /// <returns>模块用户信息是否存在</returns>
    public virtual Task<bool> CheckModuleUserExists(Expression<Func<ModuleUser, bool>> predicate, long id = default)
    {
        return ModuleUserRepository.CheckExistsAsync(predicate, id);
    }

    /// <summary>
    /// 设置用户的可访问模块
    /// </summary>
    /// <param name="userId">用户编号</param>
    /// <param name="moduleIds">要赋给用户的模块编号集合</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> SeUserModules(long userId, long[] moduleIds)
    {
        User user = await UserRepository.GetAsync(userId);
        if (user == null)
        {
            return new OperationResult(OperationResultType.QueryNull, $"编号为“{userId}”的用户信息不存在");
        }

        long[] existModuleIds = ModuleUserRepository.QueryAsNoTracking(m => m.UserId.Equals(userId)).Select(m => m.ModuleId).ToArray();
        long[] addModuleIds = moduleIds.Except(existModuleIds).ToArray();
        long[] removeModuleIds = existModuleIds.Except(moduleIds).ToArray();
        List<string> addNames = new List<string>(), removeNames = new List<string>();
        int count = 0;

        foreach (long moduleId in addModuleIds)
        {
            Module module = await ModuleRepository.GetAsync(moduleId);
            if (module == null)
            {
                return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
            }
            ModuleUser moduleUser = new ModuleUser() { ModuleId = moduleId, UserId = userId };
            count += await ModuleUserRepository.InsertAsync(moduleUser);
            addNames.Add(module.Name);
        }
        foreach (long moduleId in removeModuleIds)
        {
            Module module = await ModuleRepository.GetAsync(moduleId);
            if (module == null)
            {
                return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
            }
            ModuleUser moduleUser = ModuleUserRepository.GetFirst(m => m.ModuleId.Equals(moduleId) && m.UserId.Equals(userId));
            if (moduleUser == null)
            {
                continue;
            }
            count += await ModuleUserRepository.DeleteAsync(moduleUser);
            removeNames.Add(module.Name);
        }
        if (count > 0)
        {
            //功能权限缓存刷新事件
            FunctionAuthCacheRefreshEventData removeEventData = new FunctionAuthCacheRefreshEventData() { UserNames = new[] { user.UserName } };
            await EventBus.PublishAsync(removeEventData);

            if (addNames.Count > 0 && removeNames.Count == 0)
            {
                return new OperationResult(OperationResultType.Success, $"用户“{user.UserName}”添加模块“{addNames.ExpandAndToString()}”操作成功");
            }
            if (addNames.Count == 0 && removeNames.Count > 0)
            {
                return new OperationResult(OperationResultType.Success, $"用户“{user.UserName}”移除模块“{removeNames.ExpandAndToString()}”操作成功");
            }
            return new OperationResult(OperationResultType.Success,
                $"用户“{user.UserName}”添加模块“{addNames.ExpandAndToString()}”，移除模块“{removeNames.ExpandAndToString()}”操作成功");
        }
        return OperationResult.NoChanged;
    }

    /// <summary>
    /// 获取用户自己的可访问模块编号
    /// </summary>
    /// <param name="userId">用户编号</param>
    /// <returns>模块编号集合</returns>
    public virtual long[] GeUserSelfModuleIds(long userId)
    {
        long[] moduleIds = ModuleUserRepository.QueryAsNoTracking(m => m.UserId.Equals(userId)).Select(m => m.ModuleId).Distinct().ToArray();
        return GetModuleTreeIds(moduleIds);
    }

    /// <summary>
    /// 获取用户及其拥有角色可访问模块编号
    /// </summary>
    /// <param name="userId">用户编号</param>
    /// <returns>模块编号集合</returns>
    public virtual long[] GeUserWithRoleModuleIds(long userId)
    {
        long[] selfModuleIds = GeUserSelfModuleIds(userId);

        long[] roleIds = UserRoleRepository.QueryAsNoTracking(m => m.UserId.Equals(userId)).Select(m => m.RoleId).ToArray();
        long[] roleModuleIds = roleIds
            .SelectMany(m => ModuleRoleRepository.QueryAsNoTracking(n => n.RoleId.Equals(m)).Select(n => n.ModuleId))
            .Distinct().ToArray();
        roleModuleIds = GetModuleTreeIds(roleModuleIds);

        return roleModuleIds.Union(selfModuleIds).Distinct().ToArray();
    }

    #endregion Implementation of IModuleUserStore<ModuleUser>
}