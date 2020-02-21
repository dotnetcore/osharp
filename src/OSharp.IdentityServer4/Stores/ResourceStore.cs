// -----------------------------------------------------------------------
//  <copyright file="ResourceStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 18:41</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Stores;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.IdentityServer4.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="ResourceStore"/>类型的新实例
        /// </summary>
        public ResourceStore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected IRepository<Entities.ApiResource, int> ApiResourceRepository
            => _serviceProvider.GetService<IRepository<Entities.ApiResource, int>>();

        protected IRepository<Entities.IdentityResource, int> IdentityResourceRepository
            => _serviceProvider.GetService<IRepository<Entities.IdentityResource, int>>();

        /// <summary>Gets identity resources by scope name.</summary>
        public virtual Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();
            var entities = IdentityResourceRepository.Query(m => m.UserClaims, m => m.Properties).Where(m => names.Contains(m.Name)).ToArray();
            var models = entities.Select(m => m.MapTo<IdentityResource>());
            return Task.FromResult(models);
        }

        /// <summary>Gets API resources by scope name.</summary>
        public virtual Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();
            var entities = ApiResourceRepository.Query(m => m.Secrets,
                m => m.Scopes,
                m => m.UserClaims,
                m => m.Properties).Where(m => names.Contains(m.Name)).ToArray();
            var models = entities.Select(m => m.MapTo<ApiResource>());
            return Task.FromResult(models);
        }

        /// <summary>Finds the API resource by name.</summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual Task<ApiResource> FindApiResourceAsync(string name)
        {
            var entity = ApiResourceRepository.Query(m => m.Secrets,
                m => m.Scopes,
                m => m.UserClaims,
                m => m.Properties).FirstOrDefault(m => m.Name == name);
            var model = entity.MapTo<ApiResource>();
            return Task.FromResult(model);
        }

        /// <summary>Gets all resources.</summary>
        public virtual Task<Resources> GetAllResourcesAsync()
        {
            var identities = IdentityResourceRepository.Query(m => m.UserClaims, m => m.Properties).ToArray()
                .Select(m => m.MapTo<IdentityResource>());
            var apis = ApiResourceRepository.Query(m => m.Secrets,
                m => m.Scopes,
                m => m.UserClaims,
                m => m.Properties).ToArray().Select(m => m.MapTo<ApiResource>());
            var models = new Resources(identities, apis);
            return Task.FromResult(models);
        }
    }
}