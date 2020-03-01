// -----------------------------------------------------------------------
//  <copyright file="DeviceFlowStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 17:16</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using IdentityModel;

using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Data;
using OSharp.Entity;
using OSharp.IdentityServer.Storage.Entities;
using OSharp.Mapping;


namespace OSharp.IdentityServer.Storage
{
    public class DeviceFlowStore : IDeviceFlowStore
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="DeviceFlowStore"/>类型的新实例
        /// </summary>
        public DeviceFlowStore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected IPersistentGrantSerializer Serializer => _serviceProvider.GetService<IPersistentGrantSerializer>();

        protected IRepository<DeviceFlowCodes, int> DeviceFlowCodesRepository =>
            _serviceProvider.GetService<IRepository<DeviceFlowCodes, int>>();

        /// <summary>Stores the device authorization request.</summary>
        /// <param name="deviceCode">The device code.</param>
        /// <param name="userCode">The user code.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public virtual async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
        {
            Check.NotNull(deviceCode, nameof(deviceCode));
            Check.NotNull(userCode, nameof(userCode));
            Check.NotNull(data, nameof(data));
            
            DeviceFlowCodes entity = ToEntity(data, deviceCode, userCode);
            int count = await DeviceFlowCodesRepository.InsertAsync(entity);
        }

        /// <summary>Finds device authorization by user code.</summary>
        /// <param name="userCode">The user code.</param>
        /// <returns></returns>
        public virtual Task<DeviceCode> FindByUserCodeAsync(string userCode)
        {
            DeviceFlowCodes deviceFlowCodes = DeviceFlowCodesRepository.QueryAsNoTracking(m => m.UserCode == userCode).FirstOrDefault();
            if (deviceFlowCodes == null)
            {
                return null;
            }

            DeviceCode model = Serializer.Deserialize<DeviceCode>(deviceFlowCodes.Data);

            return Task.FromResult(model);
        }

        /// <summary>Finds device authorization by device code.</summary>
        /// <param name="deviceCode">The device code.</param>
        public virtual Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            DeviceFlowCodes deviceFlowCodes = DeviceFlowCodesRepository.QueryAsNoTracking(m => m.DeviceCode == deviceCode).FirstOrDefault();
            if (deviceFlowCodes == null)
            {
                return null;
            }

            DeviceCode model = Serializer.Deserialize<DeviceCode>(deviceFlowCodes.Data);

            return Task.FromResult(model);
        }

        /// <summary>Updates device authorization, searching by user code.</summary>
        /// <param name="userCode">The user code.</param>
        /// <param name="data">The data.</param>
        public virtual async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
        {
            DeviceFlowCodes existing = DeviceFlowCodesRepository.GetFirst(m => m.UserCode == userCode);
            if (existing == null)
            {
                throw new InvalidOperationException("Could not update device code");
            }

            DeviceFlowCodes entity = ToEntity(data, existing.DeviceCode, userCode);

            existing = entity.MapTo(existing);
            await DeviceFlowCodesRepository.UpdateAsync(existing);
        }

        /// <summary>
        /// Removes the device authorization, searching by device code.
        /// </summary>
        /// <param name="deviceCode">The device code.</param>
        public virtual async Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            DeviceFlowCodes existing = DeviceFlowCodesRepository.GetFirst(m => m.DeviceCode == deviceCode);
            if (existing == null)
            {
                return;
            }

            await DeviceFlowCodesRepository.DeleteAsync(existing);
        }

        protected virtual DeviceFlowCodes ToEntity(DeviceCode model, string deviceCode, string userCode)
        {
            if (model == null || deviceCode == null || userCode == null)
            {
                return null;
            }

            return new DeviceFlowCodes()
            {
                DeviceCode = deviceCode,
                UserCode = userCode,
                ClientId = model.ClientId,
                SubjectId = model.Subject?.FindFirst(JwtClaimTypes.Subject).Value,
                CreationTime = model.CreationTime,
                Expiration = model.CreationTime.AddSeconds(model.Lifetime),
                Data = Serializer.Serialize(model)
            };
        }
    }
}