using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Entities;
using Liuliu.Demo.MultiTenancy.Dtos;
using OSharp.Data;
using OSharp.Caching;
using OSharp.Entity;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Core.Options;
using OSharp.Exceptions;

namespace Liuliu.Demo.MultiTenancy
{
    public partial class MultiTenancyService
    {
        private readonly int tenantCacheSeconds = 600; //10分钟
        private const string tenantCacheKeyPrefix = "Tenant_";
        private const string allTenantsCacheKey = "AllTenants";
        private object tenantLockObj = new object();

        /// <summary>
        /// 获取所有租户缓存
        /// </summary>
        /// <returns></returns>
        public List<TenantOutputDto> GetAllTenants()
        {
            var cacheKey = allTenantsCacheKey;
            var cacheData = Cache.Get<List<TenantOutputDto>>(cacheKey);
            if (cacheData == null)
            {
                lock (tenantLockObj)
                {
                    cacheData = Cache.Get<List<TenantOutputDto>>(cacheKey);
                    if (cacheData == null)
                    {
                        cacheData = Tenants.ToOutput<Tenant, TenantOutputDto>().ToList();

                        if (cacheData == null || cacheData.Count == 0)
                        {
                            InitTenants();
                            cacheData = Tenants.ToOutput<Tenant, TenantOutputDto>().ToList();
                        }

                        Cache.Set(cacheKey, cacheData, tenantCacheSeconds);
                    }
                }
            }
            return cacheData;
        }

        /// <summary>
        /// 根据标识获取租户
        /// </summary>
        /// <param name="tenantKey"></param>
        /// <returns></returns>
        public TenantOutputDto GetTenant(string tenantKey)
        {
            if (string.IsNullOrEmpty(tenantKey))
            {
                return null;
            }

            // 尝试从缓存获取
            string cacheKey = $"{tenantCacheKeyPrefix}{tenantKey}";

            var cacheData = Cache.Get<TenantOutputDto>(cacheKey);

            if (cacheData == null)
            {
                cacheData = GetAllTenants().FirstOrDefault(p=>p.TenantKey==tenantKey);
                Cache.Set(cacheKey, cacheData, tenantCacheSeconds);
            }

            return cacheData;
        }

        /// <summary>
        /// 清理租户缓存
        /// </summary>
        /// <param name="tenantKey"></param>
        public void ClearTenant(string tenantKey)
        {
            string cacheKey = $"{tenantCacheKeyPrefix}{tenantKey}";
            Cache.Remove(cacheKey);
        }

        /// <summary>
        /// 清理所有租户缓存
        /// </summary>
        public void ClearAllTenants()
        {
            foreach (var tenant in GetAllTenants())
            {
                ClearTenant(tenant.TenantKey);
            }
            Cache.Remove(allTenantsCacheKey);
        }
    }
}
