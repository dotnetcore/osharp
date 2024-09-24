// -----------------------------------------------------------------------
//  <copyright file="Organization.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hosting.Identity.Entities;

/// <summary>
/// 实体类：组织机构
/// </summary>
[Description("组织机构信息")]
[TableNamePrefix("Identity")]
public class Organization : EntityBase<long>, ILockable, ICreatedTime
{
    /// <summary>
    /// 获取或设置 名称
    /// </summary>
    [Required, DisplayName("名称"), StringLength(200)]
    public string Name { get; set; }

    /// <summary>
    /// 获取或设置 编码
    /// </summary>
    [Required, DisplayName("编码"), StringLength(200)]
    public string Code { get; set; }

    /// <summary>
    /// 获取或设置 描述
    /// </summary>
    [DisplayName("描述"), StringLength(500)]
    public string Remark { get; set; }

    /// <summary>
    /// 获取或设置 父组织机构
    /// </summary>
    [DisplayName("父组织机构编号")]
    public long? ParentId { get; set; }

    /// <summary>
    /// 获取或设置 是否锁定当前信息
    /// </summary>
    [DisplayName("是否锁定")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// 获取或设置 创建时间
    /// </summary>
    [DisplayName("创建时间")]
    public DateTime CreatedTime { get; set; }
}
