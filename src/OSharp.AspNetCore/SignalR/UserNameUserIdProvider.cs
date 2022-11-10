// -----------------------------------------------------------------------
//  <copyright file="UserNameUserIdProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-04 20:34</last-date>
// -----------------------------------------------------------------------


namespace OSharp.AspNetCore.SignalR;

/// <summary>
/// 用户名用户标识提供者
/// </summary>
public class UserNameUserIdProvider : IUserIdProvider
{
    /// <summary>Gets the user ID for the specified connection.</summary>
    /// <param name="connection">The connection to get the user ID for.</param>
    /// <returns>The user ID for the specified connection.</returns>
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
    }
}
