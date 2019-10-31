// -----------------------------------------------------------------------
//  <copyright file="HttpEncryptOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-31 0:06</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Options
{
    /// <summary>
    /// Http通信加密选项
    /// </summary>
    public class HttpEncryptOptions
    {
        /// <summary>
        /// 获取或设置 服务端私钥
        /// </summary>
        public string HostPrivateKey { get; set; }

        /// <summary>
        /// 获取或设置 客户端公钥
        /// </summary>
        public string ClientPublicKey { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}