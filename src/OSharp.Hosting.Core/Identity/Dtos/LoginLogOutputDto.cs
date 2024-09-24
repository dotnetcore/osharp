// -----------------------------------------------------------------------
//  <copyright file="LoginLogOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-22 14:23</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Identity.Dtos;

/// <summary>
/// 输出DTO：登录日志
/// </summary>
[MapFrom(typeof(LoginLog))]
public class LoginLogOutputDto : IOutputDto
{
    /// <summary>
    /// 初始化一个<see cref="LoginLogOutputDto"/>类型的新实例
    /// </summary>
    public LoginLogOutputDto()
    { }

    /// <summary>
    /// 初始化一个<see cref="LoginLogOutputDto"/>类型的新实例
    /// </summary>
    public LoginLogOutputDto(LoginLog e)
    {
        Id = e.Id;
        Ip = e.Ip;
        UserAgent = e.UserAgent;
        LogoutTime = e.LogoutTime;
        CreatedTime = e.CreatedTime;
        UserId = e.UserId;
    }

    /// <summary>
    /// 获取或设置 编号
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 获取或设置 登录IP
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// 获取或设置 用户代理头
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// 获取或设置 退出时间
    /// </summary>
    public DateTime? LogoutTime { get; set; }

    /// <summary>
    /// 获取或设置 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// 获取或设置 用户编号
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 获取或设置 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 获取或设置 用户昵称
    /// </summary>
    public string NickName { get; set; }
}
