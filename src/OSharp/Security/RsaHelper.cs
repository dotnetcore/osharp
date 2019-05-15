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
using System.Xml;

using OSharp.Extensions;


namespace OSharp.Security
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
            RSA rsa = RSA.Create();
            PublicKey = rsa.ToXmlString2(false);
            PrivateKey = rsa.ToXmlString2(true);
        }

        /// <summary>
        /// 获取 公钥
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        /// 获取 私钥
        /// </summary>
        public string PrivateKey { get; }
        
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
        /// <returns>明文签名的字节数组</returns>
        public byte[] SignData(byte[] source)
        {
            source.CheckNotNull("source");

            return SignData(source, PrivateKey);
        }

        /// <summary>
        /// 验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密的明文字节数组</param>
        /// <param name="signData">明文签名字节数组</param>
        /// <returns>验证是否通过</returns>
        public bool VerifyData(byte[] source, byte[] signData)
        {
            source.CheckNotNull("source");
            signData.CheckNotNull("signData");

            return VerifyData(source, signData, PublicKey);
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
        /// <returns>明文签名的BASE64字符串</returns>
        public string SignData(string source)
        {
            source.CheckNotNull("source");

            return SignData(source, PrivateKey);
        }

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密后的明文</param>
        /// <param name="signData">明文的签名</param>
        /// <returns>验证是否通过</returns>
        public bool VerifyData(string source, string signData)
        {
            source.CheckNotNull("source");
            signData.CheckNotNullOrEmpty("signData");

            return VerifyData(source, signData, PublicKey);
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

            RSA rsa = RSA.Create();
            rsa.FromXmlString2(publicKey);
            return rsa.Encrypt(source, RSAEncryptionPadding.Pkcs1);
        }

        /// <summary>
        /// 使用私钥解密字节数组
        /// </summary>
        public static byte[] Decrypt(byte[] source, string privateKey)
        {
            source.CheckNotNull("source");
            privateKey.CheckNotNullOrEmpty("privateKey");

            RSA rsa = RSA.Create();
            rsa.FromXmlString2(privateKey);
            return rsa.Decrypt(source, RSAEncryptionPadding.Pkcs1);
        }

        /// <summary>
        /// 使用指定私钥对明文进行签名，返回明文签名的字节数组
        /// </summary>
        /// <param name="source">要签名的明文字节数组</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>明文签名的字节数组</returns>
        public static byte[] SignData(byte[] source, string privateKey)
        {
            source.CheckNotNull("source");
            privateKey.CheckNotNullOrEmpty("privateKey");

            RSA rsa = RSA.Create();
            rsa.FromXmlString2(privateKey);
            return rsa.SignData(source, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密的明文字节数组</param>
        /// <param name="signData">明文签名字节数组</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>验证是否通过</returns>
        public static bool VerifyData(byte[] source, byte[] signData, string publicKey)
        {
            source.CheckNotNull("source");
            signData.CheckNotNull("signData");

            RSA rsa = RSA.Create();
            rsa.FromXmlString2(publicKey);
            return rsa.VerifyData(source, signData, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
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
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string SignData(string source, string privateKey)
        {
            source.CheckNotNull("source");

            byte[] bytes = Encoding.UTF8.GetBytes(source);
            byte[] signBytes = SignData(bytes, privateKey);
            return Convert.ToBase64String(signBytes);
        }

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名的BASE64字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>验证是否通过</returns>
        public static bool VerifyData(string source, string signData, string publicKey)
        {
            source.CheckNotNull("source");
            signData.CheckNotNullOrEmpty("signData");

            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            byte[] signBytes = Convert.FromBase64String(signData);
            return VerifyData(sourceBytes, signBytes, publicKey);
        }

        #endregion
    }


    internal static class RSAKeyExtensions
    {
        //#region JSON
        //internal static void FromJsonString(this RSA rsa, string jsonString)
        //{
        //    jsonString.CheckNotNullOrEmpty("jsonString" );
        //    try
        //    {
        //        var paramsJson = JsonConvert.DeserializeObject<RSAParametersJson>(jsonString);

        //        RSAParameters parameters = new RSAParameters();

        //        parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
        //        parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
        //        parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
        //        parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
        //        parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
        //        parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
        //        parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
        //        parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
        //        rsa.ImportParameters(parameters);
        //    }
        //    catch
        //    {
        //        throw new Exception("Invalid JSON RSA key.");
        //    }
        //}

        //internal static string ToJsonString(this RSA rsa, bool includePrivateParameters)
        //{
        //    RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

        //    var parasJson = new RSAParametersJson()
        //    {
        //        Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
        //        Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
        //        P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
        //        Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
        //        DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
        //        DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
        //        InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
        //        D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
        //    };

        //    return JsonConvert.SerializeObject(parasJson);
        //}
        //#endregion

        #region XML

        public static void FromXmlString2(this RSA rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }

        public static string ToXmlString2(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                  parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                  parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                  parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                  parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                  parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                  parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                  parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                  parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
        }

        #endregion
    }
}