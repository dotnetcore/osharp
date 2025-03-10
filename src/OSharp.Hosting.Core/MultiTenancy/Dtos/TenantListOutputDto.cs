using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Hosting.MultiTenancy.Entities;
using OSharp.Entity;
using OSharp.Mapping;

namespace OSharp.Hosting.MultiTenancy.Dtos
{
    /// <summary>
    /// 输入DTO：租户列表信息
    /// </summary>
    [MapFrom(typeof(Tenant))]
    [Description("租户列表信息")]
    public class TenantListOutputDto : IOutputDto
    {
        /// <summary>
        /// 初始化一个<see cref="TenantListOutputDto"/>类型的新实例
        /// </summary>
        public TenantListOutputDto()
        { }

        /// <summary>
        /// 初始化一个<see cref="TenantListOutputDto"/>类型的新实例
        /// </summary>
        public TenantListOutputDto(Tenant entity)
        {
            Id = entity.Id;
            TenantKey = entity.TenantKey;
            Name = entity.Name;
            ShortName = entity.ShortName;
            Host = entity.Host;
            IsEnabled = entity.IsEnabled;
            ExpireDate = entity.ExpireDate;
            CreatedTime = entity.CreatedTime;
        }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        public long Id { get; set; }

        /// <summary>
        /// 获取或设置 租户标识
        /// </summary>
        [Description("租户标识")]
        public string TenantKey { get; set; }

        /// <summary>
        /// 获取或设置 租户名称
        /// </summary>
        [Description("租户名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 租户简称
        /// </summary>
        [Description("租户简称")]
        public string ShortName { get; set; }

        /// <summary>
        /// 获取或设置 租户主机
        /// </summary>
        [Description("租户主机")]
        public string Host { get; set; }

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
        /// 获取或设置 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}
