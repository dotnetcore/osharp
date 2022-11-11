// -----------------------------------------------------------------------
//  <copyright file="AuditEntity.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:04</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hosting.Systems.Entities;

/// <summary>
/// 实体类：审计实体信息
/// </summary>
[MapFrom(typeof(AuditEntityEntry))]
[TableNamePrefix("Systems")]
[Description("审计实体信息")]
public class AuditEntity : EntityBase<long>
{
    /// <summary>
    /// 初始化一个<see cref="AuditEntityEntry"/>类型的新实例
    /// </summary>
    public AuditEntity()
        : this(null, null, OperateType.Query)
    { }

    /// <summary>
    /// 初始化一个<see cref="AuditEntityEntry"/>类型的新实例
    /// </summary>
    public AuditEntity(string name, string typeName, OperateType operateType)
    {
        Name = name;
        TypeName = typeName;
        OperateType = operateType;
    }

    /// <summary>
    /// 获取或设置 实体名称
    /// </summary>
    [DisplayName("实体名称"), Required, StringLength(200)]
    public string Name { get; set; }

    /// <summary>
    /// 获取或设置 类型名称
    /// </summary>
    [DisplayName("类型名称"), Required, StringLength(500)]
    public string TypeName { get; set; }

    /// <summary>
    /// 获取或设置 数据编号
    /// </summary>
    [DisplayName("类型名称"), StringLength(200)]
    public string EntityKey { get; set; }

    /// <summary>
    /// 获取或设置 操作类型
    /// </summary>
    public OperateType OperateType { get; set; }

    /// <summary>
    /// 获取或设置 所属审计操作编号
    /// </summary>
    public long OperationId { get; set; }

    /// <summary>
    /// 获取或设置 所属审计操作
    /// </summary>
    public virtual AuditOperation Operation { get; set; }

    /// <summary>
    /// 获取或设置 审计实体属性集合
    /// </summary>
    public virtual ICollection<AuditProperty> Properties { get; set; } = new List<AuditProperty>();
}
