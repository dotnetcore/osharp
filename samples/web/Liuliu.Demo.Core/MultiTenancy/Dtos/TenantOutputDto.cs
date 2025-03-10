using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Entities;
using OSharp.Entity;
using OSharp.Mapping;

namespace Liuliu.Demo.MultiTenancy.Dtos
{
    /// <summary>
    /// 输入DTO：租户信息
    /// </summary>
    [MapFrom(typeof(Tenant))]
    [Description("租户信息")]
    public class TenantOutputDto : IOutputDto
    {
        /// <summary>
        /// 初始化一个<see cref="TenantOutputDto"/>类型的新实例
        /// </summary>
        public TenantOutputDto()
        { }

        /// <summary>
        /// 初始化一个<see cref="TenantOutputDto"/>类型的新实例
        /// </summary>
        public TenantOutputDto(Tenant entity)
        {
            Id = entity.Id;
            TenantKey = entity.TenantKey;
            Name = entity.Name;
            ShortName = entity.ShortName;
            Host = entity.Host;
            ConnectionString = entity.ConnectionString;
            IsEnabled = entity.IsEnabled;
            ExpireDate = entity.ExpireDate;
            CustomJson = entity.CustomJson;
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
        /// 获取或设置 连接字符串
        /// </summary>
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
        public DateTime CreatedTime { get; set; }
    }
}
