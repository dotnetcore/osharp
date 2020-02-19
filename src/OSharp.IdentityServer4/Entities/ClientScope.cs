using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 实体类：客户端作用域
    /// </summary>
    [Description("客户端作用域")]
    [TableNamePrefix("Id4")]
    public class ClientScope : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 作用域
        /// </summary>
        [DisplayName("作用域"), StringLength(200), Required]
        public string Scope { get; set; }

        /// <summary>
        /// 获取或设置 所属客户端编号
        /// </summary>
        [DisplayName("客户端编号")]
        public int ClientId { get; set; }

        /// <summary>
        /// 获取或设置 所属客户端
        /// </summary>
        [DisplayName("客户端")]
        public virtual Client Client { get; set; }
    }
}
