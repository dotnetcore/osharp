// -----------------------------------------------------------------------
//  <copyright file="ClientStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 10:33</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Stores;

using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.IdentityServer.Storage
{
    /// <summary>
    /// 客户端存储
    /// </summary>
    public class ClientStore : IClientStore
    {
        private readonly IRepository<Entities.Client, int> _clientRepository;

        /// <summary>
        /// 初始化一个<see cref="ClientStore"/>类型的新实例
        /// </summary>
        public ClientStore(IRepository<Entities.Client, int> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        /// <summary>Finds a client by id</summary>
        /// <param name="clientId">The client id</param>
        /// <returns>The client</returns>
        public virtual Task<Client> FindClientByIdAsync(string clientId)
        {
            Entities.Client client = _clientRepository.Query(m => m.AllowedCorsOrigins,
                m => m.AllowedGrantTypes,
                m => m.AllowedScopes,
                m => m.Claims,
                m => m.ClientSecrets,
                m => m.IdentityProviderRestrictions,
                m => m.PostLogoutRedirectUris,
                m => m.Properties,
                m => m.RedirectUris).FirstOrDefault(m => m.ClientId == clientId);

            Client model = client.MapTo<Client>();
            return Task.FromResult(model);
        }
    }
}