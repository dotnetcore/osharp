// -----------------------------------------------------------------------
//  <copyright file="TokenCleanupService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-21 18:52</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Entity;
using OSharp.IdentityServer4.Entities;
using OSharp.IdentityServer4.Options;
using OSharp.IdentityServer4.Services;


namespace OSharp.IdentityServer4.Stores
{
    public class TokenCleanupService : ITokenCleanupService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IdentityServerOptions _options;

        /// <summary>
        /// 初始化一个<see cref="TokenCleanupService"/>类型的新实例
        /// </summary>
        public TokenCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = serviceProvider.GetService<IConfiguration>().GetInstance("OSharp:IdentityServer", new IdentityServerOptions());
            if (_options.TokenCleanupBatchSize < 1)
            {
                _options.TokenCleanupBatchSize = 1;
            }
        }

        protected IRepository<PersistedGrant, int> PersistedGrantRepository => _serviceProvider.GetService<IRepository<PersistedGrant, int>>();

        protected IRepository<DeviceFlowCodes, int> DeviceFlowCodesRepository => _serviceProvider.GetService<IRepository<DeviceFlowCodes, int>>();

        /// <summary>
        /// 清理过期的Token
        /// </summary>
        /// <returns></returns>
        public async Task RemoveExpiredTokenAsync()
        {
            await RemovePersistedGrantsAsync();
            await RemoveDeviceFlowCodesAsync();
        }

        protected virtual async Task RemovePersistedGrantsAsync()
        {
            int found = int.MaxValue;
            while (found >= _options.TokenCleanupBatchSize)
            {
                PersistedGrant[] grants = PersistedGrantRepository.Query(m => m.Expiration < DateTime.UtcNow).OrderBy(m => m.Key)
                    .Take(_options.TokenCleanupBatchSize).ToArray();
                found = grants.Length;
                if (found > 0)
                {
                    await PersistedGrantRepository.DeleteAsync(grants);
                }
            }
        }

        protected virtual async Task RemoveDeviceFlowCodesAsync()
        {
            int found = int.MaxValue;
            while (found >= _options.TokenCleanupBatchSize)
            {
                DeviceFlowCodes[] codeses = DeviceFlowCodesRepository.Query(m => m.Expiration < DateTime.UtcNow).OrderBy(m => m.DeviceCode)
                    .Take(_options.TokenCleanupBatchSize).ToArray();
                found = codeses.Length;
                if (found > 0)
                {
                    await DeviceFlowCodesRepository.DeleteAsync(codeses);
                }
            }
        }
    }
}