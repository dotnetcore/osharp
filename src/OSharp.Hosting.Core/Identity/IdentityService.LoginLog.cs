// -----------------------------------------------------------------------
//  <copyright file="IdentityService.LoginLog.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-22 14:08</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;

namespace OSharp.Hosting.Identity;

public partial class IdentityService
{
    /// <summary>
    /// 获取 登录日志信息查询数据集
    /// </summary>
    public IQueryable<LoginLog> LoginLogs => LoginLogReqRepository.QueryAsNoTracking();

    /// <summary>
    /// 删除登录日志信息信息
    /// </summary>
    /// <param name="ids">要删除的登录日志信息编号</param>
    /// <returns>业务操作结果</returns>
    public Task<OperationResult> DeleteLoginLogs(params long[] ids)
    {
        Check.NotNull(ids, nameof(ids));
            
        return LoginLogReqRepository.DeleteAsync(ids);
    }
}
