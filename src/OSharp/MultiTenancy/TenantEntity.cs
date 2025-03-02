using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OSharp.Entity;

namespace OSharp.MultiTenancy
{
    /// <summary>
    /// 租户数据库实体
    /// </summary>
    [Description("租户信息")]
    public class TenantEntity : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 租户ID
        /// </summary>
        [Required, StringLength(50)]
        [Description("租户ID")]
        public string TenantId { get; set; }

        /// <summary>
        /// 获取或设置 租户名称
        /// </summary>
        [Required, StringLength(100)]
        [Description("租户名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 租户主机
        /// </summary>
        [Required, StringLength(100)]
        [Description("租户主机")]
        public string Host { get; set; }

        /// <summary>
        /// 获取或设置 连接字符串
        /// </summary>
        [StringLength(1000)]
        [Description("连接字符串")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        [Description("是否启用")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 更新时间
        /// </summary>
        [Description("更新时间")]
        public DateTime? UpdatedTime { get; set; }
    }
} 