// -----------------------------------------------------------------------
//  <copyright file="StreamExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-12 18:27</last-date>
// -----------------------------------------------------------------------

using System.IO;
using System.Text;


namespace OSharp.Extensions
{
    /// <summary>
    /// Stream扩展方法
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 把<see cref="Stream"/>转换为<see cref="string"/>
        /// </summary>
        public static string ToString2(this Stream stream, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            using (StreamReader reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}