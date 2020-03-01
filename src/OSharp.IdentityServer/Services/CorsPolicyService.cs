// -----------------------------------------------------------------------
//  <copyright file="CorsPolicyService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-21 13:40</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Data;
using OSharp.Entity;
using OSharp.IdentityServer.Storage.Entities;


namespace OSharp.IdentityServer.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 初始化一个<see cref="CorsPolicyService"/>类型的新实例
        /// </summary>
        public CorsPolicyService(IHttpContextAccessor httpContextAccessor)
        {
            Check.NotNull(httpContextAccessor, nameof(httpContextAccessor));
            
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>Determines whether origin is allowed.</summary>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            origin = origin.ToLowerInvariant();

            IServiceProvider provider = _httpContextAccessor.HttpContext.RequestServices;
            IRepository<Client, int> clientRepository = provider.GetService<IRepository<Client, int>>();
            List<string> origins = clientRepository.QueryAsNoTracking().SelectMany(m => m.AllowedCorsOrigins.Select(n => n.Origin)).ToList();
            bool isAllowed = origins.Where(m => m != null).Distinct().Contains(origin, StringComparer.OrdinalIgnoreCase);
            return Task.FromResult(isAllowed);
        }
    }
}