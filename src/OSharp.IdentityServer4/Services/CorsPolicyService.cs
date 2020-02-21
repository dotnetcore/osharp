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

using OSharp.Data;
using OSharp.Entity;
using OSharp.IdentityServer4.Entities;


namespace OSharp.IdentityServer4.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Client, int> _clientRepository;

        /// <summary>
        /// 初始化一个<see cref="CorsPolicyService"/>类型的新实例
        /// </summary>
        public CorsPolicyService(IHttpContextAccessor httpContextAccessor, IRepository<Client, int>clientRepository)
        {
            Check.NotNull(httpContextAccessor, nameof(httpContextAccessor));
            
            _httpContextAccessor = httpContextAccessor;
            _clientRepository = clientRepository;
        }

        /// <summary>Determines whether origin is allowed.</summary>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            origin = origin.ToLowerInvariant();

            List<string> origins = _clientRepository.QueryAsNoTracking().SelectMany(m => m.AllowedCorsOrigins.Select(n => n.Origin)).ToList();
            bool isAllowed = origins.Where(m => m != null).Distinct().Contains(origin, StringComparer.OrdinalIgnoreCase);
            return Task.FromResult(isAllowed);
        }
    }
}