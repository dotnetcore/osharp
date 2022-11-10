// -----------------------------------------------------------------------
//  <copyright file="Logout_RemoveRefreshTokenEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-11 2:27</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Identity;

using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Identity.Events;

public class Logout_RemoveRefreshTokenEventHandler : EventHandlerBase<LogoutEventData>
{
    private readonly IPrincipal _principal;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// 初始化一个<see cref="Logout_RemoveRefreshTokenEventHandler"/>类型的新实例
    /// </summary>
    public Logout_RemoveRefreshTokenEventHandler(UserManager<User> userManager, IPrincipal principal)
    {
        _userManager = userManager;
        _principal = principal;
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    public override void Handle(LogoutEventData eventData)
    {
        HandleAsync(eventData).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 异步事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    /// <param name="cancelToken">异步取消标识</param>
    /// <returns>是否成功</returns>
    public override async Task HandleAsync(LogoutEventData eventData, CancellationToken cancelToken = default(CancellationToken))
    {
        ClaimsIdentity identity = _principal.Identity as ClaimsIdentity;
        if (identity?.IsAuthenticated != true)
        {
            return;
        }

        string clientId = identity.GetClaimValueFirstOrDefault("clientId");
        if (clientId == null)
        {
            return;
        }

        await _userManager.RemoveRefreshToken(eventData.UserId.ToString(), clientId);
    }
}
