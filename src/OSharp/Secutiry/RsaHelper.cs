// -----------------------------------------------------------------------
//  <copyright file="RsaHelper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 14:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;

using OSharp.Properties;


namespace OSharp.Secutiry
{
    /// <summary>
    /// RSA加密解密操作类
    /// </summary>
    public class RsaHelper
    {
        /// <summary>
        /// 初始化一个<see cref="RsaHelper"/>类的新实例
        /// </summary>
        public RsaHelper()
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            PublicKey = provider.ToXmlString(false);
            PrivateKey = provider.ToXmlString(true);
        }

        /// <summary>
        /// 获取 公钥
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        /// 获取 私钥
        /// </summary>
        public string PrivateKey { get; }

        #region 私有方法

        private static void HashTypeRequired(string hashType)
        {
            hashType = hashType.ToUpper();
            hashType.Required(type => type == "MD5" || type == "SHA1", Resources.Security_RSA_Sign_HashType);
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 加密字节数组
        /// </summary>
        public byte[] Encrypt(byte[] source)
        {
            source.CheckNotNull("source");
            return Encrypt(source, PublicKey);
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        public byte[] Decrypt(byte[] source)
        {
            source.CheckNotNull("source");
            return Decrypt(source, PrivateKey);
        }

        /// <summary>
        /// 对明文进行签名，返回明文签名的字节数组
        /// </summary>
        /// <param name="source">要签名的明文字节数组</param>
        /// <param name="hashType">哈希类型，必须为 MD5 或 SHA1</param>
        /// <returns>明文签名的字节数组</returns>
        public byte[] SignData(byte[] source, string hashType)
        {
            source.CheckNotNull("source");
            hashType.CheckNotNullOrEmpty("hashType");
            HashTypeRequired(hashType);

            return SignData(source, hashType, PrivateKey);
        }

        /// <summary>
        /// 验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密的明文字节数组</param>
        /// <param name="signData">明文签名字节数组</param>
        /// <param name="hashType">哈希类型，必须为 MD5 或 SHA1</param>
        /// <returns>验证是否通过</returns>
        public bool VerifyData(byte[] source, byte[] signData, string hashType)
        {
            source.CheckNotNull("source");
            signData.CheckNotNull("signData");
            hashType.CheckNotNullOrEmpty("hashType");
            HashTypeRequired(hashType);

            return VerifyData(source, signData, hashType, PublicKey);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        public string Encrypt(string source)
        {
            source.CheckNotNull("source");
            return Encrypt(source, PublicKey);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        public string Decrypt(string source)
        {
            source.CheckNotNullOrEmpty("source");
            return Decrypt(source, PrivateKey);
        }

        /// <summary>
        /// 对明文进行签名，返回明文签名的BASE64字符串
        /// </summary>
        /// <param name="source">要签名的明文</param>
        /// <param name="hashType">哈希类型，必须为 MD5 或 SHA1</param>
        /// <returns>明文签名的BASE64字符串</returns>
        public string SignData(string source, string hashType)
        {
            source.CheckNotNull("source");
            hashType.CheckNotNullOrEmpty("hashType");
            HashTypeRequired(hashType);

            return SignData(source, hashType, PrivateKey);
        }

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密后的明文</param>
        /// <param name="signData">明文的签名</param>
        /// <param name="hashType">哈希类型，必须为 MD5 或 SHA1</param>
        /// <returns>验证是否通过</returns>
        public bool VerifyData(string source, string signData, string hashType)
        {
            source.CheckNotNull("source");
            signData.CheckNotNullOrEmpty("signData");
            hashType.CheckNotNullOrEmpty("hashType");
            HashTypeRequired(hashType);

            return VerifyData(source, signData, hashType, PublicKey);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 使用指定公钥加密字节数组
        /// </summary>
        public static byte[] Encrypt(byte[] source, string publicKey)
        {
            source.CheckNotNull("source");
            publicKey.CheckNotNullOrEmpty("publicKey");

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKey);
            return provider.Encrypt(source, false);
        }

        /// <summary>
        /// 使用私钥解密字节数组
        /// </summary>
        public static byte[] Decrypt(byte[] source, string privateKey)
        {
            source.CheckNotNull("source");
            privateKey.CheckNotNullOrEmpty("privateKey");

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(privateKey);
            return provider.Decrypt(source, false);
        }

        /// <summary>
        /// 使用指定私钥对明文进行签名，返回明文签名的字节数组
        /// </summary>
        /// <param name="source">要签名的明文字节数组</param>
        /// <param name="hashType">哈希类型，必须为 MD5 或 SHA1</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>明文签名的字节数组</returns>
        public static byte[] SignData(byte[] source, string hashType, string privateKey)
        {
            source.CheckNotNull("source");
            hashType.CheckNotNullOrEmpty("hashType");
            HashTypeRequired(hashType);
            privateKey.CheckNotNullOrEmpty("privateKey");

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(privateKey);
            return provider.SignData(source, hashType);
        }

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密的明文字节数组</param>
        /// <param name="signData">明文签名字节数组</param>
        /// <param name="hashType">哈希类型，必须为 MD5 或 SHA1</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>验证是否通过</returns>
        public static bool VerifyData(byte[] source, byte[] signData, string hashType, string publicKey)
        {
            source.CheckNotNull("source");
            hashType.CheckNotNullOrEmpty("hashType");
            HashTypeRequired(hashType);
            signData.CheckNotNull("signData");

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKey);
            return provider.VerifyData(source, hashType, signData);
        }

        /// <summary>
        /// 使用指定公钥加密字符串
        /// </summary>
        public static string Encrypt(string source, string publicKey)
        {
            source.CheckNotNull("source");
            publicKey.CheckNotNullOrEmpty("publicKey");

            byte[] bytes = Encoding.UTF8.GetBytes(source);
            bytes = Encrypt(bytes, publicKey);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 使用指定私钥解密字符串
        /// </summary>
        public static string Decrypt(string source, string privateKey)
        {
            source.CheckNotNullOrEmpty("source");
            privateKey.CheckNotNullOrEmpty("privateKey");

            byte[] bytes = Convert.FromBase64String(source);
            bytes = Decrypt(bytes, privateKey);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 使用指定私钥签名字符串
        /// </summary>
        /// <param name="source">要签名的字符串</param>
        /// <param name="hashType">哈希类型，必须为 MD5 或 SHA1</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string SignData(string source, string hashType, string privateKey)
        {
            source.CheckNotNull("source");
            hashType.CheckNotNullOrEmpty("hashType");
            HashTypeRequired(hashType);

            byte[] bytes = Encoding.UTF8.GetBytes(source);
            byte[] signBytes = SignData(bytes, hashType, privateKey);
            return Convert.ToBase64String(signBytes);
        }

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名的BASE64字符串</param>
        /// <param name="hashType">哈希类型，必须为 MD5 或 SHA1</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>验证是否通过</returns>
        public static bool VerifyData(string source, string signData, string hashType, string publicKey)
        {
            source.CheckNotNull("source");
            signData.CheckNotNullOrEmpty("signData");
            hashType.CheckNotNullOrEmpty("hashType");
            HashTypeRequired(hashType);

            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            byte[] signBytes = Convert.FromBase64String(signData);
            return VerifyData(sourceBytes, signBytes, hashType, publicKey);
        }

        #endregion
    }
}