// -----------------------------------------------------------------------
//  <copyright file="IdentityServerPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-02 22:22</last-date>
// -----------------------------------------------------------------------

using System.Linq;

using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;
using OSharp.Entity;
using OSharp.IdentityServer;
using OSharp.IdentityServer.Storage.Entities;
using OSharp.Mapping;


namespace Liuliu.Demo.Identity
{
    public class IdentityServerPack : IdentityServerPackBase<User>
    {
        #region Overrides of IdentityServerPackBase<User>

        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Asp应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            base.UsePack(app);

            ServiceLocator.Instance.ExecuteScopedWork(provider =>
            {
                IUnitOfWorkManager manager = provider.GetService<IUnitOfWorkManager>();
                IRepository<Client, int> clientRepository = provider.GetService<IRepository<Client, int>>();
                if (!clientRepository.QueryAsNoTracking().Any())
                {
                    var clients = Config.Clients.Select(m => m.MapTo<Client>()).ToArray();
                    clientRepository.Insert(clients);
                }

                IRepository<IdentityResource, int> identityResourceRepository = provider.GetService<IRepository<IdentityResource, int>>();
                if (!identityResourceRepository.QueryAsNoTracking().Any())
                {
                    var ids = Config.Ids.Select(m => m.MapTo<IdentityResource>()).ToArray();
                    identityResourceRepository.Insert(ids);
                }

                IRepository<ApiResource, int> apiResourceRepository = provider.GetService<IRepository<ApiResource, int>>();
                if (!apiResourceRepository.QueryAsNoTracking().Any())
                {
                    var apis = Config.Apis.Select(m => m.MapTo<ApiResource>()).ToArray();
                    apiResourceRepository.Insert(apis);
                }
                manager.Commit();
            }, false);
        }

        #endregion
    }
}