// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:05 13:48</last-date>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using OSharp.Properties;


namespace OSharp.Secutiry
{
    /// <summary>
    /// DES / TripleDES加密解密操作类
    /// </summary>
    public class DesHelper
    {
        private const int BufferAppendSize = 64;
        private const string SectionSign = "?SECTION?";
        private readonly bool _isTriple;
        private readonly byte[] _key;

        /// <summary>
        /// 使用随机密码初始化一个<see cref="DesHelper"/>类的新实例
        /// </summary>
        /// <param name="isTriple">是否使用TripleDES方式，否则为DES方式</param>
        public DesHelper(bool isTriple = false)
            : this(isTriple
                ? new TripleDESCryptoServiceProvider().Key
                : new DESCryptoServiceProvider().Key)
        {
            _isTriple = isTriple;
        }

        /// <summary>
        /// 使用指定8位或24位密码初始化一个<see cref="DesHelper"/>类的新实例
        /// </summary>
        public DesHelper(byte[] key)
        {
            key.CheckNotNull("key");
            key.Required(k => k.Length == 8 || k.Length == 24, string.Format(Resources.Security_DES_KeyLenght, key.Length));
            _key = key;
            _isTriple = key.Length == 24;
        }

        /// <summary>
        /// 获取 密钥
        /// </summary>
        public byte[] Key { get { return _key; } }

        #region 实例方法

        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="source">要加密的字节数组</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] source)
        {
            source.CheckNotNull("source");
            SymmetricAlgorithm provider = _isTriple
                ? (SymmetricAlgorithm)new TripleDESCryptoServiceProvider { Key = _key, Mode = CipherMode.ECB }
                : new DESCryptoServiceProvider { Key = _key, Mode = CipherMode.ECB };

            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(source, 0, source.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="source">要解密的字节数组</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] source)
        {
            source.CheckNotNull("source");

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            SymmetricAlgorithm provider = _isTriple
                ? (SymmetricAlgorithm)new TripleDESCryptoServiceProvider { Key = _key, Mode = CipherMode.ECB }
                : new DESCryptoServiceProvider { Key = _key, Mode = CipherMode.ECB };
            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(source, 0, source.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }
        
        /// <summary>
        /// 加密字符串，输出BASE64编码字符串
        /// </summary>
        /// <param name="source">要加密的明文字符串</param>
        /// <returns>加密的BASE64编码的字符串</returns>
        public string Encrypt(string source)
        {
            source.CheckNotNull("source");
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(Encrypt(bytes));
        }

        /// <summary>
        /// 解密字符串，输入为BASE64编码字符串
        /// </summary>
        /// <param name="source">要解密的BASE64编码的字符串</param>
        /// <returns>明文字符串</returns>
        public string Decrypt(string source)
        {
            source.CheckNotNullOrEmpty("source");
            byte[] bytes = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(Decrypt(bytes));
        }

        /// <summary>
        /// 整体加密文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        public void EncryptFile(string sourceFile, string targetFile)
        {
            sourceFile.CheckFileExists("sourceFile");
            targetFile.CheckNotNullOrEmpty("targetFile");

            using (FileStream ifs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read),
                ofs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                long length = ifs.Length;
                byte[] sourceBytes = new byte[length];
                ifs.Read(sourceBytes, 0, sourceBytes.Length);
                byte[] targetBytes = Encrypt(sourceBytes);
                ofs.Write(targetBytes, 0, targetBytes.Length);
            }
        }

        /// <summary>
        /// 分段加密文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        /// <param name="sectionLength">分段大小（字节）</param>
        public void EncryptFile(string sourceFile, string targetFile, int sectionLength)
        {
            sourceFile.CheckFileExists("sourceFile");
            targetFile.CheckNotNullOrEmpty("targetFile");
            sectionLength.CheckGreaterThan("sectionLength", 0);

            using (FileStream ifs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read),
                ofs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                //追加附加数据到加密文件开关
                long decryptFileSize = ifs.Length;
                byte[] appendBytes = new byte[BufferAppendSize];

                //0位为加密分段大小，1位为未加密文件的长度
                //附加信息格式为：{分段长度}|{明文文件长度}|{结束0标记}
                string appendStr = "{0}|{1}|{2}".FormatWith(sectionLength, decryptFileSize, 0);
                appendStr = Encrypt(appendStr);
                int sectionSignSize = (SectionSign + "|").Length;
                //密文附加信息长度
                int appendStrSize = appendStr.Length + sectionSignSize;
                //附加信息格式为：{附加信息长度}|{分段长度}|{明文文件长度}|{结束0标记}
                appendStr = "{0}|{1}".FormatWith(appendStrSize, appendStr);

                //在文件最开关添加分段标记Section_Sign，说明文件是分段加密文件
                //附加串信息格式为：{分段加密标记}|{附加信息长度}|{分段长度}|{明文文件长度}|{结束0标记}
                appendStr = "{0}|{1}".FormatWith(SectionSign, appendStr);
                appendStr = appendStr.Replace("|" + appendStrSize.ToString(CultureInfo.InvariantCulture) + "|",
                    "|" + appendStr.Length.ToString(CultureInfo.InvariantCulture) + "|");
                byte[] tmpBytes = Encoding.UTF8.GetBytes(appendStr);
                using (MemoryStream ms = new MemoryStream(appendBytes))
                {
                    ms.Write(tmpBytes, 0, tmpBytes.Length);
                    appendBytes = ms.ToArray();
                }

                ofs.Seek(0, SeekOrigin.Begin);
                ofs.Write(appendBytes, 0, appendBytes.Length);

                long fileSize = ifs.Length;
                long sectionCount = fileSize / sectionLength;
                int lastLength = (int)(fileSize % sectionLength);

                int length;
                byte[] sourceBytes = new byte[sectionLength];
                if (sectionCount > 0)
                {
                    length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                }
                else
                {
                    sourceBytes = new byte[lastLength];
                    length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                }
                while (length > 0)
                {
                    byte[] targetBytes = Encrypt(sourceBytes);
                    ofs.Write(targetBytes, 0, targetBytes.Length);
                    sectionCount--;
                    if (sectionCount > 0)
                    {
                        length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    }
                    else
                    {
                        sourceBytes = new byte[lastLength];
                        length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 对文件内容进行DES解密，能自动识别并处理是否为分段加密
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        public void DecryptFile(string sourceFile, string targetFile)
        {
            sourceFile.CheckFileExists("sourceFile");
            targetFile.CheckNotNullOrEmpty("targetFile");

            using (FileStream ifs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read),
                ofs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                //判断是否分段加密
                bool isSection = CheckSectionSign(ifs);
                if (!isSection)
                {
                    ifs.Seek(0, SeekOrigin.Begin);
                    long length = ifs.Length;
                    byte[] sourceBytes = new byte[length];
                    ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    byte[] targetBytes = Decrypt(sourceBytes);
                    ofs.Write(targetBytes, 0, targetBytes.Length);
                }
                else
                {
                    //从加密文件中读取附加信息，获取加密时分段大小
                    ifs.Seek(0, SeekOrigin.Begin);
                    byte[] appendBytes = new byte[BufferAppendSize];
                    ifs.Read(appendBytes, 0, appendBytes.Length);
                    string appendStr = Encoding.UTF8.GetString(appendBytes);
                    string tmpAppend = appendStr.Substring(SectionSign.Length + 1);
                    int appendStrSize = Convert.ToInt32(tmpAppend.Substring(0, tmpAppend.IndexOf("|", StringComparison.Ordinal)));
                    appendStr = appendStr.Substring(0, appendStrSize);
                    tmpAppend = appendStr.Substring(appendStr.LastIndexOf("|", StringComparison.Ordinal) + 1,
                        appendStr.Length - appendStr.LastIndexOf("|", StringComparison.Ordinal) - 1);
                    appendStr = Decrypt(tmpAppend);
                    int sectionLength = Convert.ToInt32(appendStr.Split('|')[0]);
                    ifs.Seek(BufferAppendSize, SeekOrigin.Begin); //把文件读取指针移到附加信息后面
                    sectionLength = Encrypt(new byte[sectionLength]).Length;
                    long fileSize = ifs.Length;
                    fileSize -= BufferAppendSize;
                    long sectionCount = fileSize / sectionLength; //段数
                    int laseLength = (int)(fileSize % sectionLength); //最后一段长度
                    int length;
                    byte[] sourceBytes = new byte[sectionLength]; //加密数据缓冲区
                    if (sectionCount > 0)
                    {
                        length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    }
                    else
                    {
                        sourceBytes = new byte[laseLength];
                        length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    }
                    while (length > 0)
                    {
                        byte[] targetBytes = Decrypt(sourceBytes);
                        ofs.Write(targetBytes, 0, targetBytes.Length);
                        sectionCount--;
                        if (sectionCount > 0)
                        {
                            length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                        }
                        else if (sectionCount == 0)
                        {
                            sourceBytes = new byte[laseLength];
                            length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                        }
                        else
                        {
                            length = 0;
                        }
                    }
                }
            }
        }

        private static bool CheckSectionSign(Stream ifs)
        {
            int sectionSignSize = SectionSign.Length;
            ifs.Seek(0, SeekOrigin.Begin);
            byte[] sectionSignBytes = new byte[sectionSignSize];
            ifs.Read(sectionSignBytes, 0, sectionSignBytes.Length);
            string sectionSignString = Encoding.UTF8.GetString(sectionSignBytes);
            return sectionSignString == SectionSign;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="source">要加密的字节数组</param>
        /// <param name="key">密钥字节数组，长度为8或者24</param>
        /// <returns>加密后的字节数组</returns>
        public static byte[] Encrypt(byte[] source, byte[] key)
        {
            DesHelper des = new DesHelper(key);
            return des.Encrypt(source);
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="source">要解密的字节数组</param>
        /// <param name="key">密钥字节数组，长度为8或者24</param>
        /// <returns>解密后的字节数组</returns>
        public static byte[] Decrypt(byte[] source, byte[] key)
        {
            DesHelper des = new DesHelper(key);
            return des.Decrypt(source);
        }

        /// <summary>
        /// 加密字符串，输出BASE64编码字符串
        /// </summary>
        /// <param name="source">要加密的明文字符串</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        /// <returns>加密的BASE64编码的字符串</returns>
        public static string Encrypt(string source, string key)
        {
            source.CheckNotNull("source");
            key.CheckNotNullOrEmpty("key");
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            DesHelper des = new DesHelper(keyBytes);
            return des.Encrypt(source);
        }

        /// <summary>
        /// 解密字符串，输入BASE64编码字符串
        /// </summary>
        /// <param name="source">要解密的BASE64编码字符串</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        /// <returns>解密的明文字符串</returns>
        public static string Decrypt(string source, string key)
        {
            source.CheckNotNullOrEmpty("source");
            key.CheckNotNullOrEmpty("key");
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            DesHelper des = new DesHelper(keyBytes);
            return des.Decrypt(source);
        }

        /// <summary>
        /// 整体加密文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        public static void EncryptFile(string sourceFile, string targetFile, string key)
        {
            sourceFile.CheckFileExists("sourceFile");
            targetFile.CheckNotNullOrEmpty("targetFile");
            key.CheckNotNullOrEmpty("key");

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            DesHelper des = new DesHelper(keyBytes);
            des.EncryptFile(sourceFile, targetFile);
        }

        /// <summary>
        /// 分段加密文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        /// <param name="sectionLength">分段大小（字节）</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        public static void EncryptFile(string sourceFile, string targetFile, int sectionLength, string key)
        {
            sourceFile.CheckFileExists("sourceFile");
            targetFile.CheckNotNullOrEmpty("targetFile");
            key.CheckNotNullOrEmpty("key");
            sectionLength.CheckGreaterThan("sectionLength", 0);

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            DesHelper des = new DesHelper(keyBytes);
            des.EncryptFile(sourceFile, targetFile, sectionLength);
        }

        /// <summary>
        /// 对文件内容进行DES解密，能自动识别并处理是否为分段加密
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        public static void DecryptFile(string sourceFile, string targetFile, string key)
        {
            sourceFile.CheckFileExists("sourceFile");
            targetFile.CheckNotNullOrEmpty("targetFile");
            key.CheckNotNullOrEmpty("key");

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            DesHelper des = new DesHelper(keyBytes);
            des.DecryptFile(sourceFile, targetFile);
        }

        #endregion
    }
}