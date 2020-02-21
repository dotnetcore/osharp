// -----------------------------------------------------------------------
//  <copyright file="PersistedGrantStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 18:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Stores;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.IdentityServer4.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="PersistedGrantStore"/>类型的新实例
        /// </summary>
        public PersistedGrantStore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected IRepository<Entities.PersistedGrant, int> PersistedGrantRepository =>
            _serviceProvider.GetService<IRepository<Entities.PersistedGrant, int>>();

        /// <summary>Stores the grant.</summary>
        /// <param name="grant">The grant.</param>
        /// <returns></returns>
        public virtual async Task StoreAsync(PersistedGrant grant)
        {
            var existing = PersistedGrantRepository.GetFirst(m => m.Key == grant.Key);
            if (existing == null)
            {
                var entity = grant.MapTo<Entities.PersistedGrant>();
                await PersistedGrantRepository.InsertAsync(entity);
                return;
            }

            existing = grant.MapTo(existing);
            await PersistedGrantRepository.UpdateAsync(existing);
        }

        /// <summary>Gets the grant.</summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual Task<PersistedGrant> GetAsync(string key)
        {
            var entity = PersistedGrantRepository.QueryAsNoTracking(m => m.Key == key).FirstOrDefault();
            var model = entity.MapTo<PersistedGrant>();
            return Task.FromResult(model);
        }

        /// <summary>Gets all grants for a given subject id.</summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public virtual Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var entities = PersistedGrantRepository.QueryAsNoTracking(m => m.SubjectId == subjectId).ToList();
            var models = entities.Select(e => e.MapTo<PersistedGrant>());
            return Task.FromResult(models);
        }

        /// <summary>Removes the grant by key.</summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual async Task RemoveAsync(string key)
        {
            var existing = PersistedGrantRepository.GetFirst(m => m.Key == key);
            if (existing == null)
            {
                return;
            }

            await PersistedGrantRepository.DeleteAsync(existing);
        }

        /// <summary>
        /// Removes all grants for a given subject id and client id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public virtual async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var entities = PersistedGrantRepository.Query(m => m.SubjectId == subjectId && m.ClientId == clientId).ToArray();
            if (entities.Length == 0)
            {
                return;
            }

            await PersistedGrantRepository.DeleteAsync(entities);
        }

        /// <summary>
        /// Removes all grants of a give type for a given subject id and client id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public virtual async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var entities = PersistedGrantRepository.Query(m => m.SubjectId == subjectId && m.ClientId == clientId && m.Type == type).ToArray();
            if (entities.Length == 0)
            {
                return;
            }

            await PersistedGrantRepository.DeleteAsync(entities);
        }
    }
}