using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Entities;
using OSharp.Entity;
using OSharp.Mapping;

namespace Liuliu.Demo.MultiTenancy.Dtos
{
    /// <summary>
    /// 输入DTO：站内信信息
    /// </summary>
    [MapTo(typeof(Tenant))]
    [Description("租户信息")]
    public class TenantInputDto : IInputDto<long>
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        public long Id { get; set; }

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
    }

}
