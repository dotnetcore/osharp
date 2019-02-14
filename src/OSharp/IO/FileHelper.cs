// -----------------------------------------------------------------------
//  <copyright file="FileHelper.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-18 18:25</last-date>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace OSharp.IO
{
    /// <summary>
    /// 文件辅助操作类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 创建文件，如果文件不存在
        /// </summary>
        /// <param name="fileName">要创建的文件</param>
        public static void CreateIfNotExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                return;
            }

            string dir = Path.GetDirectoryName(fileName);
            if (dir != null)
            {
                DirectoryHelper.CreateIfNotExists(dir);
            }
            File.Create(fileName);
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="fileName">要删除的文件名</param>
        public static void DeleteIfExists(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            File.Delete(fileName);
        }

        /// <summary>
        /// 设置或取消文件的指定<see cref="FileAttributes"/>属性
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="attribute">要设置的文件属性</param>
        /// <param name="isSet">true为设置，false为取消</param>
        public static void SetAttribute(string fileName, FileAttributes attribute, bool isSet)
        {
            FileInfo fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                throw new FileNotFoundException("要设置属性的文件不存在。", fileName);
            }
            if (isSet)
            {
                fi.Attributes = fi.Attributes | attribute;
            }
            else
            {
                fi.Attributes = fi.Attributes & ~attribute;
            }
        }

        /// <summary>
        /// 获取文件版本号
        /// </summary>
        /// <param name="fileName"> 完整文件名 </param>
        /// <returns> 文件版本号 </returns>
        public static string GetVersion(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
                return fvi.FileVersion;
            }
            return null;
        }

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="fileName"> 文件名 </param>
        /// <returns> 32位MD5 </returns>
        public static string GetFileMd5(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                const int bufferSize = 1024 * 1024;
                byte[] buffer = new byte[bufferSize];
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    md5.Initialize();
                    long offset = 0;
                    while (offset < fs.Length)
                    {
                        long readSize = bufferSize;
                        if (offset + readSize > fs.Length)
                        {
                            readSize = fs.Length - offset;
                        }
                        fs.Read(buffer, 0, (int)readSize);
                        if (offset + readSize < fs.Length)
                        {
                            md5.TransformBlock(buffer, 0, (int)readSize, buffer, 0);
                        }
                        else
                        {
                            md5.TransformFinalBlock(buffer, 0, (int)readSize);
                        }
                        offset += bufferSize;
                    }
                    fs.Close();
                    byte[] result = md5.Hash;
                    md5.Clear();
                    StringBuilder sb = new StringBuilder(32);
                    foreach (byte b in result)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }
    }
}