// -----------------------------------------------------------------------
//  <copyright file="SecurityManagerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-15 20:36</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Security
{
    /// <summary>
    /// 权限安全管理器基类
    /// </summary>
    public abstract class SecurityManagerBase<TFunction, TFunctionInputDto, TEntityInfo, TEntityInfoInputDto, TModule, TModuleInputDto, TModuleKey,
        TModuleFunction, TModuleRole, TModuleUser, TRoleKey, TUserKey>
        : IFunctionStore<TFunction, TFunctionInputDto>,
        IEntityInfoStore<TEntityInfo, TEntityInfoInputDto>,
        IModuleStore<TModule, TModuleInputDto, TModuleKey>,
        IModuleFunctionStore<TModuleFunction>,
        IModuleRoleStore<TModuleRole>,
        IModuleUserStore<TModuleUser>
        where TFunction : IFunction, IEntity<Guid>
        where TFunctionInputDto : FunctionInputDtoBase
        where TEntityInfo : IEntityInfo, IEntity<Guid>
        where TEntityInfoInputDto : EntityInfoInputDtoBase
        where TModule : ModuleBase<TModuleKey>
        where TModuleInputDto : ModuleInputDtoBase<TModuleKey>
        where TModuleFunction : ModuleFunctionBase<TModuleKey>
        where TModuleRole : ModuleRoleBase<TModuleKey, TRoleKey>
        where TModuleUser : ModuleUserBase<TModuleKey, TUserKey>
        where TModuleKey : struct, IEquatable<TModuleKey>
    {
        private readonly IRepository<TFunction, Guid> _functionRepository;
        private readonly IRepository<TEntityInfo, Guid> _entityInfoRepository;
        private readonly IRepository<TModule, TModuleKey> _moduleRepository;
        private readonly IRepository<TModuleFunction, Guid> _moduleFunctionRepository;
        private readonly IRepository<TModuleRole, Guid> _moduleRoleRepository;
        private readonly IRepository<TModuleUser, Guid> _moduleUserRepository;

        /// <summary>
        /// 初始化一个<see cref="SecurityManagerBase"/>类型的新实例
        /// </summary>
        protected SecurityManagerBase(
            IRepository<TFunction, Guid> functionRepository,
            IRepository<TEntityInfo, Guid> entityInfoRepository,
            IRepository<TModule, TModuleKey> moduleRepository,
            IRepository<TModuleFunction, Guid> moduleFunctionRepository,
            IRepository<TModuleRole, Guid> moduleRoleRepository,
            IRepository<TModuleUser, Guid> moduleUserRepository)
        {
            _functionRepository = functionRepository;
            _entityInfoRepository = entityInfoRepository;
            _moduleRepository = moduleRepository;
            _moduleFunctionRepository = moduleFunctionRepository;
            _moduleRoleRepository = moduleRoleRepository;
            _moduleUserRepository = moduleUserRepository;
        }

        #region Implementation of IFunctionStore<TFunction,in TFunctionInputDto>

        /// <summary>
        /// 获取 功能信息查询数据集
        /// </summary>
        public IQueryable<TFunction> Functions
        {
            get { return _functionRepository.Query(); }
        }

        /// <summary>
        /// 检查功能信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的功能信息编号</param>
        /// <returns>功能信息是否存在</returns>
        public Task<bool> CheckFunctionExists(Expression<Func<TFunction, bool>> predicate, Guid id = default(Guid))
        {
            return _functionRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 更新功能信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的功能信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> UpdateFunctions(params TFunctionInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            return _functionRepository.UpdateAsync(dtos,
                async (dto, entity) =>
                {
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
                });
        }

        #endregion

        #region Implementation of IEntityInfoStore<TEntityInfo,in TEntityInfoInputDto>

        /// <summary>
        /// 获取 实体信息查询数据集
        /// </summary>
        public IQueryable<TEntityInfo> EntityInfos
        {
            get { return _entityInfoRepository.Query(); }
        }

        /// <summary>
        /// 检查实体信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的实体信息编号</param>
        /// <returns>实体信息是否存在</returns>
        public Task<bool> CheckEntityInfoExists(Expression<Func<TEntityInfo, bool>> predicate, Guid id = default(Guid))
        {
            return _entityInfoRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 更新实体信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的实体信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> UpdateEntityInfos(params TEntityInfoInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            return _entityInfoRepository.UpdateAsync(dtos);
        }

        #endregion

        #region Implementation of IModuleStore<TModule,in TModuleInputDto,in TModuleKey>

        /// <summary>
        /// 获取 模块信息查询数据集
        /// </summary>
        public IQueryable<TModule> Modules
        {
            get { return _moduleRepository.Query(); }
        }

        /// <summary>
        /// 检查模块信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块信息编号</param>
        /// <returns>模块信息是否存在</returns>
        public Task<bool> CheckModuleExists(Expression<Func<TModule, bool>> predicate, TModuleKey id = default(TModuleKey))
        {
            return _moduleRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 添加模块信息信息
        /// </summary>
        /// <param name="dto">要添加的模块信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> CreateModule(TModuleInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));
            var exist = Modules.Where(m => m.Name == dto.Name && m.ParentId != null && m.ParentId.Equals(dto.ParentId))
                .SelectMany(m => Modules.Where(n => n.Id.Equals(m.ParentId)).Select(n => n.Name)).FirstOrDefault();
            if (exist != null)
            {
                return new OperationResult(OperationResultType.Error, $"模块“{exist}”中已存在名称为“{dto.Name}”的子模块，不能重复添加。");
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
            if (!dto.ParentId.Equals(default(TModuleKey)))
            {
                var parent = Modules.Where(m => m.Id.Equals(dto.ParentId)).Select(m => new { m.Id, m.TreePathString }).FirstOrDefault();
                if (parent == null)
                {
                    return new OperationResult(OperationResultType.Error, $"编号为“{dto.ParentId}”的父模块信息不存在");
                }
                entity.ParentId = dto.ParentId;
                entity.TreePathString = GetModuleTreePath(parent.Id, parent.TreePathString);
            }
            else
            {
                entity.ParentId = null;
            }
            return await _moduleRepository.InsertAsync(entity) > 0
                ? new OperationResult(OperationResultType.Success, $"模块“{dto.Name}”创建成功")
                : OperationResult.NoChanged;
        }

        /// <summary>
        /// 更新模块信息信息
        /// </summary>
        /// <param name="dto">包含更新信息的模块信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> UpdateModule(TModuleInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));
            var exist = Modules.Where(m => m.Name == dto.Name && m.ParentId != null && m.ParentId.Equals(dto.ParentId))
                .SelectMany(m => Modules.Where(n => n.Id.Equals(m.ParentId)).Select(n => new { n.Id, n.Name })).FirstOrDefault();
            if (exist != null)
            {
                return new OperationResult(OperationResultType.Error, $"模块“{exist}”中已存在名称为“{dto.Name}”的子模块，不能重复添加。");
            }
            TModule entity = await _moduleRepository.GetAsync(dto.Id);
            if (entity == null)
            {
                return new OperationResult(OperationResultType.Error, $"编号为“{dto.Id}”的模块信息不存在。");
            }
            entity = dto.MapTo(entity);
            if (!dto.ParentId.Equals(default(TModuleKey)))
            {
                if (!entity.ParentId.Equals(dto.ParentId))
                {
                    var parent = Modules.Where(m => m.Id.Equals(dto.ParentId)).Select(m => new { m.Id, m.TreePathString }).FirstOrDefault();
                    if (parent == null)
                    {
                        return new OperationResult(OperationResultType.Error, $"编号为“{dto.ParentId}”的父模块信息不存在");
                    }
                    entity.ParentId = dto.ParentId;
                    entity.TreePathString = GetModuleTreePath(parent.Id, parent.TreePathString);
                }
                else
                {
                    entity.ParentId = null;
                }
            }
            else
            {
                entity.ParentId = null;
            }
            return await _moduleRepository.UpdateAsync(entity) > 0
                ? new OperationResult(OperationResultType.Success, $"模块“{dto.Name}”更新成功")
                : OperationResult.NoChanged;
        }

        /// <summary>
        /// 删除模块信息信息
        /// </summary>
        /// <param name="id">要删除的模块信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteModule(TModuleKey id)
        {
            TModule entity = await _moduleRepository.GetAsync(id);
            if (entity == null)
            {
                return OperationResult.Success;
            }
            if (await _moduleRepository.CheckExistsAsync(m => m.ParentId.Equals(id)))
            {
                return new OperationResult(OperationResultType.Error, $"模块“{entity.Name}”的子模块不为空，不能删除");
            }
            //清除附属引用
            await _moduleFunctionRepository.DeleteBatchAsync(m => m.ModuleId.Equals(id));
            await _moduleRoleRepository.DeleteBatchAsync(m => m.ModuleId.Equals(id));
            await _moduleUserRepository.DeleteBatchAsync(m => m.ModuleId.Equals(id));

            return await _moduleRepository.DeleteAsync(entity) > 0
                ? new OperationResult(OperationResultType.Success, $"模块“{entity.Name}”删除成功")
                : OperationResult.NoChanged;
        }

        private string GetModuleTreePath(TModuleKey parentId, string parentTreePath)
        {
            const string treePathItemFormat = "${0}$";
            if (parentTreePath == null)
            {
                return null;
            }
            return treePathItemFormat + treePathItemFormat.FormatWith(parentId);
        }

        #endregion

        #region Implementation of IModuleFunctionStore<TModuleFunction>

        /// <summary>
        /// 获取 模块功能信息查询数据集
        /// </summary>
        public IQueryable<TModuleFunction> ModuleFunctions
        {
            get { return _moduleFunctionRepository.Query(); }
        }

        /// <summary>
        /// 检查模块功能信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块功能信息编号</param>
        /// <returns>模块功能信息是否存在</returns>
        public Task<bool> CheckModuleFunctionExists(Expression<Func<TModuleFunction, bool>> predicate, Guid id = default(Guid))
        {
            return _moduleFunctionRepository.CheckExistsAsync(predicate, id);
        }

        #endregion

        #region Implementation of IModuleRoleStore<TModuleRole>

        /// <summary>
        /// 获取 模块角色信息查询数据集
        /// </summary>
        public IQueryable<TModuleRole> ModuleRoles
        {
            get { return _moduleRoleRepository.Query(); }
        }

        /// <summary>
        /// 检查模块角色信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块角色信息编号</param>
        /// <returns>模块角色信息是否存在</returns>
        public Task<bool> CheckModuleRoleExists(Expression<Func<TModuleRole, bool>> predicate, Guid id = default(Guid))
        {
            return _moduleRoleRepository.CheckExistsAsync(predicate, id);
        }

        #endregion

        #region Implementation of IModuleUserStore<TModuleUser>

        /// <summary>
        /// 获取 模块用户信息查询数据集
        /// </summary>
        public IQueryable<TModuleUser> ModuleUsers
        {
            get { return _moduleUserRepository.Query(); }
        }

        /// <summary>
        /// 检查模块用户信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块用户信息编号</param>
        /// <returns>模块用户信息是否存在</returns>
        public Task<bool> CheckModuleUserExists(Expression<Func<TModuleUser, bool>> predicate, Guid id = default(Guid))
        {
            return _moduleUserRepository.CheckExistsAsync(predicate, id);
        }

        #endregion
    }
}