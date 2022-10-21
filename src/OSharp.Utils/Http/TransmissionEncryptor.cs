// -----------------------------------------------------------------------
//  <copyright file="TransmissionEncryptor.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-30 23:29</last-date>
// -----------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;

using OSharp.Collections;
using OSharp.Extensions;
using OSharp.Security;


namespace OSharp.Http
{
    /// <summary>
    /// 结合RSA，AES的通信传输加密解密操作类，使用AES对数据进行对称加密，使用RSA加密AES的密钥，并对数据进行签名校验，保证数据传输安全与完整性
    /// </summary>
    public class TransmissionEncryptor
    {
        private readonly string _separator = "#@|osharp|@#";
        private readonly string _facePublicKey;
        private readonly string _ownPrivateKey;

        /// <summary>
        /// 初始化一个<see cref="TransmissionEncryptor"/>类型的新实例
        /// </summary>
        /// <param name="ownPrivateKey">己方私钥</param>
        /// <param name="facePublicKey">对方公钥</param>
        public TransmissionEncryptor(string ownPrivateKey, string facePublicKey)
        {
            ownPrivateKey.CheckNotNull("ownPrivateKey");
            facePublicKey.CheckNotNull("facePublicKey");

            _ownPrivateKey = ownPrivateKey;
            _facePublicKey = facePublicKey;
        }

        /// <summary>
        /// 解密接收到的加密数据并验证完整性，如果验证通过返回明文
        /// </summary>
        /// <param name="data">接收到的加密数据</param>
        /// <returns>解密并验证成功后，返回明文</returns>
        public string DecryptAndVerifyData(string data)
        {
            data.CheckNotNullOrEmpty("data");

            string separator = GetSeparator(_separator);
            if (!data.Contains(separator))
            {
                return data;
            }
            string[] separators = { separator };
            //0为AES密钥密文，1为 正文+摘要 的密文
            string[] datas = data.Split(separators, StringSplitOptions.None);
            //用接收端私钥RSA解密获取AES密钥
            byte[] keyBytes = RsaHelper.Decrypt(Convert.FromBase64String(datas[0]), _ownPrivateKey);
            string key = keyBytes.ToString2();
            //AES解密获取 正文+摘要 的明文
            data = new AesHelper(key, true).Decrypt(datas[1]);
            //0为正文明文，1为摘要
            datas = data.Split(separators, StringSplitOptions.None);
            data = datas[0];
            if (RsaHelper.VerifyData(data, datas[1], _facePublicKey))
            {
                return data;
            }

            throw new CryptographicException("加密数据在进行解密时校验失败");
        }

        /// <summary>
        /// 加密要发送的数据，包含签名，AES加密，RSA加密AES密钥等步骤
        /// </summary>
        /// <param name="data">要加密的正文明文数据</param>
        /// <returns>已加密待发送的密文</returns>
        public string EncryptData(string data)
        {
            data.CheckNotNull("data");

            string separator = GetSeparator(_separator);
            //获取正文摘要
            string signData = RsaHelper.SignData(data, _ownPrivateKey);
            data = new[] { data, signData }.ExpandAndToString(separator);
            //使用AES加密 正文+摘要
            AesHelper aes = new AesHelper(true);
            data = aes.Encrypt(data);
            //RSA加密AES密钥
            byte[] keyBytes = aes.Key.ToBytes();
            string enAesKey = Convert.ToBase64String(RsaHelper.Encrypt(keyBytes, _facePublicKey));
            return new[] { enAesKey, data }.ExpandAndToString(separator);
        }

        private static string GetSeparator(string separator)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(separator));
        }
    }
}