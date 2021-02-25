// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorizationManagerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-26 23:05</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization.Dtos;
using OSharp.Authorization.Entities;
using OSharp.Authorization.Events;
using OSharp.Authorization.Functions;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Identity.Entities;
using OSharp.Mapping;


namespace OSharp.Authorization
{
    /// <summary>
    /// 功能权限管理器基类
    /// </summary>
    /// <typeparam name="TFunction">功能类型</typeparam>
    /// <typeparam name="TFunctionInputDto">功能输入DTO类型</typeparam>
    /// <typeparam name="TModule">模块类型</typeparam>
    /// <typeparam name="TModuleInputDto">模块输入类型</typeparam>
    /// <typeparam name="TModuleKey">模块编号类型</typeparam>
    /// <typeparam name="TModuleFunction">模块功能类型</typeparam>
    /// <typeparam name="TModuleRole">模块角色类型</typeparam>
    /// <typeparam name="TModuleUser">模块用户类型</typeparam>
    /// <typeparam name="TUserRole">用户角色类型</typeparam>
    /// <typeparam name="TUserRoleKey">用户角色编号类型</typeparam>
    /// <typeparam name="TRole">角色类型</typeparam>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    /// <typeparam name="TUser">用户类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    public abstract class FunctionAuthorizationManagerBase<TFunction, TFunctionInputDto, TModule, TModuleInputDto, TModuleKey,
            TModuleFunction, TModuleRole, TModuleUser, TUserRole, TUserRoleKey, TRole, TRoleKey, TUser, TUserKey>
        : IFunctionStore<TFunction, TFunctionInputDto>,
          IModuleStore<TModule, TModuleInputDto, TModuleKey>,
          IModuleFunctionStore<TModuleFunction, TModuleKey>,
          IModuleRoleStore<TModuleRole, TRoleKey, TModuleKey>,
          IModuleUserStore<TModuleUser, TUserKey, TModuleKey>
        where TFunction : IFunction
        where TFunctionInputDto : FunctionInputDtoBase
        where TModule : ModuleBase<TModuleKey>
        where TModuleInputDto : ModuleInputDtoBase<TModuleKey>
        where TModuleFunction : ModuleFunctionBase<TModuleKey>, new()
        where TModuleRole : ModuleRoleBase<TModuleKey, TRoleKey>, new()
        where TModuleUser : ModuleUserBase<TModuleKey, TUserKey>, new()
        where TModuleKey : struct, IEquatable<TModuleKey>
        where TUserRole : UserRoleBase<TUserRoleKey, TUserKey, TRoleKey>
        where TUserRoleKey : IEquatable<TUserRoleKey>
        where TRole : RoleBase<TRoleKey>
        where TUser : UserBase<TUserKey>
        where TRoleKey : IEquatable<TRoleKey>
        where TUserKey : IEquatable<TUserKey>
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个 SecurityManager 类型的新实例
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        protected FunctionAuthorizationManagerBase(IServiceProvider provider)
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
        protected IRepository<TFunction, Guid> FunctionRepository => _provider.GetService<IRepository<TFunction, Guid>>();

        /// <summary>
        /// 获取 模块仓储
        /// </summary>
        protected IRepository<TModule, TModuleKey> ModuleRepository => _provider.GetService<IRepository<TModule, TModuleKey>>();

        /// <summary>
        /// 获取 模块功能仓储
        /// </summary>
        protected IRepository<TModuleFunction, Guid> ModuleFunctionRepository => _provider.GetService<IRepository<TModuleFunction, Guid>>();

        /// <summary>
        /// 获取 模块角色仓储
        /// </summary>
        protected IRepository<TModuleRole, Guid> ModuleRoleRepository => _provider.GetService<IRepository<TModuleRole, Guid>>();

        /// <summary>
        /// 获取 模块用户仓储
        /// </summary>
        protected IRepository<TModuleUser, Guid> ModuleUserRepository => _provider.GetService<IRepository<TModuleUser, Guid>>();

        /// <summary>
        /// 获取 用户角色仓储
        /// </summary>
        protected IRepository<TUserRole, TUserRoleKey> UserRoleRepository => _provider.GetService<IRepository<TUserRole, TUserRoleKey>>();

        /// <summary>
        /// 获取 角色仓储
        /// </summary>
        protected IRepository<TRole, TRoleKey> RoleRepository => _provider.GetService<IRepository<TRole, TRoleKey>>();

        /// <summary>
        /// 获取 用户仓储
        /// </summary>
        protected IRepository<TUser, TUserKey> UserRepository => _provider.GetService<IRepository<TUser, TUserKey>>();

        #endregion

        #region Implementation of IFunctionStore<TFunction,in TFunctionInputDto>

        /// <summary>
        /// 获取 功能信息查询数据集
        /// </summary>
        public IQueryable<TFunction> Functions => FunctionRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查功能信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的功能信息编号</param>
        /// <returns>功能信息是否存在</returns>
        public virtual Task<bool> CheckFunctionExists(Expression<Func<TFunction, bool>> predicate, Guid id = default(Guid))
        {
            return FunctionRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 更新功能信息
        /// </summary>
        /// <param name="dtos">包含更新信息的功能信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult> UpdateFunctions(params TFunctionInputDto[] dtos)
        {
            Check.Validate<TFunctionInputDto, Guid>(dtos, nameof(dtos));

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

        #endregion Implementation of IFunctionStore<TFunction,in TFunctionInputDto>

        #region Implementation of IModuleStore<TModule,in TModuleInputDto,in TModuleKey>

        /// <summary>
        /// 获取 模块信息查询数据集
        /// </summary>
        public IQueryable<TModule> Modules => ModuleRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查模块信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块信息编号</param>
        /// <returns>模块信息是否存在</returns>
        public virtual Task<bool> CheckModuleExists(Expression<Func<TModule, bool>> predicate, TModuleKey id = default(TModuleKey))
        {
            return ModuleRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 添加模块信息
        /// </summary>
        /// <param name="dto">要添加的模块信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult> CreateModule(TModuleInputDto dto)
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

            TModule entity = dto.MapTo<TModule>();
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
            if (!Equals(dto.ParentId, default(TModuleKey)))
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
        public virtual async Task<OperationResult> UpdateModule(TModuleInputDto dto)
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
            TModule entity = await ModuleRepository.GetAsync(dto.Id);
            if (entity == null)
            {
                return new OperationResult(OperationResultType.Error, $"编号为“{dto.Id}”的模块信息不存在。");
            }
            entity = dto.MapTo(entity);
            if (!Equals(dto.ParentId, default(TModuleKey)))
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
        public virtual async Task<OperationResult> DeleteModule(TModuleKey id)
        {
            TModule entity = await ModuleRepository.GetAsync(id);
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
                Guid[] functionIds = ModuleFunctionRepository.QueryAsNoTracking(m => m.Id.Equals(id)).Select(m => m.FunctionId).ToArray();
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
        public virtual TModuleKey[] GetModuleTreeIds(params TModuleKey[] rootIds)
        {
            return rootIds.SelectMany(m => ModuleRepository.QueryAsNoTracking(n => n.TreePathString.Contains($"${m}$")).Select(n => n.Id)).Distinct()
                .ToArray();
        }

        private static string GetModuleTreePath(TModuleKey currentId, string parentTreePath, string treePathItemFormat)
        {
            return $"{parentTreePath},{treePathItemFormat.FormatWith(currentId)}";
        }

        #endregion Implementation of IModuleStore<TModule,in TModuleInputDto,in TModuleKey>

        #region Implementation of IModuleFunctionStore<TModuleFunction>

        /// <summary>
        /// 获取 模块功能信息查询数据集
        /// </summary>
        public IQueryable<TModuleFunction> ModuleFunctions => ModuleFunctionRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查模块功能信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块功能信息编号</param>
        /// <returns>模块功能信息是否存在</returns>
        public virtual Task<bool> CheckModuleFunctionExists(Expression<Func<TModuleFunction, bool>> predicate, Guid id = default(Guid))
        {
            return ModuleFunctionRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 设置模块的功能信息
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <param name="functionIds">要设置的功能编号</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult> SetModuleFunctions(TModuleKey moduleId, Guid[] functionIds)
        {
            TModule module = await ModuleRepository.GetAsync(moduleId);
            if (module == null)
            {
                return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
            }

            Guid[] existFunctionIds = ModuleFunctionRepository.QueryAsNoTracking(m => m.ModuleId.Equals(moduleId)).Select(m => m.FunctionId).ToArray();
            Guid[] addFunctionIds = functionIds.Except(existFunctionIds).ToArray();
            Guid[] removeFunctionIds = existFunctionIds.Except(functionIds).ToArray();
            List<string> addNames = new List<string>(), removeNames = new List<string>();
            int count = 0;

            foreach (Guid functionId in addFunctionIds)
            {
                TFunction function = await FunctionRepository.GetAsync(functionId);
                if (function == null)
                {
                    continue;
                }
                TModuleFunction moduleFunction = new TModuleFunction() { ModuleId = moduleId, FunctionId = functionId };
                count = count + await ModuleFunctionRepository.InsertAsync(moduleFunction);
                addNames.Add(function.Name);
            }
            foreach (Guid functionId in removeFunctionIds)
            {
                TFunction function = await FunctionRepository.GetAsync(functionId);
                if (function == null)
                {
                    continue;
                }
                TModuleFunction moduleFunction = ModuleFunctionRepository.QueryAsNoTracking()
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

        #endregion Implementation of IModuleFunctionStore<TModuleFunction>

        #region Implementation of IModuleRoleStore<TModuleRole>

        /// <summary>
        /// 获取 模块角色信息查询数据集
        /// </summary>
        public IQueryable<TModuleRole> ModuleRoles => ModuleRoleRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查模块角色信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块角色信息编号</param>
        /// <returns>模块角色信息是否存在</returns>
        public virtual Task<bool> CheckModuleRoleExists(Expression<Func<TModuleRole, bool>> predicate, Guid id = default(Guid))
        {
            return ModuleRoleRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 设置角色的可访问模块
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="moduleIds">要赋予的模块编号集合</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult> SetRoleModules(TRoleKey roleId, TModuleKey[] moduleIds)
        {
            TRole role = await RoleRepository.GetAsync(roleId);
            if (role == null)
            {
                return new OperationResult(OperationResultType.QueryNull, $"编号为“{roleId}”的角色信息不存在");
            }

            TModuleKey[] existModuleIds = ModuleRoleRepository.QueryAsNoTracking(m => m.RoleId.Equals(roleId)).Select(m => m.ModuleId).ToArray();
            TModuleKey[] addModuleIds = moduleIds.Except(existModuleIds).ToArray();
            TModuleKey[] removeModuleIds = existModuleIds.Except(moduleIds).ToArray();
            List<string> addNames = new List<string>(), removeNames = new List<string>();
            int count = 0;

            foreach (TModuleKey moduleId in addModuleIds)
            {
                TModule module = await ModuleRepository.GetAsync(moduleId);
                if (module == null)
                {
                    return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
                }
                TModuleRole moduleRole = new TModuleRole() { ModuleId = moduleId, RoleId = roleId };
                count = count + await ModuleRoleRepository.InsertAsync(moduleRole);
                addNames.Add(module.Name);
            }
            foreach (TModuleKey moduleId in removeModuleIds)
            {
                TModule module = await ModuleRepository.GetAsync(moduleId);
                if (module == null)
                {
                    return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
                }
                TModuleRole moduleRole = ModuleRoleRepository.GetFirst(m => m.RoleId.Equals(roleId) && m.ModuleId.Equals(moduleId));
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
                Guid[] functionIds = ModuleFunctionRepository.QueryAsNoTracking(m => moduleIds.Contains(m.ModuleId))
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
        public virtual TModuleKey[] GetRoleModuleIds(TRoleKey roleId)
        {
            TModuleKey[] moduleIds = ModuleRoleRepository.QueryAsNoTracking(m => m.RoleId.Equals(roleId)).Select(m => m.ModuleId).Distinct().ToArray();
            return GetModuleTreeIds(moduleIds);
        }

        #endregion Implementation of IModuleRoleStore<TModuleRole>

        #region Implementation of IModuleUserStore<TModuleUser>

        /// <summary>
        /// 获取 模块用户信息查询数据集
        /// </summary>
        public IQueryable<TModuleUser> ModuleUsers => ModuleUserRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查模块用户信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块用户信息编号</param>
        /// <returns>模块用户信息是否存在</returns>
        public virtual Task<bool> CheckModuleUserExists(Expression<Func<TModuleUser, bool>> predicate, Guid id = default(Guid))
        {
            return ModuleUserRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 设置用户的可访问模块
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="moduleIds">要赋给用户的模块编号集合</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult> SetUserModules(TUserKey userId, TModuleKey[] moduleIds)
        {
            TUser user = await UserRepository.GetAsync(userId);
            if (user == null)
            {
                return new OperationResult(OperationResultType.QueryNull, $"编号为“{userId}”的用户信息不存在");
            }

            TModuleKey[] existModuleIds = ModuleUserRepository.QueryAsNoTracking(m => m.UserId.Equals(userId)).Select(m => m.ModuleId).ToArray();
            TModuleKey[] addModuleIds = moduleIds.Except(existModuleIds).ToArray();
            TModuleKey[] removeModuleIds = existModuleIds.Except(moduleIds).ToArray();
            List<string> addNames = new List<string>(), removeNames = new List<string>();
            int count = 0;

            foreach (TModuleKey moduleId in addModuleIds)
            {
                TModule module = await ModuleRepository.GetAsync(moduleId);
                if (module == null)
                {
                    return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
                }
                TModuleUser moduleUser = new TModuleUser() { ModuleId = moduleId, UserId = userId };
                count += await ModuleUserRepository.InsertAsync(moduleUser);
                addNames.Add(module.Name);
            }
            foreach (TModuleKey moduleId in removeModuleIds)
            {
                TModule module = await ModuleRepository.GetAsync(moduleId);
                if (module == null)
                {
                    return new OperationResult(OperationResultType.QueryNull, $"编号为“{moduleId}”的模块信息不存在");
                }
                TModuleUser moduleUser = ModuleUserRepository.GetFirst(m => m.ModuleId.Equals(moduleId) && m.UserId.Equals(userId));
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
        public virtual TModuleKey[] GetUserSelfModuleIds(TUserKey userId)
        {
            TModuleKey[] moduleIds = ModuleUserRepository.QueryAsNoTracking(m => m.UserId.Equals(userId)).Select(m => m.ModuleId).Distinct().ToArray();
            return GetModuleTreeIds(moduleIds);
        }

        /// <summary>
        /// 获取用户及其拥有角色可访问模块编号
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>模块编号集合</returns>
        public virtual TModuleKey[] GetUserWithRoleModuleIds(TUserKey userId)
        {
            TModuleKey[] selfModuleIds = GetUserSelfModuleIds(userId);

            TRoleKey[] roleIds = UserRoleRepository.QueryAsNoTracking(m => m.UserId.Equals(userId)).Select(m => m.RoleId).ToArray();
            TModuleKey[] roleModuleIds = roleIds
                .SelectMany(m => ModuleRoleRepository.QueryAsNoTracking(n => n.RoleId.Equals(m)).Select(n => n.ModuleId))
                .Distinct().ToArray();
            roleModuleIds = GetModuleTreeIds(roleModuleIds);

            return roleModuleIds.Union(selfModuleIds).Distinct().ToArray();
        }

        #endregion Implementation of IModuleUserStore<TModuleUser>
    }

}