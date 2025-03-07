using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Entity;

namespace Liuliu.Demo.MultiTenancy.Entities
{
    /// <summary>
    /// 租户
    /// </summary>
    [TableNamePrefix("MultiTenancy")]
    public class Tenant : EntityBase<long>, ICreatedTime, ISoftDeletable
    {
        /// <summary>
        /// 获取或设置 租户标识
        /// </summary>
        [Required, StringLength(50)]
        [Description("租户标识")]
        public string TenantKey { get; set; }

        /// <summary>
        /// 获取或设置 租户名称
        /// </summary>
        [Required, StringLength(100)]
        [Description("租户名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 租户简称
        /// </summary>
        [Required, StringLength(50)]
        [Description("租户简称")]
        public string ShortName { get; set; }

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
        /// 获取或设置 到期时间
        /// </summary>
        [Description("到期时间")]
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 获取或设置 自定义配置数据
        /// </summary>
        [Description("自定义配置数据")]
        public string CustomJson { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 数据逻辑删除时间，为null表示正常数据，有值表示已逻辑删除，同时删除时间每次不同也能保证索引唯一性
        /// </summary>
        public DateTime? DeletedTime { get; set; }

        /// <summary>
        /// 获取或设置 更新时间
        /// </summary>
        [Description("更新时间")]
        public DateTime? UpdatedTime { get; set; }
    }
}