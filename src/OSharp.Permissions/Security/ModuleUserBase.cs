using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using OSharp.Entity;


namespace OSharp.Security
{
    /// <summary>
    /// 模块用户信息基类
    /// </summary>
    /// <typeparam name="TModuleKey">模块编号</typeparam>
    /// <typeparam name="TUserKey">用户编号</typeparam>
    public abstract class ModuleUserBase<TModuleKey, TUserKey> : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 模块编号
        /// </summary>
        [DisplayName("模块编号")]
        public TModuleKey ModuleId { get; set; }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public TUserKey UserId { get; set; }
    }
}
