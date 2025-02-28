using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Core
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; } // 租户域名
        public string ConnectionString { get; set; } // 租户数据库连接字符串
                                                     // 其他租户相关属性
    }
}
