using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Entities;
using Liuliu.Demo.MultiTenancy.Dtos;
using OSharp.Data;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Core.Options;
using OSharp.Exceptions;

namespace Liuliu.Demo.MultiTenancy
{
    public partial class MultiTenancyService
    {
        /// <summary>
        /// 获取 站内信信息查询数据集
        /// </summary>
        public IQueryable<Tenant> Tenants
        {
            get { return TenantRepository.QueryAsNoTracking(); }
        }

        /// <summary>
        /// 检查站内信信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的站内信信息编号</param>
        /// <returns>站内信信息是否存在</returns>
        public Task<bool> CheckTenantExists(Expression<Func<Tenant, bool>> predicate, long id = default(long))
        {
            return TenantRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 添加站内信信息
        /// </summary>
        /// <param name="dtos">要添加的站内信信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> CreateTenants(params TenantInputDto[] dtos)
        {
            Check2.Validate<TenantInputDto, long>(dtos, nameof(dtos));
            var result = TenantRepository.InsertAsync(dtos);
            ClearAllTenants();
            return result;
        }

        public OperationResult InitTenants()
        {
            var _dbContexts = ServiceProvider.GetOSharpOptions().DbContexts;
            OsharpDbContextOptions dbContextOptions = _dbContexts.Values.FirstOrDefault(m => m.DbContextType.Name == "DefaultDbContext");
            if (dbContextOptions == null)
            {
                throw new OsharpException($"数据上下文“DefaultDbContext”的数据上下文配置信息不存在");
            }
            var defaultConnectionString = dbContextOptions.ConnectionString;

            var dtos = new List<TenantInputDto>();
            var dto = new TenantInputDto()
            {
                TenantKey = "Default",
                ConnectionString = defaultConnectionString,
                CustomJson = "",
                ExpireDate = null,
                Host = "localhost",
                IsEnabled = true,
                Name = "杭州智密科技有限公司",
                ShortName = "智密科技"
            };
            dtos.Add(dto);

            var result = TenantRepository.Insert(dtos);
            ClearAllTenants();
            return result;
        }

        /// <summary>
        /// 更新站内信信息
        /// </summary>
        /// <param name="dtos">包含更新信息的站内信信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> UpdateTenants(params TenantInputDto[] dtos)
        {
            Check2.Validate<TenantInputDto, long>(dtos, nameof(dtos));
            var result = TenantRepository.UpdateAsync(dtos);
            ClearAllTenants();
            return result;
        }

        /// <summary>
        /// 租户启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetTenantEnable(long id)
        {
            return await SetTenantIsEnable(id, true);
        }

        /// <summary>
        /// 租户禁用
        /// </summary>
        /// <param name="id"></param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetTenantDisable(long id)
        {
            return await SetTenantIsEnable(id, false);
        }

        /// <summary>
        /// 删除站内信信息
        /// </summary>
        /// <param name="ids">要删除的站内信信息编号</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> DeleteTenants(params long[] ids)
        {
            Check.NotNull(ids, nameof(ids));
            var result = TenantRepository.DeleteAsync(ids);
            ClearAllTenants();
            return result;
        }

        /// <summary>
        /// 设置租户启用状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns>业务操作结果</returns>
        private async Task<OperationResult> SetTenantIsEnable(long id,bool enable)
        {
            Check.NotNull(id, nameof(id));
            var tenantEntity = TenantRepository.Query().FirstOrDefault(x => x.Id == id);
            if (tenantEntity==null)
            {
                return new OperationResult(OperationResultType.Error, "数据未找到");
            }

            tenantEntity.IsEnabled = enable;

            var count = await TenantRepository.UpdateAsync(tenantEntity);
            return count>0? new OperationResult(OperationResultType.Success) : new OperationResult(OperationResultType.NoChanged);
        }
    }
}
