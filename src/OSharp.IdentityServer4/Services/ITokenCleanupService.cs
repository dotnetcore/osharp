// -----------------------------------------------------------------------
//  <copyright file="ITokenCleanupService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-21 18:41</last-date>
// -----------------------------------------------------------------------

using System.Threading.Tasks;


namespace OSharp.IdentityServer4.Services
{
    /// <summary>
    /// 定义Token清理服务
    /// </summary>
    public interface ITokenCleanupService
    {
        /// <summary>
        /// 清理过期的Token
        /// </summary>
        /// <returns></returns>
        Task RemoveExpiredTokenAsync();
    }
}