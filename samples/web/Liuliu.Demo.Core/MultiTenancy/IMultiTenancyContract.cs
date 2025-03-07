using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Dtos;
using Liuliu.Demo.MultiTenancy.Entities;
using OSharp.Data;

namespace Liuliu.Demo.MultiTenancy
{
    /// <summary>
    /// 业务契约接口：多租户模块
    /// </summary>
    public interface IMultiTenancyContract
    {
        #region 多租户信息业务

        /// <summary>
        /// 获取 多租户信息查询数据集
        /// </summary>
        IQueryable<Tenant> Tenants { get; }

        /// <summary>
        /// 检查多租户信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的多租户信息编号</param>
        /// <returns>多租户信息是否存在</returns>
        Task<bool> CheckTenantExists(Expression<Func<Tenant, bool>> predicate, long id = default(long));

        /// <summary>
        /// 添加多租户信息信息
        /// </summary>
        /// <param name="dtos">要添加的多租户信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> CreateTenants(params TenantInputDto[] dtos);

        /// <summary>
        /// 更新多租户信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的多租户信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateTenants(params TenantInputDto[] dtos);

        /// <summary>
        /// 租户启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SetTenantEnable(long id);

        /// <summary>
        /// 租户禁用
        /// </summary>
        /// <param name="id"></param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SetTenantDisable(long id);

        /// <summary>
        /// 删除多租户信息信息
        /// </summary>
        /// <param name="ids">要删除的多租户信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteTenants(params long[] ids);


        #endregion

        #region 多租户缓存业务
        /// <summary>
        /// 获取所有租户缓存
        /// </summary>
        /// <returns></returns>
        List<TenantOutputDto> GetAllTenants();

        /// <summary>
        /// 根据标识获取租户
        /// </summary>
        /// <param name="tenantKey"></param>
        /// <returns></returns>
        TenantOutputDto GetTenant(string tenantKey);

        /// <summary>
        /// 清理租户缓存
        /// </summary>
        /// <param name="tenantKey"></param>
        void ClearTenant(string tenantKey);

        /// <summary>
        /// 清理所有租户缓存
        /// </summary>
        void ClearAllTenants();
        #endregion
    }
}
