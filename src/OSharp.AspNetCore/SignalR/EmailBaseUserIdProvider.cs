// -----------------------------------------------------------------------
//  <copyright file="EmailBaseUserIdProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-11-07 23:46</last-date>
// -----------------------------------------------------------------------

using System.Security.Claims;

using Microsoft.AspNetCore.SignalR;


namespace OSharp.AspNetCore.SignalR
{
    /// <summary>
    /// 基于Email的用户标识提供者
    /// </summary>
    public class EmailBaseUserIdProvider:IUserIdProvider
    {
        /// <summary>Gets the user ID for the specified connection.</summary>
        /// <param name="connection">The connection to get the user ID for.</param>
        /// <returns>The user ID for the specified connection.</returns>
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}