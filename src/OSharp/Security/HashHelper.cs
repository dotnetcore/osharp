// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:05 15:18</last-date>
// -----------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;

using OSharp.Extensions;


namespace OSharp.Security
{
    /// <summary>
    /// 字符串Hash操作类
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// 获取字符串的MD5哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string GetMd5(string value, Encoding encoding = null)
        {
            value.CheckNotNull("value");
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = encoding.GetBytes(value);
            return GetMd5(bytes);
        }

        /// <summary>
        /// 获取字节数组的MD5哈希值
        /// </summary>
        public static string GetMd5(byte[] bytes)
        {
            bytes.CheckNotNullOrEmpty("bytes");
            StringBuilder sb = new StringBuilder();
            MD5 hash = new MD5CryptoServiceProvider();
            bytes = hash.ComputeHash(bytes);
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的SHA1哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string GetSha1(string value, Encoding encoding = null)
        {
            value.CheckNotNullOrEmpty("value");

            StringBuilder sb = new StringBuilder();
            SHA1Managed hash = new SHA1Managed();
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的Sha256哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string GetSha256(string value, Encoding encoding = null)
        {
            value.CheckNotNullOrEmpty("value");

            StringBuilder sb = new StringBuilder();
            SHA256Managed hash = new SHA256Managed();
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的Sha512哈希值，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string GetSha512(string value, Encoding encoding = null)
        {
            value.CheckNotNullOrEmpty("value");

            StringBuilder sb = new StringBuilder();
            SHA512Managed hash = new SHA512Managed();
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}