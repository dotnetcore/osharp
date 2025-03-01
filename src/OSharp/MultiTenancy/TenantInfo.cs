using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.MultiTenancy
{
    /// <summary>
    /// 租户信息
    /// </summary>
    public class TenantInfo
    {
        /// <summary>
        /// 获取或设置 租户ID
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 获取或设置 租户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 租户主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 获取或设置 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
